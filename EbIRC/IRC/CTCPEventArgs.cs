using System;

namespace EbiSoft.EbIRC.IRC 
{
	/// <summary>
	/// CTCPクエリイベントのデータ
	/// </summary>
	public class CtcpEventArgs : EventArgs
	{

		private string m_sender;
		private string m_command;
		private string m_param;
		private string m_reply;

        /// <summary>
        /// 送信者
        /// </summary>
		public string Sender
		{
			get{ return m_sender;  }
		}

        /// <summary>
        /// CTCPコマンド
        /// </summary>
		public string Command
		{
			get{ return m_command;  }
			set{ m_command = value; }
		}

        /// <summary>
        /// CTCPコマンド パラメータ
        /// </summary>
		public string Parameter
		{
			get{ return m_param;  }
		}

        /// <summary>
        /// 返信データ
        /// </summary>
		public string Reply
		{
			get{ return m_reply;  }
			set{ m_reply = value; }
		}

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sender">送信者</param>
        /// <param name="command">CTCPコマンド</param>
        /// <param name="param">CTCPコマンド パラメータ</param>
		public CtcpEventArgs(string sender, string command, string param)
		{
			m_sender  = sender;
			m_command = command;
			m_param   = param;
			m_reply   = string.Empty;
		}
	}

    /// <summary>
    /// CTCPクエリイベントのデリゲート
    /// </summary>
	public delegate void CtcpEventHandler(object sender, CtcpEventArgs e);
}
