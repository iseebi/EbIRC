using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using EbiSoft.EbIRC.Settings;

namespace EbiSoft.EbIRC
{
    /// <summary>
    /// チャンネルを持つMenuItem
    /// </summary>
    class ChannelMenuItem : MenuItem, IComparable
    {
        private Channel m_channel;
        private int m_index;

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

        /// <summary>
        /// ソート用のインデックスを取得または設定します。
        /// </summary>
        public int Index
        {
            get { return m_index; }
            set { m_index = value; }
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

                // ハイライト優先
                if (SettingManager.Data.QuickSwitchHilightsSort)
                {
                    if ((this.Checked && ch1.IsSortTarget) && !(chItem.Checked && ch2.IsSortTarget))
                    {
                        return 1;
                    }
                    else if (!(this.Checked && ch1.IsSortTarget) && (chItem.Checked && ch2.IsSortTarget))
                    {
                        return -1;
                    }
                }
                // 未読数優先
                if (SettingManager.Data.QuickSwitchUnreadCountSort)
                {
                    int count1 = ch1.IsSortTarget ? ch1.UnreadCount : 0;
                    int count2 = ch2.IsSortTarget ? ch2.UnreadCount : 0;
                    int dx = count1.CompareTo(count2);
                    if (dx != 0)
                        return dx;
                }
                return chItem.Index.CompareTo(this.Index);
            }
            else
            {
                return 0;
            }
        }

        #endregion
    }
}
