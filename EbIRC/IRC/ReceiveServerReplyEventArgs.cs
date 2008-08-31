using System;

namespace EbiSoft.EbIRC.IRC
{

	/// <summary>
	/// エラーイベント情報
	/// </summary>
	public class ReceiveServerReplyEventArgs : EventArgs {
		ReplyNumbers m_number;
        string[] m_param;
		string m_message;

        /// <summary>
        /// リプライ番号
        /// </summary>
		public ReplyNumbers Number
		{
			get{ return m_number;  }
		}

        /// <summary>
        /// メッセージ
        /// </summary>
		public string Message
		{
			get{ return m_message;  }
		}

        /// <summary>
        /// パラメータ
        /// </summary>
        public string[] Parameter
        {
            get { return m_param; }
            set { m_param = value; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="number">リプライ番号</param>
        /// <param name="param">サーバーから渡されたパラメータ</param>
		public ReceiveServerReplyEventArgs(ReplyNumbers number, string[] param) {
			m_number  = number;
			m_param   = param;
			m_message = param[param.Length - 1];
		}
	}

    /// <summary>
    /// エラーイベントのデリゲート
    /// </summary>
	public delegate void ReceiveServerReplyEventHandler(object sender, ReceiveServerReplyEventArgs e);

}
