using System;
using System.Collections.Generic;
using System.Text;

namespace EbiSoft.EbIRC.IRC
{
    /// <summary>
    /// モード変更イベント
    /// </summary>
    public class ModeChangeEventArgs : EventArgs
    {
        private string m_channel;
        private string[] m_target;
        private string m_mode;
        private string m_sender;

        /// <summary>
        /// 対象チャンネル
        /// </summary>
        public string Channel
        {
            get { return m_channel; }
            set { m_channel = value; }
        }

        /// <summary>
        /// 変更対象
        /// </summary>
        public string[] Target
        {
            get { return m_target; }
            set { m_target = value; }
        }

        /// <summary>
        /// 変更モード
        /// </summary>
        public string Mode
        {
            get { return m_mode; }
            set { m_mode = value; }
        }

        /// <summary>
        /// 送信者
        /// </summary>
        public string Sender
        {
            get { return m_sender; }
            set { m_sender = value; }
        }
	

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="channel">チャンネル</param>
        /// <param name="mode">モード</param>
        public ModeChangeEventArgs(string sender, string channel, string mode)
            : this(sender, channel, mode, new string[] { })
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="channel">チャンネル</param>
        /// <param name="mode">モード</param>
        /// <param name="target">対象になる人</param>
        public ModeChangeEventArgs(string sender, string channel, string mode, string[] target)
        {
            m_channel = channel;
            m_mode = mode;
            m_target = target;
            m_sender = sender;
        }
    }

    /// <summary>
    /// モード変更イベントハンドラ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ModeChangeEventHandler(object sender, ModeChangeEventArgs e);
}
