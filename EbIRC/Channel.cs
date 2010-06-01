using System;
using System.Text;
using System.Windows.Forms;
using EbiSoft.Library;
using System.IO;
using EbiSoft.EbIRC.Settings;

namespace EbiSoft.EbIRC
{
    /// <summary>
    /// チャンネルをあらわすクラス
    /// </summary>
    class Channel
    {
        RingBuffer<string> m_log;
        private bool m_defaultChannel;
        private bool m_isJoin;
        private string m_name;
        private string m_topic;
        private string[] m_members;
        private int m_unreadCount;
        private string m_password;

        private static char[] escapeForFilenameTargets = "\\/:*?<>\"|".ToCharArray();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">チャンネル名</param>
        /// <param name="defaultChannel">デフォルトチャンネル指定ON/OFF</param>
        public Channel(string name, bool defaultChannel, string password)
        {
            m_log = new RingBuffer<string>(SettingManager.Data.MaxLogs);
            m_topic = string.Empty;
            m_name = name;
            m_defaultChannel = defaultChannel;
            m_members = new string[] { };
            m_unreadCount = 0;
            m_password = password;
        }

        /// <summary>
        /// ログを収得する
        /// </summary>
        /// <returns>ログ</returns>
        public string GetLogs()
        {
            // 全部改行でつなげて返す
            return string.Join("\r\n", m_log.ToArray());
        }

        /// <summary>
        /// ログを追加する
        /// </summary>
        /// <param name="logLine">追加するログ</param>
        public void AddLogs(string logLine)
        {
            // ログを追加する
            m_log.Add(logLine);
            if (SettingManager.Data.LogingEnable)
            {
                try
                {
                    string baseDir;
                    if (!String.IsNullOrEmpty(SettingManager.Data.LogDirectory)
                        && Directory.Exists(SettingManager.Data.LogDirectory))
                    {
                        baseDir = Path.Combine(SettingManager.Data.LogDirectory, "Log");
                    }
                    else
                    {
                        baseDir = Path.Combine(Program.ProgramDirectory, "Log");
                    }
                    string directory = Path.Combine(baseDir, EscapeForFilename(Name));
                    string filename = Path.Combine(directory, DateTime.Now.ToString("yyyyMMdd") + ".htm");
                    if (!Directory.Exists(baseDir)) Directory.CreateDirectory(baseDir);
                    if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                    using (StreamWriter writer = new StreamWriter(filename, true, Encoding.Default))
                    {
                        writer.WriteLine(logLine.Replace("<", "&lt;").Replace(">", "&gt;") + "<br>");
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// ログをクリアする
        /// </summary>
        public void ClearLog()
        {
            m_log.Clear();
        }

        private string EscapeForFilename(string str)
        {
            string returnValue = str;
            foreach (char escapeChar in escapeForFilenameTargets)
            {
                returnValue = returnValue.Replace(escapeChar, '_');
            }
            return returnValue;
        }

        /// <summary>
        /// Join してるかどうか
        /// </summary>
        public bool IsJoin
        {
            get { return m_isJoin; }
            set {
                m_isJoin = value;
                if (!value)
                {
                    // メンバー一覧クリア
                    m_members = new string[] { };
                    // トピッククリア
                    Topic = string.Empty;
                }
            }
        }

        /// <summary>
        /// デフォルトチャンネルに設定されてるかどうか
        /// </summary>
        public bool IsDefaultChannel
        {
            get { return m_defaultChannel; }
            set { m_defaultChannel = value; }
        }

        /// <summary>
        /// チャンネル名
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
            }
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
        /// メンバー一覧
        /// </summary>
        public string[] Members
        {
            get { return m_members; }
            set { m_members = value; }
        }

        /// <summary>
        /// チャンネルかどうか
        /// </summary>
        public bool IsChannel
        {
            get { return IRC.IRCClient.IsChannelString(this.Name); }
        }

        /// <summary>
        /// 未読メッセージ数
        /// </summary>
        public int UnreadCount
        {
            get { return m_unreadCount; }
            set { m_unreadCount = value; }
        }

        /// <summary>
        /// パスワード
        /// </summary>
        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }
    }
}
