using System;

namespace EbiSoft.EbIRC.IRC {
	/// <summary>
	/// ユーザー情報
	/// </summary>
	public class UserInfo 
	{
		string m_nickname;
		string m_realname;

        /// <summary>
        /// ニックネーム
        /// </summary>
		public string NickName
		{
			get{ return m_nickname;  }
		}

        /// <summary>
        /// ニックネームを設定する
        /// </summary>
        /// <param name="value">設定するニックネーム</param>
		internal void setNick(string value)
		{
			m_nickname = value;
		}

        /// <summary>
        /// ログインネーム
        /// </summary>
		public string RealName
		{
			get{ return m_realname;  }
		}

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="nickName">ニックネーム</param>
        /// <param name="realName">ログインネーム</param>
		public UserInfo(string nickName, string realName)
		{
			m_nickname = nickName;
			m_realname = realName;
		}
	}
}
