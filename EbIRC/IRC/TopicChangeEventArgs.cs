using System;
using System.Collections.Generic;
using System.Text;

namespace EbiSoft.EbIRC.IRC
{
    /// <summary>
    /// トピック変更イベントのイベントデータ
    /// </summary>
    public class TopicChangeEventArgs : EventArgs
    {
        private string m_channel;
        private string m_topic;
        private string m_sender;

        /// <summary>
        /// チャンネル
        /// </summary>
        public string Channel
        {
            get { return m_channel; }
            set { m_channel = value; }
        }

        /// <summary>
        /// トピック
        /// </summary>
        public string Topic
        {
            get { return m_topic; }
            set { m_topic = value; }
        }

        /// <summary>
        /// 変更した人
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
        /// <param name="topic">トピック</param>
        public TopicChangeEventArgs(string channel, string topic)
            : this(string.Empty, channel, topic)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sender">送信者</param>
        /// <param name="channel">チャンネル</param>
        /// <param name="topic">トピック</param>
        public TopicChangeEventArgs(string sender, string channel, string topic)
        {
            m_channel = channel;
            m_topic = topic;
            m_sender = sender;
        }
	
    }

    /// <summary>
    /// トピック変更イベントのデリゲート
    /// </summary>
    public delegate void TopicChangeEventDelegate(object sender, TopicChangeEventArgs e);
}
