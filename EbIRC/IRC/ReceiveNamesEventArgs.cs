using System;

namespace EbiSoft.EbIRC.IRC
{
	/// <summary>
	/// 名前一覧受信イベント
	/// </summary>
	public class ReceiveNamesEventArgs : EventArgs {
		string[] m_names;
		string   m_channel;

        /// <summary>
        /// 名前の一覧
        /// </summary>
		public string[] Names
		{
			get{ return m_names;  }
		}

        /// <summary>
        /// チャンネル
        /// </summary>
		public string Channel
		{
			get{ return m_channel;  }
		}

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="channel">チャンネル</param>
        /// <param name="names">名前の一覧</param>
		public ReceiveNamesEventArgs(string channel, string[] names) {
			m_channel = channel;
			m_names   = names;
		}
	}

    /// <summary>
    /// 名前の一覧受信イベントのデリゲート
    /// </summary>
	public delegate void ReceiveNamesEventHandler(object sender, ReceiveNamesEventArgs e);
}
