using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace EbiSoft.EbIRC
{
    /// <summary>
    /// 設定
    /// </summary>
    class Settings
    {
        private static readonly string settingFile = Path.Combine(Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName),
        Properties.Resources.ResourceManager.GetString("SettingFileName"));

        private const string REG_KEY = @"Software\EbiSoft\EbIRC\";

        private static SettingData m_data;

        /// <summary>
        /// インスタンス
        /// </summary>
        public static SettingData Data
        {
            get {
                if (m_data == null)
                    m_data = new SettingData();
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
                m_data = new SettingData();
                XmlSerializer serializer = new XmlSerializer(typeof(SettingData));
                using (FileStream fs = new FileStream(settingFile, FileMode.Open, FileAccess.Read))
                {
                    m_deserializing = true;
                    m_data = (SettingData) serializer.Deserialize(fs);
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
                    }

                }
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
            // 旧設定変換：デフォルトチャンネル設定を無視
            XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            XmlAttributes attr = new XmlAttributes();
            attr.XmlIgnore = true;
            overrides.Add(typeof(SettingData), "DefaultChannels", attr);

            XmlSerializer serializer = new XmlSerializer(typeof(SettingData), overrides);
            using (FileStream fs = new FileStream(settingFile, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(fs, m_data);
            }
        }
    }

    /// <summary>
    /// キーオペレーションの種類
    /// </summary>
    public enum EbIRCKeyOperations
    {
        /// <summary>
        /// デフォルト動作
        /// </summary>
        Default,
        /// <summary>
        /// クイックチャンネルセレクト、次へ
        /// </summary>
        QuickChannelNext,
        /// <summary>
        /// クイックチャンネルセレクト、前へ
        /// </summary>
        QuickChannelPrev,
        /// <summary>
        /// 次ページへ
        /// </summary>
        PageUp,
        /// <summary>
        /// 前ページへ
        /// </summary>
        PageDown,
        /// <summary>
        /// 入力ログ巻き戻し
        /// </summary>
        InputLogPrev,
        /// <summary>
        /// 入力ログ先送り
        /// </summary>
        InputLogNext,
        /// <summary>
        /// フォントサイズ拡大
        /// </summary>
        FontSizeUp,
        /// <summary>
        /// フォントサイズ縮小
        /// </summary>
        FontSizeDown,
        /// <summary>
        /// 動作なし
        /// </summary>
        NoOperation
    }

    /// <summary>
    /// キーワード反応の方法
    /// </summary>
    public enum EbIRCHilightMethod
    {
        /// <summary>
        /// 反応なし
        /// </summary>
        None,
        /// <summary>
        /// バイブレーション
        /// </summary>
        Vibration,
        /// <summary>
        /// LED点灯
        /// </summary>
        Led,
        /// <summary>
        /// バイブ＋LED
        /// </summary>
        VibrationAndLed
    }
}
