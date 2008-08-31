using System;

namespace EbiSoft.EbIRC.IRC {
	/// <summary>
	/// ユーザーの入退室イベントのデータ
	/// </summary>
	public class UserInOutEventArgs : EventArgs
    {
		string        m_user;
		string        m_channel;
		InOutCommands m_command;

        /// <summary>
        /// 入退室したユーザー
        /// </summary>
		public string User
		{
			get{ return m_user;  }
		}

        /// <summary>
        /// 入退室したチャンネル
        /// </summary>
		public string Channel
		{
			get{ return m_channel;  }
		}

        /// <summary>
        /// 入退室イベントの種類
        /// </summary>
		public InOutCommands Command
		{
			get{ return m_command;  }
		}
		
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="user">入退室したユーザー</param>
        /// <param name="command">入退室イベントの種類</param>
        /// <param name="channel">入退室したチャンネル</param>
		public UserInOutEventArgs(string user, InOutCommands command, string channel)
		{
			m_user    = user;
			m_command = command;
			m_channel = channel;
		}
	}

    /// <summary>
    /// 入退室イベントの種類
    /// </summary>
	public enum InOutCommands
	{
		Join,
		Leave,
		Quit,
	}

    /// <summary>
    /// 入退室イベントのデリゲート
    /// </summary>
	public delegate void UserInOutEventHandler(object sender, UserInOutEventArgs e);
}
