using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;
using System.Text.RegularExpressions;

namespace EbiSoft.EbIRC
{
    [XmlType(Namespace="EbIRC", TypeName="Settings")]
    public class SettingData
    {
        private const string DEFAULT_FONT = "Tahoma";

        private ConnectionProfileData m_profiles = new ConnectionProfileData();
        private string[] m_defchannels = new string[] { };
        private int m_maxLog = 60;
        private string m_fontName = DEFAULT_FONT;
        private int m_fontSize = 9;
        private bool m_topicVisible = true;
        private bool m_selectChannelAtConnect = false;
        private int m_horizontalKey = 1;
        private int m_verticalKey = 2;
        private int m_horizontalKey2 = 0;
        private int m_verticalKey2 = 3;
        private string[] m_subnick = new string[] { };
        private int m_inputLogBuffer = 10;
        private bool m_confimDisconnect = false;
        private bool m_confimExit = false;
        private string[] m_defaultServers = new string[] { };
        private bool m_useNetworkControl = true;
        private bool m_cacheConnection = true;
/*
        private int m_channelControlSplitVertical = 320 / 2;
        private int m_channelControlSplitHorizonal = 240 / 2;
*/
        private bool m_reverseSoftKey = false;
        private int m_scrollLines = 5;
        private bool m_forcePong = false;
        private string[] m_dislikeKeywords = new string[] { };
        private string[] m_highlightKeywords = new string[] { };
        private bool m_regexHighlight = false;
        private bool m_regexDislike = false;
        private EbIRCHilightMethod m_highlightMethod = EbIRCHilightMethod.None;
        private bool m_highlightChannelChange = false;
        private int m_highlightContinueTime = 1500;
        private int m_channelShortcutIgnoreTimes = 400;


        /// <summary>
        /// 接続プロファイル
        /// </summary>
        public ConnectionProfileData Profiles
        {
            get { return m_profiles;  }
            set { m_profiles = value; }
        }

        /// <summary>
        /// 接続時にJOINするチャンネル
        /// </summary>
        [Obsolete("Profiles.ActiveProfile.DefaultChannelsを使用してください。")]
        public string[] DefaultChannels
        {
            get { return m_defchannels; }
            set { m_defchannels = value; }
        }

        /// <summary>
        /// 1チャンネルあたりの最大ログ行数
        /// </summary>
        public int MaxLogs
        {
            get { return m_maxLog; }
            set { m_maxLog = value; }
        }

        /// <summary>
        /// フォント名
        /// </summary>
        public string FontName
        {
            get { return m_fontName; }
            set { m_fontName = value; }
        }

        /// <summary>
        /// フォントサイズ
        /// </summary>
        public int FontSize
        {
            get { return m_fontSize; }
            set { m_fontSize = value; }
        }

        /// <summary>
        /// トピックパネルの表示状態
        /// </summary>
        public bool TopicVisible
        {
            get { return m_topicVisible; }
            set { m_topicVisible = value; }
        }

        /// <summary>
        /// 接続時に一番上のチャンネルを選択
        /// </summary>
        public bool SelectChannelAtConnect
        {
            get { return m_selectChannelAtConnect; }
            set { m_selectChannelAtConnect = value; }
        }

        /// <summary>
        /// 切断確認メッセージを表示するかどうか
        /// </summary>
        public bool ConfimDisconnect
        {
            get { return m_confimDisconnect; }
            set { m_confimDisconnect = value; }
        }

        /// <summary>
        /// 終了確認メッセージを表示するかどうか
        /// </summary>
        public bool ConfimExit
        {
            get { return m_confimExit; }
            set { m_confimExit = value; }
        }

        /// <summary>
        /// 初期設定サーバー一覧
        /// </summary>
        [XmlIgnore]
        public string[] DefaultServers
        {
            get { return m_defaultServers; }
            set { m_defaultServers = value; }
        }

        /// <summary>
        /// 左右キーオペレーション
        /// </summary>
        /// <remarks>
        /// 0:通常動作
        /// 1:クイックチャンネルセレクト
        /// 2:ページ送り
        /// 3:発言ログブラウズ
        /// 4:フォントサイズ変更
        /// 5:動作なし
        /// </remarks>
        public int HorizontalKeyOperation
        {
            get { return m_horizontalKey; }
            set { m_horizontalKey = value; }
        }

        /// <summary>
        /// 上下キーオペレーション
        /// </summary>
        /// <remarks>
        /// 0:通常動作
        /// 1:クイックチャンネルセレクト
        /// 2:ページ送り
        /// 3:発言ログブラウズ
        /// 4:フォントサイズ変更
        /// 5:動作なし
        /// </remarks>
        public int VerticalKeyOperation
        {
            get { return m_verticalKey; }
            set { m_verticalKey = value; }
        }

        /// <summary>
        /// Ctrl+左右キーオペレーション
        /// </summary>
        /// <remarks>
        /// 0:通常動作
        /// 1:クイックチャンネルセレクト
        /// 2:ページ送り
        /// 3:発言ログブラウズ
        /// </remarks>
        public int HorizontalKeyWithCtrlOperation
        {
            get { return m_horizontalKey2; }
            set { m_horizontalKey2 = value; }
        }

        /// <summary>
        /// Ctrl+上下キーオペレーション
        /// </summary>
        /// <remarks>
        /// 0:通常動作
        /// 1:クイックチャンネルセレクト
        /// 2:ページ送り
        /// 3:発言ログブラウズ
        /// </remarks>
        public int VerticalKeyWithCtrlOperation
        {
            get { return m_verticalKey2; }
            set { m_verticalKey2 = value; }
        }

        /// <summary>
        /// サブニックネームリスト
        /// </summary>
        public string[] SubNicknames
        {
            get { return m_subnick; }
            set { m_subnick = value; }
        }

        /// <summary>
        /// 入力ログバッファサイズ
        /// </summary>
        public int InputLogBufferSize
        {
            get { return m_inputLogBuffer; }
            set { m_inputLogBuffer = value; }
        }

        /// <summary>
        /// ネットワーク接続制御を使用するかどうか
        /// </summary>
        public bool UseNetworkControl
        {
            get { return m_useNetworkControl; }
            set { m_useNetworkControl = value; }
        }

        /// <summary>
        /// ネットワーク接続をキャッシュするかどうか
        /// </summary>
        public bool CacheConnection
        {
            get { return m_cacheConnection; }
            set { m_cacheConnection = value; }
        }
/*
        /// <summary>
        /// チャンネル操作ダイアログ縦画面時の分割幅
        /// </summary>
        public int ChannelControlSplitHorizonal
        {
            get { return m_channelControlSplitHorizonal; }
            set { m_channelControlSplitHorizonal = value; }
        }

        /// <summary>
        /// チャンネル操作ダイアログ横画面時の分割幅
        /// </summary>
        public int ChannelControlSplitVertical
        {
            get { return m_channelControlSplitVertical; }
            set { m_channelControlSplitVertical = value; }
        }
*/
        /// <summary>
        /// ソフトキーの入れ替え
        /// </summary>
        public bool ReverseSoftKey
        {
            get { return m_reverseSoftKey; }
            set { m_reverseSoftKey = value; }
        }

        /// <summary>
        /// スクロールする行数
        /// </summary>
        public int ScrollLines
        {
            get { return m_scrollLines; }
            set { m_scrollLines = value; }
        }

        /// <summary>
        /// 強制PONG送信
        /// </summary>
        public bool ForcePong
        {
            get { return m_forcePong; }
            set { m_forcePong = value; }
        }

        /// <summary>
        /// ハイライトのマッチングキーワード
        /// </summary>
        public string[] HighlightKeywords
        {
            get { return m_highlightKeywords; }
            set { m_highlightKeywords = value; }
        }

        /// <summary>
        /// ハイライトのマッチングで正規表現を使用する
        /// </summary>
        public bool UseRegexHighlight
        {
            get { return m_regexHighlight; }
            set { m_regexHighlight = value; }
        }

        /// <summary>
        /// ハイライト種別
        /// </summary>
        public EbIRCHilightMethod HighlightMethod
        {
            get { return m_highlightMethod; }
            set { m_highlightMethod = value; }
        }

        /// <summary>
        /// ハイライト時チャンネル切り替え
        /// </summary>
        public bool HighlightChannelChange
        {
            get { return m_highlightChannelChange; }
            set { m_highlightChannelChange = value; }
        }

        /// <summary>
        /// ハイライト継続時間(ms)
        /// </summary>
        public int HighlightContinueTime
        {
            get { return m_highlightContinueTime; }
            set { m_highlightContinueTime = value; }
        }
	

        /// <summary>
        /// 無視キーワード
        /// </summary>
        public string[] DislikeKeywords
        {
            get { return m_dislikeKeywords; }
            set { m_dislikeKeywords = value; }
        }

        /// <summary>
        /// 無視ワードのマッチングで正規表現を使用する
        /// </summary>
        public bool UseRegexDislike
        {
            get { return m_regexDislike; }
            set { m_regexDislike = value; }
        }

        /// <summary>
        /// 最後の発言後、空打ちショートカットが無効な時間(ms)
        /// </summary>
        public int ChannelShortcutIgnoreTimes
        {
            get { return m_channelShortcutIgnoreTimes; }
            set { m_channelShortcutIgnoreTimes = value; }
        }
	

        #region 設定からデータを作成するメソッドとプロパティ

        /// <summary>
        /// フォントを取得する
        /// </summary>
        /// <returns>設定から作られたフォント</returns>
        public Font GetFont()
        {
            try
            {
                return new Font(FontName, (float)FontSize, FontStyle.Regular);
            }
            catch (Exception)
            {
                return new Font(FontFamily.GenericMonospace, 9, FontStyle.Regular);
            }
        }

        /// <summary>
        /// ハイライトキーワードにマッチする正規表現クラスを作成します
        /// </summary>
        /// <returns>指定されたキーワードにマッチする正規表現。形式が正しくなければ null</returns>
        public Regex GetHighlightKeywordMatcher()
        {
            return GetKeywordMatcher(HighlightKeywords, UseRegexHighlight);
        }

        /// <summary>
        /// 無視キーワードにマッチする正規表現クラスを作成します
        /// </summary>
        /// <returns>指定されたキーワードにマッチする正規表現。形式が正しくなければ null</returns>
        public Regex GetDislikeKeywordMatcher()
        {
            return GetKeywordMatcher(DislikeKeywords, UseRegexDislike);
        }

        /// <summary>
        /// キーワードマッチ用の正規表現クラスを作成します
        /// </summary>
        /// <param name="keywords">キーワード</param>
        /// <param name="useRegex">正規表現を使用するかどうか</param>
        /// <returns>指定されたキーワードにマッチする正規表現。形式が正しくなければ null</returns>
        private Regex GetKeywordMatcher(string[] keywords, bool useRegex)
        {
            if (keywords == null) return null;
            if (keywords.Length == 0) return null;
            if ((keywords.Length == 1) && (string.IsNullOrEmpty(keywords[0]))) return null;

            if (useRegex)
            {
                return new Regex(keywords[0]);
            }
            else
            {
                List<string> escapedKeywords = new List<string>();
                foreach (string keyword in keywords)
                {
                    if (!string.IsNullOrEmpty(keyword)) 
                        escapedKeywords.Add(keyword);
                }
                return new Regex(string.Join("|", escapedKeywords.ToArray()));
            }
        }

        /// <summary>
        /// 上キーのオペレーション
        /// </summary>
        [XmlIgnore]
        public EbIRCKeyOperations UpKeyOperation
        {
            get
            {
                switch (VerticalKeyOperation)
                {
                    case 1:
                        return EbIRCKeyOperations.QuickChannelPrev;

                    case 2:
                        return EbIRCKeyOperations.PageUp;

                    case 3:
                        return EbIRCKeyOperations.InputLogPrev;

                    case 4:
                        return EbIRCKeyOperations.FontSizeUp;

                    case 5:
                        return EbIRCKeyOperations.NoOperation;

                    default:
                        return EbIRCKeyOperations.Default;
                }
            }
        }

        /// <summary>
        /// 下キーのオペレーション
        /// </summary>
        [XmlIgnore]
        public EbIRCKeyOperations DownKeyOperation
        {
            get
            {
                switch (VerticalKeyOperation)
                {
                    case 1:
                        return EbIRCKeyOperations.QuickChannelNext;

                    case 2:
                        return EbIRCKeyOperations.PageDown;

                    case 3:
                        return EbIRCKeyOperations.InputLogNext;

                    case 4:
                        return EbIRCKeyOperations.FontSizeDown;

                    case 5:
                        return EbIRCKeyOperations.NoOperation;

                    default:
                        return EbIRCKeyOperations.Default;
                }
            }
        }

        /// <summary>
        /// 左キーのオペレーション
        /// </summary>
        [XmlIgnore]
        public EbIRCKeyOperations LeftKeyOperation
        {
            get
            {
                switch (HorizontalKeyOperation)
                {
                    case 1:
                        return EbIRCKeyOperations.QuickChannelPrev;

                    case 2:
                        return EbIRCKeyOperations.PageUp;

                    case 3:
                        return EbIRCKeyOperations.InputLogPrev;

                    case 4:
                        return EbIRCKeyOperations.FontSizeUp;

                    case 5:
                        return EbIRCKeyOperations.NoOperation;

                    default:
                        return EbIRCKeyOperations.Default;
                }
            }
        }

        /// <summary>
        /// 右キーのオペレーション
        /// </summary>
        [XmlIgnore]
        public EbIRCKeyOperations RightKeyOperation
        {
            get
            {
                switch (HorizontalKeyOperation)
                {
                    case 1:
                        return EbIRCKeyOperations.QuickChannelNext;

                    case 2:
                        return EbIRCKeyOperations.PageDown;

                    case 3:
                        return EbIRCKeyOperations.InputLogNext;

                    case 4:
                        return EbIRCKeyOperations.FontSizeDown;

                    case 5:
                        return EbIRCKeyOperations.NoOperation;

                    default:
                        return EbIRCKeyOperations.Default;
                }
            }
        }

        /// <summary>
        /// Ctrl+上キーのオペレーション
        /// </summary>
        [XmlIgnore]
        public EbIRCKeyOperations CtrlUpKeyOperation
        {
            get
            {
                switch (VerticalKeyWithCtrlOperation)
                {
                    case 1:
                        return EbIRCKeyOperations.QuickChannelPrev;

                    case 2:
                        return EbIRCKeyOperations.PageUp;

                    case 3:
                        return EbIRCKeyOperations.InputLogPrev;

                    case 4:
                        return EbIRCKeyOperations.FontSizeUp;

                    case 5:
                        return EbIRCKeyOperations.NoOperation;

                    default:
                        return EbIRCKeyOperations.Default;
                }
            }
        }

        /// <summary>
        /// Ctrl+下キーのオペレーション
        /// </summary>
        [XmlIgnore]
        public EbIRCKeyOperations CtrlDownKeyOperation
        {
            get
            {
                switch (VerticalKeyWithCtrlOperation)
                {
                    case 1:
                        return EbIRCKeyOperations.QuickChannelNext;

                    case 2:
                        return EbIRCKeyOperations.PageDown;

                    case 3:
                        return EbIRCKeyOperations.InputLogNext;

                    case 4:
                        return EbIRCKeyOperations.FontSizeDown;

                    case 5:
                        return EbIRCKeyOperations.NoOperation;

                    default:
                        return EbIRCKeyOperations.Default;
                }
            }
        }

        /// <summary>
        /// Ctrl+左キーのオペレーション
        /// </summary>
        [XmlIgnore]
        public EbIRCKeyOperations CtrlLeftKeyOperation
        {
            get
            {
                switch (HorizontalKeyWithCtrlOperation)
                {
                    case 1:
                        return EbIRCKeyOperations.QuickChannelPrev;

                    case 2:
                        return EbIRCKeyOperations.PageUp;

                    case 3:
                        return EbIRCKeyOperations.InputLogPrev;

                    case 4:
                        return EbIRCKeyOperations.FontSizeUp;

                    case 5:
                        return EbIRCKeyOperations.NoOperation;

                    default:
                        return EbIRCKeyOperations.Default;
                }
            }
        }

        /// <summary>
        /// Ctrl+右キーのオペレーション
        /// </summary>
        [XmlIgnore]
        public EbIRCKeyOperations CtrlRightKeyOperation
        {
            get
            {
                switch (HorizontalKeyWithCtrlOperation)
                {
                    case 1:
                        return EbIRCKeyOperations.QuickChannelNext;

                    case 2:
                        return EbIRCKeyOperations.PageDown;

                    case 3:
                        return EbIRCKeyOperations.InputLogNext;

                    case 4:
                        return EbIRCKeyOperations.FontSizeDown;

                    case 5:
                        return EbIRCKeyOperations.NoOperation;

                    default:
                        return EbIRCKeyOperations.Default;
                }
            }
        }

        #endregion

    }
}
