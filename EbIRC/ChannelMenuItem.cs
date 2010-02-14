using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace EbiSoft.EbIRC
{
    /// <summary>
    /// チャンネルを持つMenuItem
    /// </summary>
    class ChannelMenuItem : MenuItem, IComparable
    {
        private Channel m_channel;

        /// <summary>
        /// チャンネルを指定してメニューを初期化します。
        /// </summary>
        /// <param name="channel">このメニューに対応するチャンネル</param>
        public ChannelMenuItem(Channel channel) : base()
        {
            Channel = channel;
        }

        /// <summary>
        /// このインスタンスに関連づけられている Channel を取得または設定します。
        /// </summary>
        public Channel Channel
        {
            get { return m_channel; }
            set
            {
                m_channel = value;
                UpdateText();
            }
        }

        public void UpdateText()
        {
            if (Channel.UnreadCount > 0)
            {
                base.Text = string.Format("{0} ({1})", Channel.Name.Replace("&", "&&"), Channel.UnreadCount);
            }
            else
            {
                base.Text = Channel.Name.Replace("&", "&&");
            }
        }

        #region IComparable メンバ

        public int CompareTo(object obj)
        {
            if (obj is ChannelMenuItem)
            {
                ChannelMenuItem chItem = obj as ChannelMenuItem;
                Channel ch1 = m_channel;
                Channel ch2 = chItem.Channel;

                if (this.Checked && !chItem.Checked)
                {
                    return 1;
                }
                else if (!this.Checked && chItem.Checked)
                {
                    return -1;
                }
                else
                {
                    return ch1.UnreadCount.CompareTo(ch2.UnreadCount);
                }
            }
            else
            {
                return 0;
            }
        }

        #endregion
    }
}
