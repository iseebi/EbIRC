using System;

namespace EbiSoft.EbIRC.IRC 
{
	/// <summary>
	/// メッセージ受信イベント
	/// </summary>
	public class ReceiveMessageEventArgs : EventArgs 
	{
		string m_sender;
		string m_receiver;
		string m_message;
		
        /// <summary>
        /// 送信先
        /// </summary>
		public string Receiver
		{
			get{ return m_receiver;  }
		}

        /// <summary>
        /// 送信者
        /// </summary>
		public string Sender{
			get{ return m_sender; }
		}

        /// <summary>
        /// メッセージ
        /// </summary>
		public string Message
		{
			get{ return m_message;  }
		}
		
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sender">送信先</param>
        /// <param name="receiver">送信者</param>
        /// <param name="message">メッセージ</param>
		public ReceiveMessageEventArgs(string sender, string receiver, string message) 
		{
			m_sender   = sender;
			m_receiver = receiver;
			m_message  = message;
			
		}
	}

    /// <summary>
    /// メッセージ受信イベントのデリゲート
    /// </summary>
	public delegate void ReceiveMessageEventHandler(object sender, ReceiveMessageEventArgs e);
}
