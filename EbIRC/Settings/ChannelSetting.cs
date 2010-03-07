using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EbiSoft.EbIRC.Settings
{
    /// <summary>
    /// 接続先チャンネルの情報を保持するクラス
    /// </summary>
    [XmlType("Channel")]
    public class ChannelSetting
    {
        private string m_name = string.Empty;
        private string m_password = string.Empty;
        private bool m_ignoreInUnreadCountSort = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ChannelSetting()
        {

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">チャンネル名</param>
        public ChannelSetting(string name)
        {
            m_name = name;
        }

        /// <summary>
        /// チャンネルの名前
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// チャンネルパスワード
        /// </summary>
        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }

        /// <summary>
        /// 未読数カウントソート時に対象外にする
        /// </summary>
        public bool IgnoreInUnreadCountSort
        {
            get { return m_ignoreInUnreadCountSort; }
            set { m_ignoreInUnreadCountSort = value; }
        }
	
    }
}
