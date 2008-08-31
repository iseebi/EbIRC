using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace EbiSoft.EbIRC
{
    /// <summary>
    /// チャンネルを持つMenuItem
    /// </summary>
    class ChannelMenuItem : MenuItem
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
                base.Text = value.Name.Replace("&", "&&");
                m_channel = value;
            }
        }
    }
}
