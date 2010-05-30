using System;

namespace EbiSoft.EbIRC.IRC {
	/// <summary>
	/// ユーザー情報
	/// </summary>
	public class UserInfo 
	{
		string m_nickName;
		string m_realName;
        string m_loginName;
        string m_nickservPass;

        /// <summary>
        /// ニックネーム
        /// </summary>
		public string NickName
		{
			get{ return m_nickName;  }
		}

        /// <summary>
        /// ニックネームを設定する
        /// </summary>
        /// <param name="value">設定するニックネーム</param>
		internal void setNick(string value)
		{
			m_nickName = value;
		}

        /// <summary>
        /// ログインネーム
        /// </summary>
		public string RealName
		{
			get{ return m_realName;  }
		}

        /// <summary>
        /// ログインネーム
        /// </summary>
        public string LoginName
        {
            get { return m_loginName; }
        }

        /// <summary>
        /// Nickservパスワード
        /// </summary>
        public string NickservPass
        {
            get { return m_nickservPass; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="nickName">ニックネーム</param>
        /// <param name="realName">ログインネーム</param>
		public UserInfo(string nickName, string realName)
		{
			m_nickName = nickName;
			m_realName = realName;
		}

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="nickName">ニックネーム</param>
        /// <param name="realName">ログインネーム</param>
        public UserInfo(string nickName, string realName, string loginName, string nickServPass)
        {
            m_nickName = nickName;
            m_realName = realName;
            m_loginName = loginName;
            m_nickservPass = nickServPass;
        }
    }
}
