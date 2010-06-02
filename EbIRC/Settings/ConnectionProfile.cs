using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EbiSoft.EbIRC.Settings
{
    /// <summary>
    /// 接続プロファイル
    /// </summary>
    public class ConnectionProfile : ICloneable
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
        /// SSLの使用
        /// </summary>
        public bool NoValidation
        {
            get { return m_noValidation; }
            set { m_noValidation = value; }
        }
        private bool m_noValidation;

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
        [Obsolete("Channelsを使用してください。")]
        public string[] DefaultChannels
        {
            get { return m_defchannels; }
            set { m_defchannels = value; }
        }
        private string[] m_defchannels = new string[] { };

        /// <summary>
        /// 接続時にJOINするチャンネル
        /// </summary>
        public ChannelSettingCollection Channels
        {
            get { return m_channels; }
            set { m_channels = value; }
        }
        private ChannelSettingCollection m_channels = new ChannelSettingCollection();

        /// <summary>
        /// ログインネーム
        /// </summary>
        public string LoginName
        {
            get { return m_loginName; }
            set { m_loginName = value; }
        }
        private string m_loginName;

        /// <summary>
        /// Nickserv パスワード
        /// </summary>
        public string NickServPassword
        {
            get { return m_nickServPassword; }
            set { m_nickServPassword = value; }
        }
        private string m_nickServPassword;


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

        #region ICloneable メンバ

        /// <summary>
        /// このオブジェクトのクローンを作成します
        /// </summary>
        /// <returns>作成されたクローン</returns>
        public object Clone()
        {
            ConnectionProfile prof = new ConnectionProfile();
            prof.m_channels = m_channels;
            prof.m_defchannels = m_defchannels;
            prof.m_encoding = m_encoding;
            prof.m_nickname = m_nickname;
            prof.m_noValidation = m_noValidation;
            prof.m_password = m_password;
            prof.m_port = m_port;
            prof.m_profileName = m_profileName;
            prof.m_realname = m_realname;
            prof.m_server = m_server;
            prof.m_useSsl = m_useSsl;
            return prof;
        }

        #endregion
    }
}
