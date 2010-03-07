using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace EbiSoft.EbIRC.Settings
{
    /// <summary>
    /// 設定
    /// </summary>
    public class SettingManager
    {
        private static readonly string settingFile = Path.Combine(Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName),
        Properties.Resources.ResourceManager.GetString("SettingFileName"));

        private const string REG_KEY = @"Software\EbiSoft\EbIRC\";

        private static Setting m_data;

        /// <summary>
        /// インスタンス
        /// </summary>
        public static Setting Data
        {
            get {
                if (m_data == null)
                    m_data = new Setting();
                return m_data;
            }
        }

        private static bool m_deserializing = false;

        /// <summary>
        /// デシリアライズ中フラグ
        /// </summary>
        public static bool Deserializing
        {
            get { return m_deserializing; }
        }
	

        /// <summary>
        /// 値を読み込む
        /// </summary>
        public static void ReadSetting()
        {
            // スクリーン初期値の取得(縦画面時でもWidthが長辺になるように代入)
            int screenWidth;
            int screenHeight;
            if (Screen.PrimaryScreen.Bounds.Height < Screen.PrimaryScreen.Bounds.Width)
            {
                // 横画面
                screenWidth = Screen.PrimaryScreen.Bounds.Width;
                screenHeight = Screen.PrimaryScreen.Bounds.Height;
            }
            else
            {
                // 縦画面
                screenWidth = Screen.PrimaryScreen.Bounds.Height;
                screenHeight = Screen.PrimaryScreen.Bounds.Width;
            }

            // 設定ファイルの存在を確認
            if (File.Exists(settingFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Setting));
                using (FileStream fs = new FileStream(settingFile, FileMode.Open, FileAccess.Read))
                {
                    m_deserializing = true;
                    m_data = (Setting)serializer.Deserialize(fs);
                    m_deserializing = false;

                    // 旧設定変換処理
                    foreach (ConnectionProfile prof in m_data.Profiles.Profile)
                    {
                        // エンコード設定が未設定の場合はデフォルト設定を書き込む
                        if (string.IsNullOrEmpty(prof.Encoding))
                        {
                            prof.Encoding = Properties.Resources.DefaultEncoding;
                        }

                        // チャンネル設定が存在する場合は上書きする
                        if ((m_data.DefaultChannels.Length > 0) && (prof.DefaultChannels.Length == 0))
                        {
                            prof.DefaultChannels = m_data.DefaultChannels;
                        }

                        // チャンネル設定が存在する場合は上書きする
                        if ((prof.DefaultChannels.Length > 0) && (prof.Channels.Count == 0)) {
                            foreach (string ch in prof.DefaultChannels)
                            {
                                prof.Channels.Add(new ChannelSetting(ch));
                            }
                        }
                    }
                }
            }
            else
            {
                m_data = new Setting();
            }

            // サーバー一覧読み込み
            // まず、デフォルトの一覧を読み込んで、その後ファイルから読み込めたらそれで上書きする。
            m_data.DefaultServers = Properties.Resources.ResourceManager.GetString("DefaultServers").Replace("\r", "").Split("\n".ToCharArray());

            // 変数宣言 (ファイル入力、一覧リスト、サーバー定義ファイル)
            StreamReader sr = null;
            List<string> tempServers = new List<string>();
            string serverFile = Path.Combine(Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName),
                    Properties.Resources.ResourceManager.GetString("DefaultServerFileName")
            );
            try
            {
                if (File.Exists(serverFile))
                {
                    sr = new StreamReader(serverFile);
                    while (!sr.EndOfStream)
                    {
                        tempServers.Add(sr.ReadLine());
                    }
                    m_data.DefaultServers = tempServers.ToArray();
                }
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }

        /// <summary>
        /// 値を書き込む
        /// </summary>
        public static void WriteSetting()
        {
            XmlAttributeOverrides overrides;
            XmlAttributes attr;

            // 旧設定変換：デフォルトチャンネル設定を無視
            overrides = new XmlAttributeOverrides();
            attr = new XmlAttributes();
            attr.XmlIgnore = true;
            overrides.Add(typeof(Setting), "DefaultChannels", attr);

            // 旧設定変換：デフォルトチャンネル設定を無視(プロファイル側)
            overrides = new XmlAttributeOverrides();
            attr = new XmlAttributes();
            attr.XmlIgnore = true;
            overrides.Add(typeof(ConnectionProfile), "DefaultChannels", attr);

            XmlSerializer serializer = new XmlSerializer(typeof(Setting), overrides);
            using (FileStream fs = new FileStream(settingFile, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(fs, m_data);
            }
        }
    }
}
