// this source from this url.
// http://blogs.msdn.com/cgarcia/archive/2009/08/21/enable-ssl-for-managed-socket-on-windows-mobile.aspx

using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Globalization;

namespace EbiSoft.EbIRC.ExternalLibrary
{
    class SslHelper : IDisposable
    {
        private const ushort SO_SECURE = 0x2001;
        private const ushort SO_SEC_SSL = 0x2004;
        private const int _SO_SSL_FLAGS = 0x02;
        private const int _SO_SSL_VALIDATE_CERT_HOOK = 0x08;
        private const int SO_SSL_FAMILY = 0x00730000;
        private const long _SO_SSL = ((2L << 27) | SO_SSL_FAMILY);
        private const uint IOC_IN = 0x80000000;
        private const long SO_SSL_SET_VALIDATE_CERT_HOOK = (IOC_IN | _SO_SSL | _SO_SSL_VALIDATE_CERT_HOOK);
        private const long SO_SSL_SET_FLAGS = (IOC_IN | _SO_SSL | _SO_SSL_FLAGS);

        private const int SSL_CERT_X59 = 1;
        private const int SSL_ERR_OKAY = 0;
        private const int SSL_ERR_FAILED = 2;
        private const int SSL_ERR_BAD_LEN = 3;
        private const int SSL_ERR_BAD_TYPE = 4;
        private const int SSL_ERR_BAD_DATA = 5;
        private const int SSL_ERR_NO_CERT = 6;
        private const int SSL_ERR_BAD_SIG = 7;
        private const int SSL_ERR_CERT_EXPIRED = 8;
        private const int SSL_ERR_CERT_REVOKED = 9;
        private const int SSL_ERR_CERT_UNKNOWN = 10;
        private const int SSL_ERR_SIGNATURE = 11;
        private const int SSL_CERT_FLAG_ISSUER_UNKNOWN = 0x0001;


        public delegate int SSLVALIDATECERTFUNC(uint dwType, IntPtr pvArg, uint dwChainLen, IntPtr pCertChain, uint dwFlags);
        private IntPtr ptrHost;
        private IntPtr hookFunc;

        public SslHelper(Socket socket, string host)
        {
            //The managed SocketOptionName enum doesn't have SO_SECURE so here we cast the integer value
            socket.SetSocketOption(SocketOptionLevel.Socket, (SocketOptionName)SO_SECURE, SO_SEC_SSL);

            //We need to pass a function pointer and a pointer to a string containing the host
            //to unmanaged code
            hookFunc = Marshal.GetFunctionPointerForDelegate(new SSLVALIDATECERTFUNC(ValidateCert));

            //Allocate the buffer for the string
            ptrHost = Marshal.AllocHGlobal(host.Length + 1);
            WriteASCIIString(ptrHost, host);

            //Now put both pointers into a byte[]
            byte[] inBuffer = new byte[8];
            byte[] hookFuncBytes = BitConverter.GetBytes(hookFunc.ToInt32());
            byte[] hostPtrBytes = BitConverter.GetBytes(ptrHost.ToInt32());
            Array.Copy(hookFuncBytes, inBuffer, hookFuncBytes.Length);
            Array.Copy(hostPtrBytes, 0, inBuffer, hookFuncBytes.Length, hostPtrBytes.Length);

            unchecked
            {
                socket.IOControl((int)SO_SSL_SET_VALIDATE_CERT_HOOK, inBuffer, null);
            }
        }

        private static void WriteASCIIString(IntPtr basePtr, string s)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            for (int i = 0; i < bytes.Length; i++)
                Marshal.WriteByte(basePtr, i, bytes[i]);

            //null terminate the string
            Marshal.WriteByte(basePtr, bytes.Length, 0);
        }

        #region IDisposable Members

        ~SslHelper()
        {
            ReleaseHostPointer();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            ReleaseHostPointer();
        }

        private void ReleaseHostPointer()
        {
            if (ptrHost != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(ptrHost);
                ptrHost = IntPtr.Zero;
            }
        }

        #endregion


        private int ValidateCert(uint dwType, IntPtr pvArg, uint dwChainLen, IntPtr pCertChain, uint dwFlags)
        {
            //According to http://msdn.microsoft.com/en-us/library/ms940451.aspx:
            //
            //- dwChainLen is always 1
            //- Windows CE performs the cert chain validation
            //- pvArg is the context data we passed into the SO_SSL_SET_VALIDATE_CERT_HOOK call so in our
            //- case is the host name
            //
            //So here we are responsible for validating the dates on the certificate and the CN

            if (dwType != SSL_CERT_X59)
                return SSL_ERR_BAD_TYPE;

            //When in debug mode let self-signed certificates through ...
#if !DEBUG
            if ((dwFlags & SSL_CERT_FLAG_ISSUER_UNKNOWN) != 0)
                return SSL_ERR_CERT_UNKNOWN;
#endif

            Debug.Assert(dwChainLen == 1);

            //Note about the note: an unmanaged long is 32 bits, unlike a managed long which is 64. I was missing
            //this fact when I wrote the comment. So the docs are accurate.
            //NOTE: The documentation says pCertChain is a pointer to a LPBLOB struct:
            //
            // {ulong size, byte* data}
            //
            //in reality the size is a 32 bit integer (not 64).
            int certSize = Marshal.ReadInt32(pCertChain);
            IntPtr pData = Marshal.ReadIntPtr(new IntPtr(pCertChain.ToInt32() + sizeof(int)));

            byte[] certData = new byte[certSize];

            for (int i = 0; i < certSize; i++)
                certData[i] = Marshal.ReadByte(pData, (int)i);

            X509Certificate2 cert;
            try
            {
                cert = new X509Certificate2(certData);
            }
            catch (ArgumentException) { return SSL_ERR_BAD_DATA; }
            catch (CryptographicException) { return SSL_ERR_BAD_DATA; }

            //Validate the expiration date
            if (DateTime.Now > DateTime.Parse(cert.GetExpirationDateString(), CultureInfo.CurrentCulture))
                return SSL_ERR_CERT_EXPIRED;

            //Validate the effective date
            if (DateTime.Now < DateTime.Parse(cert.GetEffectiveDateString(), CultureInfo.CurrentCulture))
                return SSL_ERR_FAILED;

            string certName = cert.GetName();
            Debug.WriteLine(certName);

            //Validate the CN
            string host = ReadAnsiString(pvArg);
            if (!certName.Contains("CN=" + host))
                return SSL_ERR_FAILED;

            return SSL_ERR_OKAY;
        }

        private static string ReadAnsiString(IntPtr pvArg)
        {
            byte[] buffer = new byte[1024];
            int j = 0;
            do
            {
                buffer[j] = Marshal.ReadByte(pvArg, j);
                j++;
            } while (buffer[j - 1] != 0);
            string host = Encoding.ASCII.GetString(buffer, 0, j - 1);
            return host;
        }
    }
}