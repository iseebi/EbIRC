using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EbiSoft.EbIRC.Settings
{
    /// <summary>
    /// 接続プロファイル
    /// </summary>
    public class ConnectionProfile
    {
        /// <summary>
        /// プロファイル名
        /// </summary>
        [XmlAttribute]
        public string ProfileName
        {
            get { return m_profileName; }
            set { m_profileName = value; }
        }
        private string m_profileName = string.Empty;

        /// <summary>
        /// サーバー
        /// </summary>
        public string Server
        {
            get { return m_server; }
            set { m_server = value; }
        }
        private string m_server = string.Empty;

        /// <summary>
        /// ポート
        /// </summary>
        public int Port
        {
            get { return m_port; }
            set { m_port = value; }
        }
        private int m_port = 6667;

        /// <summary>
        /// SSLの使用
        /// </summary>
        public bool UseSsl
        {
            get { return m_useSsl; }
            set { m_useSsl = value; }
        }
        private bool m_useSsl;

        /// <summary>
        /// パスワード
        /// </summary>
        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }
        private string m_password = string.Empty;

        /// <summary>
        /// ニックネーム
        /// </summary>
        public string Nickname
        {
            get { return m_nickname; }
            set { m_nickname = value; }
        }
        private string m_nickname = string.Empty;

        /// <summary>
        /// 名前
        /// </summary>
        public string Realname
        {
            get { return m_realname; }
            set { m_realname = value; }
        }
        private string m_realname = string.Empty;

        /// <summary>
        /// エンコーディング
        /// </summary>
        public string Encoding
        {
            get { return m_encoding; }
            set { m_encoding = value; }
        }
        private string m_encoding = Properties.Resources.DefaultEncoding;

        /// <summary>
        /// 接続時にJOINするチャンネル
        /// </summary>
        [Obsolete("Profiles.ActiveProfile.DefaultChannelsを使用してください。")]
        public string[] DefaultChannels
        {
            get { return m_defchannels; }
            set { m_defchannels = value; }
        }
        private string[] m_defchannels = new string[] { };

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="profileName">プロファイル名</param>
        public ConnectionProfile(string profileName)
        {
            m_profileName = profileName;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConnectionProfile()
        {

        }

        #endregion

        /// <summary>
        /// エンコーディングを取得します。
        /// </summary>
        /// <returns></returns>
        public Encoding GetEncoding()
        {
            try
            {
                if (Encoding.ToLower().Replace("-", string.Empty) == "utf8")
                {
                    return new UTF8Encoding(false);
                }
                else
                {
                    return System.Text.Encoding.GetEncoding(Encoding);
                }
            }
            catch (Exception)
            {
                return System.Text.Encoding.GetEncoding(Properties.Resources.DefaultEncoding);
            }
        }

        public override string ToString()
        {
            return ProfileName;
        }
    }
}
