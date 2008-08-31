using System;
using System.Net;

namespace EbiSoft.EbIRC.IRC {
	/// <summary>
	/// サーバー情報
	/// </summary>
	public struct ServerInfo {
		string m_name;
		int    m_port;
		string m_password;

        /// <summary>
        /// サーバー名
        /// </summary>
		public string Name
		{
			get{ return m_name;  }
		}

        /// <summary>
        /// ポート
        /// </summary>
		public int Port
		{
			get{ return m_port;  }
		}

        /// <summary>
        /// サーバーパスワード
        /// </summary>
		public string Password
		{
			get{ return m_password;  }
		}

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">サーバー名</param>
        /// <param name="port">ポート</param>
        /// <param name="password">パスワード</param>
		public ServerInfo(string name, int port, string password) 
        {
			m_name     = name;
			m_port     = port;
			m_password = password;
		}

        /// <summary>
        /// 設定されてる情報から IPEndPoint を作る
        /// </summary>
        /// <returns>設定された情報を示すIPEndPoint</returns>
		public IPEndPoint GetEndPoint()
		{
			IPHostEntry entry = Dns.GetHostEntry(m_name);
			IPAddress   addr = entry.AddressList[0];

            if (IPAddress.IsLoopback(addr))
            {
                return new IPEndPoint(IPAddress.Loopback, m_port);
            }
            else
            {
                return new IPEndPoint(addr, m_port);
            }
		}
	}
}
