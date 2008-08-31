using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace EbiSoft.Library
{
    public class TextWriterTraceListener : TraceListener, IDisposable
    {
        private StreamWriter writer;
        private bool closed;

        public TextWriterTraceListener(string filename)
        {
            writer = new StreamWriter(filename, true);
            closed = false;
        }

        #region IDisposable ÉÅÉìÉo

        void IDisposable.Dispose()
        {
            Close();
        }

        #endregion

        public override void Close()
        {
            if (!closed)
            {
                writer.Close();
                closed = true;
            }
        }

        public override void Write(string message)
        {
            writer.Write(message);
        }

        public override void WriteLine(string message)
        {
            writer.Write(message);
        }

        public override void Flush()
        {
            writer.Flush();
        }
    }
}
