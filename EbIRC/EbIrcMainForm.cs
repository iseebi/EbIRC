using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EbiSoft.EbIRC.IRC;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Reflection;
using Microsoft.WindowsCE.Forms;
using EbiSoft.EbIRC.Properties;
using EbiSoft.EbIRC.Settings;

namespace EbiSoft.EbIRC
{
    /// <summary>
    /// EbIRC メインフォーム
    /// </summary>
    public partial class EbIrcMainForm : Form
    {
        private readonly string CONNMGR_URL_FORMAT;
        private readonly string LOG_KEY_SERVER;

        private readonly Regex UrlRegex = new Regex(@"([A-Za-z]+)://([^:/]+)(:(\d+))?(/[^#\s]*)(#(\S+))?", RegexOptions.Compiled);

        private const int XCRAWL_KEYCODE = 0x83;  // Xcrawl のスクロールイベント

        #region P/Invoke 定義

#if Win32PInvoke
        [DllImport("user32", CharSet = CharSet.Auto)]
        private extern static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);
        [DllImport("user32", CharSet = CharSet.Auto, EntryPoint="SendMessage")]
        private extern static IntPtr SendMessage2(IntPtr hWnd, int msg, int wParam, int lParam);
#else
        [DllImport("coredll", CharSet = CharSet.Auto)]
        private extern static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);
        [DllImport("coredll", CharSet = CharSet.Auto, EntryPoint="SendMessage")]
        private extern static IntPtr SendMessage2(IntPtr hWnd, int msg, int wParam, int lParam);
        /*
        [DllImport("coredll", CharSet = CharSet.Auto)]
        private static extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, UInt32 dwData, IntPtr dwExtraInfo);
        */
#endif
        private const int WM_SETREDRAW = 0x000B;
        private const int WM_USER = 0x400;
        private const int EM_GETEVENTMASK = (WM_USER + 59);
        private const int EM_SETEVENTMASK = (WM_USER + 69);
        private const int WM_KEYDOWN = 0x0100;
        private const int VK_PRIOR = 0x21; // Page UP
        private const int VK_NEXT = 0x22;  // Page Down
        private const int EM_LINESCROLL = 0xB6;
        private const int EM_SCROLL = 0xB5;
        private const int SB_LINEUP = 0;        // １行上にスクロール
        private const int SB_LINEDOWN = 1;      // 同、下にスクロール
        private const int SB_PAGEUP = 2;        // １ページ上にスクロール
        private const int SB_PAGEDOWN = 3;      //  同、下にスクロール
        //private const int WM_LBUTTONDOWN = 0x201;
        //private const int WM_LBUTTONUP = 0x202;

        #endregion

        IRCClient ircClient;
        Dictionary<string, Channel> m_channel;         // チャンネルリスト
        Channel m_currentCh;                           // 現在のチャンネル
        Channel m_serverCh;                            // サーバーメッセージ
        string m_nickname = string.Empty;              // 自分の現在のニックネーム
        List<string> m_inputlog;                       // 入力テキストログ
        int m_inputlogPtr;                             // テキストログ現在位置
        bool m_scrollFlag = false;                     // スクロールイベントフラグ

        InputBoxInputFilter m_inputBoxFilter;          // 入力フィルタ
        LogBoxInputFilter   m_logBoxFilter;              // ログボックスフィルタ
        bool m_rightFilterling = false;                // 右キー押下フィルタON

        bool m_storeFlag = false;                           // ログ蓄積モードフラグ
        StringBuilder m_storedLog = new StringBuilder();    // 蓄積ログ

        Regex highlightMatcher = null;   // ハイライトマッチオブジェクト
        Regex dislikeMatcher = null;     // 無視マッチオブジェクト
        bool highlightFlag = false;      // ハイライトするフラグ
        Channel highlightChannel = null; // ハイライトするチャンネル

        List<ChannelMenuItem> m_channelPopupMenus; // チャンネルポップアップメニューに出すチャンネルの項目
        int m_lastSendTick = 0;                    // 最後に発言したTicktime (無効時間判定に使用)

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EbIrcMainForm()
        {
            InitializeComponent();

            // 定数ロード
            CONNMGR_URL_FORMAT = "http://{0}:{1}/";
            LOG_KEY_SERVER     = Resources.ChannelSelecterServerCaption;

            // IRCクライアント初期化、イベントハンドラ結合
            ircClient = new IRCClient();
            ircClient.OwnerControl = this;
            ircClient.ChangedMyNickname   += new NickNameChangeEventHandler(ircClient_ChangeMyNickname);
            ircClient.Connected           += new EventHandler(ircClient_Connected);
            ircClient.ConnectionFailed    += new EventHandler(ircClient_ConnectionFailed);
            ircClient.Disconnected        += new EventHandler(ircClient_Disconnected);
            ircClient.Kick                += new KickEventHandler(ircClient_Kick);
            ircClient.ProcessedConnection += new EventHandler(ircClient_ProcessedConnection);
            ircClient.ReceiveCtcpQuery    += new CtcpEventHandler(ircClient_ReceiveCtcpQuery);
            ircClient.ReceiveCtcpReply    += new CtcpEventHandler(ircClient_ReceiveCtcpReply);
            ircClient.ReceiveMessage      += new ReceiveMessageEventHandler(ircClient_ReceiveMessage);
            ircClient.ReceiveNames        += new ReceiveNamesEventHandler(ircClient_ReceiveNames);
            ircClient.ReceiveNotice       += new ReceiveMessageEventHandler(ircClient_ReceiveNotice);
            ircClient.ReceiveServerReply  += new ReceiveServerReplyEventHandler(ircClient_ReceiveServerReply);
            ircClient.UserInOut           += new UserInOutEventHandler(ircClient_UserInOut);
            ircClient.TopicChange         += new TopicChangeEventDelegate(ircClient_TopicChange);
            ircClient.ChangedNickname     += new NickNameChangeEventHandler(ircClient_ChangedNickname);
            ircClient.ModeChange          += new ModeChangeEventHandler(ircClient_ModeChange);
            ircClient.StartMessageEvents  += new EventHandler(ircClient_StartMessageEvents);
            ircClient.FinishMessageEvents += new EventHandler(ircClient_FinishMessageEvents);

            // 設定を読み込む
            SettingManager.ReadSetting();

            // チャンネルリスト初期化
            m_channel = new Dictionary<string, Channel>(new ChannelNameEqualityComparer());
            m_channelPopupMenus = new List<ChannelMenuItem>();

            // サーバーチャンネルを用意する
            m_serverCh = new Channel(LOG_KEY_SERVER, false);

            // バージョン情報出力
            Assembly asm = Assembly.GetExecutingAssembly();
            AssemblyName asmName = asm.GetName();
            AddLog(m_serverCh, string.Format(Resources.VersionInfomation, asmName.Version.Major, asmName.Version.Minor, asmName.Version.Build));

            // テキストログ初期化
            m_inputlog = new List<string>(SettingManager.Data.InputLogBufferSize);
            m_inputlogPtr = 0;

            // メッセージフィルタ設定
            m_inputBoxFilter = new InputBoxInputFilter(inputTextBox);
            m_inputBoxFilter.EndComposition += new EventHandler(m_inputBoxFilter_EndComposition);
            m_inputBoxFilter.MouseWheelMoveDown += new EventHandler(m_inputBoxFilter_MouseWheelMoveDown);
            m_inputBoxFilter.MouseWheelMoveUp += new EventHandler(m_inputBoxFilter_MouseWheelMoveUp);
            m_logBoxFilter = new LogBoxInputFilter(logTextBox);
            m_logBoxFilter.TapUp += new EventHandler(m_logBoxFilter_TapUp);
            m_logBoxFilter.Resize += new EventHandler(m_logBoxFilter_Resize);

            // UI設定をアップデートする
            SetConnectionMenuText(); // 接続メニュー
            SetDefaultChannel();     // デフォルト接続チャンネルの読み込み
            LoadChannel(m_serverCh); // サーバーメッセージに切り替え
            UpdateUISettings();      // その他のUI設定のアップデート

            // ソフトキーの入れ替え
            if (SettingManager.Data.ReverseSoftKey)
            {
                mainMenu1.MenuItems.Remove(connectionMenuItem);
                mainMenu1.MenuItems.Add(connectionMenuItem);

            }
        }

        #region プロパティ

        /// <summary>
        /// チャンネル一覧
        /// </summary>
        internal Dictionary<string, Channel> Channels
        {
            get { return m_channel; }
        }

        /// <summary>
        /// IRCクライアント
        /// </summary>
        internal IRCClient IRCClient
        {
            get { return ircClient; }
        }

        /// <summary>
        /// 現在選択されているチャンネル
        /// </summary>
        internal Channel CurrentChannel
        {
            get { return m_currentCh; }
            set
            {
                LoadChannel(value);
            }
        }

        /// <summary>
        /// サーバーチャンネルのインスタンス
        /// </summary>
        internal Channel ServerChannel
        {
            get { return m_serverCh; }
        }

        #endregion

        #region フォームイベント

        /// <summary>
        /// フォームが閉じられるとき
        /// </summary>
        private void EbIrcMainForm_Closing(object sender, CancelEventArgs e)
        {
            ircClient.ChangedMyNickname   -= new NickNameChangeEventHandler(ircClient_ChangeMyNickname);
            ircClient.Connected           -= new EventHandler(ircClient_Connected);
            ircClient.ConnectionFailed    -= new EventHandler(ircClient_ConnectionFailed);
            ircClient.Disconnected        -= new EventHandler(ircClient_Disconnected);
            ircClient.Kick                -= new KickEventHandler(ircClient_Kick);
            ircClient.ProcessedConnection -= new EventHandler(ircClient_ProcessedConnection);
            ircClient.ReceiveCtcpQuery    -= new CtcpEventHandler(ircClient_ReceiveCtcpQuery);
            ircClient.ReceiveCtcpReply    -= new CtcpEventHandler(ircClient_ReceiveCtcpReply);
            ircClient.ReceiveMessage      -= new ReceiveMessageEventHandler(ircClient_ReceiveMessage);
            ircClient.ReceiveNames        -= new ReceiveNamesEventHandler(ircClient_ReceiveNames);
            ircClient.ReceiveNotice       -= new ReceiveMessageEventHandler(ircClient_ReceiveNotice);
            ircClient.ReceiveServerReply  -= new ReceiveServerReplyEventHandler(ircClient_ReceiveServerReply);
            ircClient.UserInOut           -= new UserInOutEventHandler(ircClient_UserInOut);
            ircClient.TopicChange         -= new TopicChangeEventDelegate(ircClient_TopicChange);
            ircClient.ChangedNickname     -= new NickNameChangeEventHandler(ircClient_ChangedNickname);
            ircClient.ModeChange          -= new ModeChangeEventHandler(ircClient_ModeChange);
            ircClient.StartMessageEvents  -= new EventHandler(ircClient_StartMessageEvents);
            ircClient.FinishMessageEvents -= new EventHandler(ircClient_FinishMessageEvents);

            ircClient.OwnerControl = null;
        }

        /// <summary>
        /// フォームが閉じられたとき
        /// </summary>
        private void EbIrcMainForm_Closed(object sender, EventArgs e)
        {
            ircClient.Close();
        }

        /// <summary>
        /// フォームがアクティブ化されたとき
        /// </summary>
        private void EbIrcMainForm_Activated(object sender, EventArgs e)
        {
            inputTextBox.Focus();
            logTextBox.Enabled = true;
        }

        /// <summary>
        /// フォームが非アクティブ化されたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EbIrcMainForm_Deactivate(object sender, EventArgs e)
        {
            logTextBox.Enabled = false;
            inputTextBox.Focus();
        }

        /// <summary>
        /// フォームがリサイズされたときの動作
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            ProcessResize();
        }

        /// <summary>
        /// SIPパネルの開閉状態が変わったとき
        /// </summary>
        private void inputPanel_EnabledChanged(object sender, EventArgs e)
        {
            ProcessResize();
        }

        private void ProcessResize()
        {
            // SIPパネル分の大きさ変更
            if (inputPanel.Enabled)
            {
                mainPanel.Location = new Point(0, 0);
                mainPanel.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - inputPanel.Bounds.Height);
            }
            else
            {
                mainPanel.Location = new Point(0, 0);
                mainPanel.Size = this.ClientSize;
            }
        }


        #endregion

        #region メニューイベント

        /// <summary>
        /// 接続・切断トグル
        /// </summary>
        private void connectionMenuItem_Click(object sender, EventArgs e)
        {
            // ガベージコレクト
            GC.Collect();

            // ステータスが切断のとき→接続する
            if (ircClient.Status == IRCClientStatus.Disconnected)
            {
                // サーバーが空欄のときには接続処理を行わない。
                if (SettingManager.Data.Profiles.ActiveProfile.Server == string.Empty)
                {
                    MessageBox.Show(Resources.NullServerSettingError,
                        Resources.ConnectionError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return;
                }

                try
                {
                    // ダイアルアップ処理
                    if (SettingManager.Data.UseNetworkControl)
                    {
                        ConnectionManager.Connection(string.Format(CONNMGR_URL_FORMAT,
                            SettingManager.Data.Profiles.ActiveProfile.Server, SettingManager.Data.Profiles.ActiveProfile.Port.ToString()));
                    }

                    BroadcastLog(Resources.BeginConnection);
                    IRCClient.Encoding = SettingManager.Data.Profiles.ActiveProfile.GetEncoding();
                    ircClient.Connect(SettingManager.Data.Profiles.ActiveProfile.Server, (int)SettingManager.Data.Profiles.ActiveProfile.Port,
                        SettingManager.Data.Profiles.ActiveProfile.Password, SettingManager.Data.Profiles.ActiveProfile.UseSsl,
                        SettingManager.Data.Profiles.ActiveProfile.Nickname, SettingManager.Data.Profiles.ActiveProfile.Realname);
                    SetConnectionMenuText();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    ircClient.Close();
                }
                return;
            }

            // 切断以外のとき→切断する
            else
            {
                // 確認メッセージ表示設定がONなら確認メッセージを表示する
                if (SettingManager.Data.ConfimDisconnect)
                {
                    // いいえが選択された場合は抜ける
                    if (MessageBox.Show(Resources.DisconnectConfim, Resources.Confim,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return;
                    }
                }

                ircClient.Disconnect();
                return;
            }
        }

        /// <summary>
        /// 設定画面
        /// </summary>
        private void menuSettingMenuItem_Click(object sender, EventArgs e)
        {
            using (SettingForm settingForm = new SettingForm())
            {
                // 設定画面開く
                settingForm.ShowDialog();
                // デフォルトチャンネル更新
                SetDefaultChannel();
                // UI更新
                UpdateUISettings();
            }
        }

        /// <summary>
        /// チャンネルリストのサーバー部分
        /// </summary>
        private void menuChannelListServerMenuItem_Click(object sender, EventArgs e)
        {
            LoadChannel(m_serverCh);
        }

        /// <summary>
        /// チャンネルリストのチャンネル
        /// </summary>
        private void menuChannelListChannelsMenuItem_Click(object sender, EventArgs e)
        {
            // 選択されたメニューを取得
            ChannelMenuItem menu = (ChannelMenuItem)sender;

            foreach (Channel channel in m_channel.Values)
            {
                // このチャンネルのメニューだったら、そのチャンネルをロード
                if (channel == menu.Channel)
                    LoadChannel(channel);
            }
        }

        /// <summary>
        /// 終了
        /// </summary>
        private void menuExitMenuItem_Click(object sender, EventArgs e)
        {
            if (SettingManager.Data.ConfimExit)
            {
                // いいえが選択された場合は抜ける
                if (MessageBox.Show(Resources.ExitConfim, Resources.Confim,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
            }
            Close();
        }

        /// <summary>
        /// 選択範囲コピー
        /// </summary>
        private void menuEditCopyMenuItem_Click(object sender, EventArgs e)
        {
            // 選択されていないなら抜ける
            if (logTextBox.SelectionLength == 0) return;
            Clipboard.SetDataObject(logTextBox.SelectedText);

        }

        /// <summary>
        /// 貼り付け
        /// </summary>
        private void menuEditPasteMenuItem_Click(object sender, EventArgs e)
        {
            IDataObject obj = Clipboard.GetDataObject();
            if (obj.GetDataPresent(typeof(string)))
            {
                inputTextBox.Text += (string)obj.GetData(typeof(string));
            }
        }

        /// <summary>
        /// Googleる
        /// </summary>
        private void menuEditGoogleMenuItem_Click(object sender, EventArgs e)
        {
            // 選択されていないなら抜ける
            if (logTextBox.SelectionLength == 0) return;
            string url = string.Format(Resources.GoogleURL, Uri.EscapeUriString(logTextBox.SelectedText));

            try
            {
                // URLオープン
                System.Diagnostics.Process.Start(url, string.Empty);
            }
            catch (Win32Exception)
            {
                MessageBox.Show(Resources.CannotOpenURL, Resources.FaildBoot,
                    MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        /// <summary>
        /// カーソル位置のURLを開く
        /// </summary>
        private void menuEditOpenURLMenuItem_Click(object sender, EventArgs e)
        {
            string url = GetSelectedURL();
            if (!string.IsNullOrEmpty(url))
            {
                OpenUrl(url);
            }
        }

        /// <summary>
        /// ニックネーム切り替え
        /// </summary>
        void nicknameSwitcher_Click(object sender, EventArgs e)
        {
            if (sender is MenuItem)
            {
                MenuItem item = (MenuItem)sender;

                if (ircClient.Status == IRCClientStatus.Online)
                {
                    ircClient.ChangeNickname(item.Text);
                }
            }
        }

        /// <summary>
        /// ニックネーム新規入力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuNicknameInputMenuItem_Click(object sender, EventArgs e)
        {
            if (ircClient.Status == IRCClientStatus.Online)
            {
                using (InputBoxForm form = new InputBoxForm())
                {
                    form.Text = Resources.InputNewNicknameTitle;
                    form.Description = Resources.InputNewNicknamePrompt;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        if (!string.IsNullOrEmpty(form.Value))
                        {
                            ircClient.ChangeNickname(form.Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ログクリア
        /// </summary>
        private void menuEditClearMenuItem_Click(object sender, EventArgs e)
        {
            ClearStoredLog();
            m_currentCh.ClearLog();
            logTextBox.Text = string.Empty;
            AddLog(m_currentCh, Resources.ClearedLog);
        }

        /// <summary>
        /// チャンネル操作メニュー押下
        /// </summary>
        private void menuChannelControlMenuItem_Click(object sender, EventArgs e)
        {
            using (ChannelControlDialog dialog = new ChannelControlDialog())
            {
                dialog.Owner = this;
                dialog.ShowDialog();
                if (dialog.SelectedChannel != null)
                {
                    LoadChannel(dialog.SelectedChannel);
                }
            }
        }

        /// <summary>
        /// ログ入力ボックスタップホールドメニューオープン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logContextMenu_Popup(object sender, EventArgs e)
        {
            logContextMenu.MenuItems.Clear();

            if (!string.IsNullOrEmpty(GetSelectedURL()))
            {
                logContextMenu.MenuItems.Add(contextUrlOpenMenuItem);
            }
            if (!string.IsNullOrEmpty(logTextBox.SelectedText))
            {
                logContextMenu.MenuItems.Add(contextGoogleMenuItem);
                logContextMenu.MenuItems.Add(contextCopyMenuItem);
            }
        }

        /// <summary>
        /// チャンネルコンテキストメニューオープン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void channelContextMenu_Popup(object sender, EventArgs e)
        {
            // 一度クリア
            channelContextMenu.MenuItems.Clear();

            // ハイライトメッセージの処理
            if (menuHilightedMessages.MenuItems.Count > 0)
            {
                channelContextMenu.MenuItems.Add(menuHilightedMessages);
                channelContextMenu.MenuItems.Add(menuHilightedSeparator);
            }

            // その他のチャンネルセレクタの処理
            List<ChannelMenuItem> menus = new List<ChannelMenuItem>(m_channelPopupMenus);

            // インデックス付与
            for (int i = 0; i < menus.Count; i++)
            {
                menus[i].Index = i;
            }

            // ソートして追加
            menus.Sort();
            menus.Reverse(); // 降順に
            foreach (ChannelMenuItem menu in menus)
            {
                menu.UpdateText();
                channelContextMenu.MenuItems.Add(menu);
            }
        }

        #endregion

        #region 入力ボックス イベント

        /// <summary>
        /// テキストボックスでキーが押されたとき
        /// </summary>
        private void inputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '\r':
                    if (inputTextBox.TextLength > 0)
                    {
                        if (ircClient.Status == IRCClientStatus.Online)
                        {
                            if (m_currentCh != m_serverCh)
                            {
                                // 入力ログを記録
                                if (m_inputlog.Count == m_inputlog.Capacity)
                                {
                                    m_inputlog.RemoveAt(0);
                                }
                                m_inputlog.Add(inputTextBox.Text);
                                m_inputlogPtr = m_inputlog.Count;

                                if (inputTextBox.Text.StartsWith("/"))
                                {
                                    ircClient.SendCommand(inputTextBox.Text.Substring(1));
                                    inputTextBox.Text = string.Empty;
                                }
                                else
                                {
                                    AddLog(m_currentCh, string.Format("{0}> {1}", ircClient.User.NickName, inputTextBox.Text));
                                    if (m_storeFlag)
                                    {
                                        CommitStoredLog();
                                        BeginStoreLog();
                                    }
                                    ircClient.SendPrivateMessage(m_currentCh.Name, inputTextBox.Text);
                                    inputTextBox.Text = string.Empty;
                                }

                                e.Handled = true;
                                m_lastSendTick = Environment.TickCount;
                            }
                        }
                    }
                    else
                    {
                        // 無効時間内でなければポップアップする
                        if ((Environment.TickCount - m_lastSendTick) > SettingManager.Data.ChannelShortcutIgnoreTimes)
                        {
                            channelContextMenu.Show(logTextBox, new Point(0, 0));
                            e.Handled = true;
                        }
                    }
                    break;
                /*
                case '\t':
                    logTextBox.Focus();
                    break;
                */
                default:
                    break;
            }
        }

        /// <summary>
        /// IMEの変換が終了したとき
        /// </summary>
        void m_inputBoxFilter_EndComposition(object sender, EventArgs e)
        {
            m_rightFilterling = m_inputBoxFilter.IsAtokConjectureActive()
                && (inputTextBox.TextLength == inputTextBox.SelectionStart);
        }

        /// <summary>
        /// ジョグホイールが上方向に動いたとき
        /// </summary>
        void m_inputBoxFilter_MouseWheelMoveUp(object sender, EventArgs e)
        {
            SendMessage2(logTextBox.Handle, EM_LINESCROLL, 0, -SettingManager.Data.ScrollLines);
        }

        /// <summary>
        /// ジョグホイールが下方向に動いたとき
        /// </summary>
        void m_inputBoxFilter_MouseWheelMoveDown(object sender, EventArgs e)
        {
            SendMessage2(logTextBox.Handle, EM_LINESCROLL, 0, SettingManager.Data.ScrollLines);
        }

        /// <summary>
        /// 入力エリアでキーが押されたとき
        /// </summary>
        private void inputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Xcrawl のスクロールキーコードの場合
            if (e.KeyValue == XCRAWL_KEYCODE)
            {
                m_scrollFlag = true;
                e.Handled = true;
            }

            // 入力中は処理を行わない
            if (m_inputBoxFilter.Conpositioning)
            {
                return;
            }

            // Control キーが押されている場合
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Right:
                        e.Handled = ProcessKeyOperation(SettingManager.Data.CtrlRightKeyOperation);
                        break;
                    case Keys.Left:
                        e.Handled = ProcessKeyOperation(SettingManager.Data.CtrlLeftKeyOperation);
                        break;
                    case Keys.Up:
                        e.Handled = ProcessKeyOperation(SettingManager.Data.CtrlUpKeyOperation);
                        break;
                    case Keys.Down:
                        e.Handled = ProcessKeyOperation(SettingManager.Data.CtrlDownKeyOperation);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Right:
                        if ((inputTextBox.Text == "")
                            || (inputTextBox.SelectionStart == inputTextBox.TextLength))
                        {
                            // 行末で確定し、推測変換が開いていた場合は1回無効にする
                            if (m_rightFilterling)
                            {
                                e.Handled = true;
                            }
                            else
                            {
                                e.Handled = ProcessKeyOperation(SettingManager.Data.RightKeyOperation);
                            }
                        }
                        break;
                    case Keys.Left:
                        if ((inputTextBox.Text == "")
                            || (inputTextBox.SelectionStart == 0))
                        {
                            e.Handled = ProcessKeyOperation(SettingManager.Data.LeftKeyOperation);
                        }
                        break;
                    case Keys.Up:
                    case Keys.Down:
                        // Xcrawl の判定のため、イベント処理はKeyUpにまかせる
                        e.Handled = true;
                        break;
                    default:
                        break;
                }
            }

            // 右キーフィルタリング終了
            m_rightFilterling = false;
        }

        /// <summary>
        /// 入力エリアでキーが離されたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // Xcrawl のスクロールキーコードの場合
            if (e.KeyValue == XCRAWL_KEYCODE)
            {
                m_scrollFlag = false;
                e.Handled = true;
                return;
            }

            // 入力中は処理を行わない
            if (m_inputBoxFilter.Conpositioning)
            {
                return;
            }

            // Control キーが押されていない場合
            if (!e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        if (m_scrollFlag)
                        {
                            System.Diagnostics.Debug.WriteLine("Xcrawl Up");
                            SendMessage2(logTextBox.Handle, EM_LINESCROLL, 0, -SettingManager.Data.ScrollLines);
                            e.Handled = true;
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Up");
                            if (!ProcessKeyOperation(SettingManager.Data.UpKeyOperation))
                            {
                                // デフォルトキー操作のエミュレーション
                                inputTextBox.SelectionStart = 0;
                            }
                            e.Handled = true;
                        }
                        break;
                    case Keys.Down:
                        if (m_scrollFlag)
                        {
                            System.Diagnostics.Debug.WriteLine("Xcrawl Down");
                            SendMessage2(logTextBox.Handle, EM_LINESCROLL, 0, SettingManager.Data.ScrollLines);
                            e.Handled = true;
                        }
                        else
                        {
                            if (!ProcessKeyOperation(SettingManager.Data.DownKeyOperation))
                            {
                                // デフォルトキー操作のエミュレーション
                                inputTextBox.SelectionStart = inputTextBox.Text.Length;
                            }
                            e.Handled = true;
                        }
                        break;
                    case Keys.Return:
                        e.Handled = true;
                        break;
                    default:
                        break;
                }
            }
        }
        
        #endregion

        #region ログボックス イベント

        /// <summary>
        /// タップが離されたときのイベント
        /// </summary>
        void m_logBoxFilter_TapUp(object sender, EventArgs e)
        {
            Match m = UrlRegex.Match(logTextBox.SelectedText);
            if ((m.Success) && (m.Index == 0) && (m.Length == logTextBox.SelectedText.Trim().Length))
            {
                OpenUrl(m.Value);
            }
        }

        /// <summary>
        /// リサイズされたときのイベント
        /// </summary>
        void m_logBoxFilter_Resize(object sender, EventArgs e)
        {
            logTextBox.SelectionStart = logTextBox.TextLength;
            logTextBox.ScrollToCaret();
        }

        #endregion

        #region IRCイベント

        /// <summary>
        /// 接続したとき
        /// </summary>
        private void ircClient_Connected(object sender, EventArgs e)
        {
            // すべてのチャンネルにログを追加・デフォルトチャンネルなら接続
            BroadcastLog(Resources.Connected);
            CommitStoredLog();
            SetConnectionMenuText();
        }

        /// <summary>
        /// 接続失敗したとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ircClient_ConnectionFailed(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.CannotConnectMessage, Resources.ConnectionError, 
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            SetDisconnected();
        }

        /// <summary>
        /// 切断したとき
        /// </summary>
        private void ircClient_Disconnected(object sender, EventArgs e)
        {
            // すべてのチャンネルにログを追加・Join を False に
            AddLog(m_serverCh, Resources.Disconnected);
            foreach (Channel channel in m_channel.Values)
            {
                // ログ追加
                AddLog(channel, Resources.Disconnected);

                // Join状態を解除
                channel.IsJoin = false;
            }
            CommitStoredLog();
            SetDisconnected();
        }

        /// <summary>
        /// ログメッセージの処理がはじまるとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ircClient_StartMessageEvents(object sender, EventArgs e)
        {
            BeginStoreLog();
        }

        /// <summary>
        /// ログメッセージの処理が終了するとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ircClient_FinishMessageEvents(object sender, EventArgs e)
        {
            CommitStoredLog();
        }

        /// <summary>
        /// 接続処理が完了したとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ircClient_ProcessedConnection(object sender, EventArgs e)
        {
            // デフォルトチャンネルに接続
            AddLog(m_serverCh, Resources.Connected);
            foreach (Channel channel in m_channel.Values)
            {
                // ログ追加
                AddLog(channel, Resources.Connected);

                // デフォルトチャンネルなら接続
                if (channel.IsDefaultChannel)
                {
                    ircClient.JoinChannel(channel.Name);

                    // デフォルトチャンネル選択設定ONのときは、選択する
                    if (SettingManager.Data.SelectChannelAtConnect
                        && (SettingManager.Data.Profiles.ActiveProfile.DefaultChannels.Length > 0)
                        && (channel.Name == SettingManager.Data.Profiles.ActiveProfile.DefaultChannels[0]))
                    {
                        LoadChannel(channel);
                    }
                }
            }
        }

        /// <summary>
        /// 自分のニックネームが変更されたとき
        /// </summary>
        private void ircClient_ChangeMyNickname(object sender, NickNameChangeEventArgs e)
        {
            ircClient_ChangedNickname(sender, e);
        }

        /// <summary>
        /// 誰かのニックネームが変更されたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ircClient_ChangedNickname(object sender, NickNameChangeEventArgs e)
        {
            List<string> list;

            // priv用チャンネルで、トークしてる人の名前がかわった場合
            if (m_channel.ContainsKey(IRCClient.GetUserName(e.Before)))
            {
                // チャンネル移動
                Channel talkch = m_channel[IRCClient.GetUserName(e.Before)];
                talkch.Name = IRCClient.GetUserName(e.After);
                m_channel.Add(IRCClient.GetUserName(e.After), talkch);
                m_channel.Remove(IRCClient.GetUserName(e.Before));

                // 現在のチャンネルだったら、フォームの状態を更新する
                if (m_currentCh == talkch)
                    LoadChannel(m_currentCh);

                // チャンネル変更を通知
                AddLog(talkch, string.Format(Resources.NicknameChangedMessage, IRCClient.GetUserName(e.Before), IRCClient.GetUserName(e.After)));
            }

            // 全チャンネルについて
            foreach (Channel ch in m_channel.Values)
            {
                list = new List<string>(ch.Members);
                // そいつがいれば
                if (list.Contains(IRCClient.GetUserName(e.Before)))
                {
                    // そいつの名前を変更する
                    int index = list.IndexOf(IRCClient.GetUserName(e.Before));
                    list[index] = e.After;
                    ch.Members = list.ToArray();
                    // 現在のチャンネルだったら、フォームの状態を更新する
                    if (m_currentCh == ch)
                        LoadChannel(m_currentCh);
                    // ログ追加
                    AddLog(ch, string.Format(Resources.NicknameChangedMessage, IRCClient.GetUserName(e.Before), IRCClient.GetUserName(e.After)));
                }
                else if (list.Contains("@" + IRCClient.GetUserName(e.Before)))
                {
                    // そいつの名前を変更する
                    int index = list.IndexOf("@" + IRCClient.GetUserName(e.Before));
                    list[index] = "@" + e.After;
                    ch.Members = list.ToArray();
                    // 現在のチャンネルだったら、フォームの状態を更新する
                    if (m_currentCh == ch)
                        LoadChannel(m_currentCh);
                    // ログ追加
                    AddLog(ch, string.Format(Resources.NicknameChangedMessage, IRCClient.GetUserName(e.Before), IRCClient.GetUserName(e.After)));
                }
            }
        }

        private void SetDisconnected()
        {
            SetConnectionMenuText();
            LoadChannel(m_currentCh);

            // ダイアルアップ処理
            if (SettingManager.Data.UseNetworkControl)
            {
                ConnectionManager.ReleaseAll();
            }
        }

        /// <summary>
        /// 蹴られたとき
        /// </summary>
        private void ircClient_Kick(object sender, EbiSoft.EbIRC.IRC.KickEventArgs e)
        {
            // 存在しないチャンネルのときは追加
            if (!m_channel.ContainsKey(e.Channel))
            {
                /*
                いなくなるんだからする必要ない
                AddChannel(e.Channel, false);
                m_channel[e.Channel].IsJoin = true;
                */ 
                return;
            }

            // ログ追加
            AddLog(m_channel[e.Channel], string.Format(Resources.KickedMessage, e.User, e.Target));

            // 蹴られたのが自分のとき
            if (e.Target == ircClient.User.NickName)
            {
                // 退室する
                m_channel[e.Channel].IsJoin = false;
                AddLog(m_channel[e.Channel], Resources.KickedMeMessage);
                // 現在のチャンネルだったら、フォームの状態を更新する
                if (m_currentCh == m_channel[e.Channel])
                    LoadChannel(m_currentCh);
            }
            // 蹴られたのが他人のとき
            else
            {
                List<string> list = new List<string>(m_channel[e.Channel].Members);
                // チャンネルから名前を削除する
                list.Remove(e.Target);
                list.Remove("@" + e.Target);
                m_channel[e.Channel].Members = list.ToArray();
                // 現在のチャンネルだったら、フォームの状態を更新する
                if (m_currentCh == m_channel[e.Channel])
                    LoadChannel(m_currentCh);    
            }
        }

        /// <summary>
        /// CTCPクエリ受信したとき
        /// </summary>
        private void ircClient_ReceiveCtcpQuery(object sender, CtcpEventArgs e)
        {
            switch (e.Command.ToUpper())
            {
                case "VERSION":
                    e.Reply = Resources.CtcpVersion;
                    break;
                case "SOURCE":
                    e.Reply = Resources.CtcpSource;
                    break;
                case "PING":
                    e.Reply = e.Parameter;
                    break;
                case "TIME":
                    e.Reply = System.DateTime.Now.ToString();
                    break;
                case "CLIENTINFO":
                    e.Reply = "CLIENTINFO VERSION SOURCE PING TIME";
                    break;
            }
        }

        /// <summary>
        /// CTCPリプライ受信したとき
        /// </summary>
        private void ircClient_ReceiveCtcpReply(object sender, EbiSoft.EbIRC.IRC.CtcpEventArgs e)
        {
            AddLog(m_serverCh, string.Format(Resources.CtcpReplyMessage, IRCClient.GetUserName(e.Sender), e.Command, e.Reply));
        }

        /// <summary>
        /// メッセージ受信
        /// </summary>
        private void ircClient_ReceiveMessage(object sender, EbiSoft.EbIRC.IRC.ReceiveMessageEventArgs e)
        {
            // privmsg 対応
            string channel;
            if (IRCClient.IsChannelString(e.Receiver))
            {
                channel = e.Receiver;
            }
            else
            {
                if (IRCClient.GetUserName(e.Sender) == ircClient.User.NickName)
                {
                    channel = IRCClient.GetUserName(e.Receiver);
                }
                else
                {
                    channel = IRCClient.GetUserName(e.Sender);
                }
            }

            // 存在しないチャンネルのときは追加
            if (!m_channel.ContainsKey(channel))
            {
                AddChannel(channel, false);
                m_channel[channel].IsJoin = true;
            }

            // 無視キーワードフィルタ
            if ((dislikeMatcher != null) && dislikeMatcher.Match(e.Message).Success) 
                return;

            // ログ追加
            string message = AddLog(m_channel[channel], string.Format(Resources.PrivmsgLogFormat, IRCClient.GetUserName(e.Sender), e.Message));

            // 発言数加算
            if (m_channel[channel] != m_currentCh)
            {
                m_channel[channel].UnreadCount++;
            }

            // ハイライトキーワードフィルタ
            if ((highlightMatcher != null) && highlightMatcher.Match(e.Message).Success)
                SetHighlight(m_channel[channel], message);
        }

        /// <summary>
        /// 名前一覧の受信
        /// </summary>
        private void ircClient_ReceiveNames(object sender, EbiSoft.EbIRC.IRC.ReceiveNamesEventArgs e)
        {
            // チャンネルのとき
            if (IRCClient.IsChannelString(e.Channel))
            {
                // 存在しないチャンネルのときは追加
                if (!m_channel.ContainsKey(e.Channel))
                {
                    AddChannel(e.Channel, false);
                    m_channel[e.Channel].IsJoin = true;
                }

                // ログ追加
                AddLog(m_channel[e.Channel], string.Format(Resources.UsersLogFormat, string.Join(", ", e.Names)));

                // 名前一覧更新
                m_channel[e.Channel].Members = e.Names;
                // 現在のチャンネルだったら、フォームの状態を更新する
                if (m_currentCh == m_channel[e.Channel])
                    LoadChannel(m_currentCh);
            }
        }

        /// <summary>
        /// Notice 受信
        /// </summary>
        private void ircClient_ReceiveNotice(object sender, EbiSoft.EbIRC.IRC.ReceiveMessageEventArgs e)
        {
            // privmsg 対応
            string channel;
            if (IRCClient.IsChannelString(e.Receiver))
            {
                channel = e.Receiver;
            }
            else
            {
                if (IRCClient.GetUserName(e.Sender) == ircClient.User.NickName)
                {
                    channel = IRCClient.GetUserName(e.Receiver);
                }
                else
                {
                    channel = IRCClient.GetUserName(e.Sender);
                }
            }
            
            // 送信者がセットされている場合
            if (channel != string.Empty)
            {
                // 存在しないチャンネルのときは追加
                if (!m_channel.ContainsKey(channel))
                {
                    AddChannel(channel, false);
                    m_channel[channel].IsJoin = true;
                    return;
                }

                // 無視キーワードフィルタ
                if ((dislikeMatcher != null) && dislikeMatcher.Match(e.Message).Success)
                    return;

                // ハイライトキーワードフィルタ
                //if ((highlightMatcher != null) && highlightMatcher.Match(e.Message).Success)
                //    SetHighlight(m_channel[channel]);
                
                // ログ追加
                AddLog(m_channel[channel], string.Format(Resources.NoticeLogFormat, IRCClient.GetUserName(e.Sender), e.Message));
            }
            // サーバーのとき
            else
            {
                // ログ追加
                AddLog(m_serverCh, string.Format(Resources.NoticeLogFormat, IRCClient.GetUserName(e.Sender), e.Message));
            }
        }

        /// <summary>
        /// サーバーメッセージ受信
        /// </summary>
        private void ircClient_ReceiveServerReply(object sender, EbiSoft.EbIRC.IRC.ReceiveServerReplyEventArgs e)
        {
            AddLog(m_serverCh, 
                    string.Format(Resources.NumericReplyLogFormat, (int) e.Number, string.Join(" ", e.Parameter) )
            );
        }

        /// <summary>
        /// ユーザーの出入り
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ircClient_UserInOut(object sender, EbiSoft.EbIRC.IRC.UserInOutEventArgs e)
        {
            // 自分のとき
            if (e.User == ircClient.UserString)
            {
                switch (e.Command)
                {
                    case InOutCommands.Join:
                        // 存在しないチャンネルのときは追加
                        if ((e.Channel != string.Empty)
                            && !m_channel.ContainsKey(e.Channel))
                        {
                            AddChannel(e.Channel, false);
                        }
                        // 参加フラグ変更
                        m_channel[e.Channel].IsJoin = true;
                        // ログ追加
                        AddLog(m_channel[e.Channel], string.Format(Resources.Joined, IRCClient.GetUserName(e.User)));
                        break;
                    case InOutCommands.Leave:
                        if (m_channel.ContainsKey(e.Channel))
                        {
                            // 参加フラグ変更
                            m_channel[e.Channel].IsJoin = false;
                            // ログ追加
                            AddLog(m_channel[e.Channel], string.Format(Resources.Leaved, IRCClient.GetUserName(e.User)));
                        }
                        break;
                }
            }
            // 他人のとき
            else
            {
                List<string> list;
                switch (e.Command)
                {
                    case InOutCommands.Join:
                        list = new List<string>(m_channel[e.Channel].Members);
                        // チャンネルに名前を追加する
                        list.Add(IRCClient.GetUserName(e.User));
                        m_channel[e.Channel].Members = list.ToArray();
                        // 現在のチャンネルだったら、フォームの状態を更新する
                        if (m_currentCh == m_channel[e.Channel])
                            LoadChannel(m_currentCh);
                        // ログ追加
                        AddLog(m_channel[e.Channel], string.Format(Resources.JoinedUser, IRCClient.GetUserName(e.User)));
                        break;
                    case InOutCommands.Leave:
                        list = new List<string>(m_channel[e.Channel].Members);
                        // チャンネルから名前を削除する
                        list.Remove(IRCClient.GetUserName(e.User));
                        list.Remove("@" + IRCClient.GetUserName(e.User));
                        m_channel[e.Channel].Members = list.ToArray();
                        // 現在のチャンネルだったら、フォームの状態を更新する
                        if (m_currentCh == m_channel[e.Channel])
                            LoadChannel(m_currentCh);    
                        // ログ追加
                        AddLog(m_channel[e.Channel], string.Format(Resources.LeavedUser, IRCClient.GetUserName(e.User)));
                        break;
                    case InOutCommands.Quit:
                        // 全チャンネルについて
                        foreach (Channel ch in m_channel.Values)
                        {
                            list = new List<string>(ch.Members);
                            // そいつがいれば
                            if (list.Contains(IRCClient.GetUserName(e.User))
                                || list.Contains("@" + IRCClient.GetUserName(e.User)))
                            {
                                // チャンネルから名前を削除する
                                list.Remove(IRCClient.GetUserName(e.User));
                                list.Remove("@" + IRCClient.GetUserName(e.User));
                                ch.Members = list.ToArray();
                                // 現在のチャンネルだったら、フォームの状態を更新する
                                if (m_currentCh == ch)
                                    LoadChannel(m_currentCh);
                                // ログ追加
                                AddLog(ch, string.Format(Resources.DisconnectedUser, IRCClient.GetUserName(e.User)));
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// トピック変更
        /// </summary>
        void ircClient_TopicChange(object sender, TopicChangeEventArgs e)
        {
            /*
            // 存在しないチャンネルのときは追加
            if (!m_channel.ContainsKey(e.Channel))
            {
                AddChannel(e.Channel, false);
                m_channel[e.Channel].IsJoin = true;
            }
            */
            if (!m_channel.ContainsKey(e.Channel)) return;

            // トピック変更        
            m_channel[e.Channel].Topic = e.Topic;
            // 現在のチャンネルだったら、フォームの状態を更新する
            if (m_currentCh == m_channel[e.Channel])
                LoadChannel(m_currentCh);

            // ログを出力する
            if (e.Sender != string.Empty)
            {
                AddLog(m_channel[e.Channel], string.Format(Resources.TopicChanged, IRCClient.GetUserName(e.Sender), e.Topic));
            }
            else
            {
                AddLog(m_channel[e.Channel], string.Format(Resources.TopicReceived, e.Topic));
            }
        }

        /// <summary>
        /// モード変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ircClient_ModeChange(object sender, ModeChangeEventArgs e)
        {
            // チャンネルがなければ抜ける
            if (!m_channel.ContainsKey(e.Channel))
                return;

            // ログを出力する
            if (e.Sender != string.Empty)
            {
                AddLog(m_channel[e.Channel], string.Format(Resources.ModeChanged, IRCClient.GetUserName(e.Sender), e.Mode, string.Join(",", e.Target)));
            }
            else
            {
                AddLog(m_channel[e.Channel], string.Format(Resources.ModeReceived, e.Mode, string.Join(",", e.Target)));
            }
        }

        #endregion

        #region チャンネル追加

        /// <summary>
        /// デフォルトメニューを読み込み
        /// </summary>
        void SetDefaultChannel()
        {
            // すでにチャンネルリストにあるチャンネルのデフォルトチェックを更新
            foreach (Channel channel in m_channel.Values)
            {
                // 一度デフォルトをはずす
                channel.IsDefaultChannel = false;
                // デフォルトリストを走査する
                foreach (string ch in SettingManager.Data.Profiles.ActiveProfile.DefaultChannels)
                {
                    // デフォルトチャンネルだったらtrueにして抜ける
                    if (channel.Name == ch)
                    {
                        channel.IsDefaultChannel = true;
                        break;
                    }
                }
            }

            // 新しく追加されたチャンネルを追加
            foreach (string ch in SettingManager.Data.Profiles.ActiveProfile.DefaultChannels)
            {
                // チャンネルリストに存在しないなら追加する
                if (!m_channel.ContainsKey(ch))
                {
                    AddChannel(ch, true);
                }
            }
        }

        /// <summary>
        /// チャンネルを追加する
        /// </summary>
        /// <param name="name">追加するチャンネル名</param>
        /// <param name="defaultChannel">デフォルトチャンネルかどうか</param>
        /// <returns>追加されたチャンネル</returns>
        internal Channel AddChannel(string name, bool defaultChannel)
        {
            // 空文字列なら抜ける
            if (string.IsNullOrEmpty(name.Trim()))
            {
                return null;
            }

            // 区切り線入れようとされた場合は抜ける
            if (name == "-")
            {
                return null;
            }

            // 既に存在してたら、そのチャンネルを返す
            if (m_channel.ContainsKey(name))
            {
                return m_channel[name];
            }

            Channel channel = new Channel(name, defaultChannel); // チャンネルを作成
            m_channel.Add(name, channel);                        // リストに追加

            // メニューへの追加用
            ChannelMenuItem menu;

            // チャンネル一覧メニューに追加
            menu = new ChannelMenuItem(channel);
            menu.Click += new EventHandler(menuChannelListChannelsMenuItem_Click);
            menuChannelListMenuItem.MenuItems.Add(menu); // メニューに追加
            
            // ポップアップメニューに追加
            menu = new ChannelMenuItem(channel);
            menu.Click += new EventHandler(menuChannelListChannelsMenuItem_Click);
            m_channelPopupMenus.Add(menu);
            return channel;
        }

        /// <summary>
        /// チャンネルを削除する
        /// </summary>
        /// <param name="name">削除するチャンネル</param>
        internal void RemoveChannel(string name)
        {
            if (!Channels.ContainsKey(name)) throw new ArgumentException();

            Channel ch = this.Channels[name];
            // 現在のチャンネルが削除されるならサーバーに移動する
            if (ch == m_currentCh)
            {
                LoadChannel(m_serverCh);
            }
            // チャンネル削除
            Channels.Remove(name);
            foreach (ChannelMenuItem item in m_channelPopupMenus)
            {
                if (item.Channel == ch)
                {
                    m_channelPopupMenus.Remove(item);
                    break;
                }
            }
            foreach (MenuItem item in menuChannelListMenuItem.MenuItems)
            {
                ChannelMenuItem channelItem = (item as ChannelMenuItem);
                if ((channelItem != null) && (channelItem.Channel == ch))
                {
                    menuChannelListMenuItem.MenuItems.Remove(item);
                    break;
                }
            }
        }

        #endregion

        #region チャンネル移動

        /// <summary>
        /// 現在のチャンネルを変更する
        /// </summary>
        /// <param name="ch">チャンネル</param>
        void LoadChannel(Channel channel)
        {
            // サーバーチャンネルのときは特別
            if (channel == m_serverCh)
            {
                // タイトルバー設定
                this.Text = Resources.ServerMessageTitlebar;
                // トピックバー設定
                topicLabel.Text = Resources.ServerMessageTopicbar;
            }
            else
            {
                // タイトルバー設定
                this.Text = string.Format(Resources.TitlebarFormat, channel.Name, channel.Members.Length);
                // トピックバー設定
                topicLabel.Text = string.Format(Resources.TopicbarFormat, channel.Name, channel.Topic);
            }

            // & を && に置き換え(アクセスキー化防止)
            topicLabel.Text = topicLabel.Text.Replace("&", "&&");

            // チャンネルが変更されるときのみログを読み込み
            if (m_currentCh != channel)
            {
                ClearStoredLog();
                IntPtr eventMask = IntPtr.Zero;
                try
                {
                    // Stop redrawing:
                    SendMessage(logTextBox.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
                    // Stop sending of events:
                    eventMask = SendMessage(logTextBox.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);

                    logTextBox.Text = channel.GetLogs();
                    LogMoveLastLine();

                    // 発言数クリア
                    channel.UnreadCount = 0;
                }
                finally
                {
                    // turn on events
                    SendMessage(logTextBox.Handle, EM_SETEVENTMASK, 0, eventMask);
                    // turn on redrawing
                    SendMessage(logTextBox.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
                }
            }

            // このチャンネルのハイライトメッセージを消す
            for (int i = (menuHilightedMessages.MenuItems.Count - 1); i >= 0; i--)
            {
                ChannelMenuItem menu = menuHilightedMessages.MenuItems[i] as ChannelMenuItem;
                if (menu.Channel == channel)
                {
                    menuHilightedMessages.MenuItems.RemoveAt(i);
                }
            }

            // ハイライトチェック解除
            foreach (ChannelMenuItem menu in m_channelPopupMenus)
            {
                if (menu.Channel == channel)
                {
                    menu.Checked = false;
                    break;
                }
            }

            // カレントチャンネル更新
            m_currentCh = channel;
        }

        /// <summary>
        /// 次のチャンネルへ移動
        /// </summary>
        void SwitchNextChannel()
        {
            // チャンネルリストが空なら何もしない
            if (m_channel.Count == 0) return;

            // チャンネルのリストを作る
            List<Channel> list = new List<Channel>(m_channel.Values);

            // 現在のチャンネルがサーバーなら
            if (m_currentCh == m_serverCh)
            {
                // 最初のチャンネルへ
                LoadChannel(list[0]);
            }
            // 現在のチャンネルがサーバー以外なら
            else
            {
                // 現在のチャンネルのインデックスを得る
                int index = list.IndexOf(m_currentCh);
                if (index == -1) return;

                // 末尾ならサーバーに切り替える
                if (index == list.Count - 1)
                {
                    LoadChannel(m_serverCh);
                }
                // 末尾でないなら+1
                else
                {
                    LoadChannel(list[index + 1]);
                }
            }
        }

        /// <summary>
        /// 前のチャンネルへ移動
        /// </summary>
        void SwitchPrevChannel()
        {
            // チャンネルリストが空なら何もしない
            if (m_channel.Count == 0) return;

            // チャンネルのリストを作る
            List<Channel> list = new List<Channel>(m_channel.Values);

            // 現在のチャンネルがサーバーなら
            if (m_currentCh == m_serverCh)
            {
                // 最後のチャンネルへ
                LoadChannel(list[list.Count - 1]);
            }
            // 現在のチャンネルがサーバー以外なら
            else
            {
                // 現在のチャンネルのインデックスを得る
                int index = list.IndexOf(m_currentCh);
                if (index == -1) return;

                // 先頭ならサーバーに切り替える
                if (index == 0)
                {
                    LoadChannel(m_serverCh);
                }
                // 先頭でないなら+1
                else
                {
                    LoadChannel(list[index - 1]);
                }

            }
        }

        #endregion

        #region ログ追加

        /// <summary>
        /// ログの追加
        /// </summary>
        /// <param name="targetCh">対象のチャンネル</param>
        /// <param name="message">メッセージ</param>
        /// <returns>追加したログメッセージ</returns>
        string AddLog(Channel channel, string message)
        {
            // 現在の時刻テキスト
            string time = string.Format(Resources.TimeFormat, DateTime.Now.Hour, DateTime.Now.Minute);

            // 文字列を更新
            message = string.Format(Resources.LogFormat, time, message);

            // ログを追加する
            channel.AddLogs(message);

            // 現在のチャンネルに出力されたものなら、出力する
            if (channel == m_currentCh)
            {
                if (m_storeFlag)
                {
                    // 蓄積モードの時は蓄積する
                    if (m_storedLog.Length > 0)
                        m_storedLog.Append("\r\n");
                    m_storedLog.Append(message);
                }
                else
                {
                    // 蓄積モードでなければ出力する
                    PrintLog(message);
                }
            }

            return message;
        }

        /// <summary>
        /// ログを出力する
        /// </summary>
        /// <param name="message"></param>
        private void PrintLog(string message)
        {
            IntPtr eventMask = IntPtr.Zero;
            IntPtr textBoxHandle = IntPtr.Zero;
            try
            {
                textBoxHandle = logTextBox.Handle;
                if (textBoxHandle != IntPtr.Zero)
                {
                    eventMask = SendMessage(textBoxHandle, EM_GETEVENTMASK, 0, IntPtr.Zero);
                    SendMessage(textBoxHandle, EM_SETEVENTMASK, 0, IntPtr.Zero);
                    SendMessage(textBoxHandle, WM_SETREDRAW, 0, IntPtr.Zero);
                }

                logTextBox.Text += "\r\n" + message;
                LogMoveLastLine();
            }
            catch (Exception)
            {
                return;
            }
            finally
            {
                if (textBoxHandle != IntPtr.Zero)
                {
                    SendMessage(textBoxHandle, EM_SETEVENTMASK, 0, eventMask);
                    SendMessage(textBoxHandle, WM_SETREDRAW, 1, IntPtr.Zero);
                }
            }
        }


        /// <summary>
        /// ログボックスを最終行へ移動させる
        /// </summary>
        private void LogMoveLastLine()
        {
            logTextBox.SelectionStart = logTextBox.TextLength;
            logTextBox.ScrollToCaret();
        }

        /// <summary>
        /// 全チャンネルにメッセージを出力します
        /// </summary>
        /// <param name="message"></param>
        private void BroadcastLog(string message)
        {
            AddLog(m_serverCh, message);
            foreach (Channel channel in m_channel.Values)
            {
                // ログ追加
                AddLog(channel, message);
            }
        }

        /// <summary>
        /// ログメッセージの蓄積を開始
        /// </summary>
        private void BeginStoreLog()
        {
            highlightFlag = false;
            highlightChannel = null;
            m_storedLog = new StringBuilder();
            m_storeFlag = true;
        }

        /// <summary>
        /// 蓄積したメッセージを反映する
        /// </summary>
        private void CommitStoredLog()
        {
            if (m_storeFlag && (m_storedLog.Length > 0))
            {
                PrintLog(m_storedLog.ToString());
                ClearStoredLog();
                DoHighlight();
            }
        }

        /// <summary>
        /// 蓄積したログをクリアする
        /// </summary>
        private void ClearStoredLog()
        {
            m_storeFlag = false;
        }

        #endregion

        #region ハイライト

        /// <summary>
        /// 指定されたチャンネルでのハイライトをセットします
        /// </summary>
        /// <param name="channel">ハイライトするチャンネル</param>
        private void SetHighlight(Channel channel, string message)
        {
            highlightFlag = true;
            highlightChannel = channel;

            // ハイライトメッセージ一覧に追加
            ChannelMenuItem mesMenu = new ChannelMenuItem(channel);
            mesMenu.Text = message;
            mesMenu.Click += new EventHandler(menuChannelListChannelsMenuItem_Click);
            menuHilightedMessages.MenuItems.Add(mesMenu);

            // 該当チャンネルをハイライトチェックする
            foreach (ChannelMenuItem menu in m_channelPopupMenus)
            {
                if (menu.Channel == channel)
                {
                    menu.Checked = true;
                    break;
                }
            }

            if (!m_storeFlag) DoHighlight();
        }

        /// <summary>
        /// ハイライト処理を実行します
        /// </summary>
        private void DoHighlight()
        {
            if (highlightFlag && (highlightChannel != null))
            {
                if ((SettingManager.Data.HighlightMethod == EbIRCHilightMethod.Vibration) || (SettingManager.Data.HighlightMethod == EbIRCHilightMethod.VibrationAndLed))
                {
                    if (Led.AvailableLed(LedType.Vibrartion))
                    {
                        Led.SetLedStatus(LedType.Vibrartion, LedStatus.On);
                        clearHighlightTimer.Enabled = true;
                    }
                }
                if ((SettingManager.Data.HighlightMethod == EbIRCHilightMethod.Led) || (SettingManager.Data.HighlightMethod == EbIRCHilightMethod.VibrationAndLed))
                {
                    if (Led.AvailableLed(LedType.Yellow))
                    {
                        Led.SetLedStatus(LedType.Yellow, LedStatus.On);
                        clearHighlightTimer.Enabled = true;
                    }
                }
                if (SettingManager.Data.HighlightChannelChange)
                {
                    LoadChannel(highlightChannel);
                }
            }
            highlightFlag = false;
            highlightChannel = null;
        }

        /// <summary>
        /// ハイライトの設定をクリアします
        /// </summary>
        private void ClearHighlight()
        {
            if (Led.AvailableLed(LedType.Vibrartion))
            {
                Led.SetLedStatus(LedType.Vibrartion, LedStatus.Off);
            }
            if (Led.AvailableLed(LedType.Yellow))
            {
                Led.SetLedStatus(LedType.Yellow, LedStatus.Off);
            }
            clearHighlightTimer.Enabled = false;
        }

        #endregion

        #region UI表示コントロール

        /// <summary>
        /// 接続ボタンのテキストを更新する
        /// </summary>
        void SetConnectionMenuText()
        {
            if (ircClient.Status == IRCClientStatus.Disconnected)
            {
                connectionMenuItem.Text = Resources.ConnectionMenuCaption;
            }
            else
            {
                connectionMenuItem.Text = Resources.DisconnectMenuCaption;
            }
        }

        /// <summary>
        /// UI設定を反映
        /// </summary>
        void UpdateUISettings()
        {
            logTextBox.Font = SettingManager.Data.GetFont();
            infomationPanel.Visible = SettingManager.Data.TopicVisible;

            // サブニックネームリストを作成 ------------------------

            MenuItem newItem;

            // 現在のニックネームリストをクリア
            nicknameSwitchMenuItem.MenuItems.Clear();

            // デフォルトニックネームをセット
            newItem = new MenuItem();
            newItem.Text = SettingManager.Data.Profiles.ActiveProfile.Nickname;
            newItem.Click += new EventHandler(nicknameSwitcher_Click);
            nicknameSwitchMenuItem.MenuItems.Add(newItem);

            // ニックネームリストを登録
            foreach (string itemName in SettingManager.Data.SubNicknames)
            {
                if (!string.IsNullOrEmpty(itemName.Trim()))
                {
                    newItem = new MenuItem();
                    newItem.Text = itemName;
                    newItem.Click += new EventHandler(nicknameSwitcher_Click);
                    nicknameSwitchMenuItem.MenuItems.Add(newItem);
                }
            }

            // カスタム入力メニューを末尾に追加
            nicknameSwitchMenuItem.MenuItems.Add(menuNicknameInputMenuItem);

            // 接続キャッシュ設定
            ConnectionManager.ConnectionCacheLength = SettingManager.Data.CacheConnection ? 1 : 0;

            // 強制PONG
            pongTimer.Enabled = SettingManager.Data.ForcePong;

            // キーワード反応マッチオブジェクト
            highlightMatcher = SettingManager.Data.GetHighlightKeywordMatcher();
            dislikeMatcher = SettingManager.Data.GetDislikeKeywordMatcher();

            // ハイライト停止タイマーの周期
            clearHighlightTimer.Interval = SettingManager.Data.HighlightContinueTime;
        }

        #endregion

        /// <summary>
        /// 特殊キーボードオペレーション実行
        /// </summary>
        /// <param name="operation">実行するキーボードオペレーション</param>
        /// <returns>何らかの操作がされれば true </returns>
        private bool ProcessKeyOperation(EbIRCKeyOperations operation)
        {
            switch (operation)
            {

                case EbIRCKeyOperations.PageDown:
                    SendMessage(logTextBox.Handle, EM_SCROLL, SB_PAGEDOWN, IntPtr.Zero);
                    return true;

                case EbIRCKeyOperations.PageUp:
                    SendMessage(logTextBox.Handle, EM_SCROLL, SB_PAGEUP, IntPtr.Zero);
                    return true;

                case EbIRCKeyOperations.QuickChannelNext:
                    SwitchNextChannel();
                    return true;

                case EbIRCKeyOperations.QuickChannelPrev:
                    SwitchPrevChannel();
                    return true;

                case EbIRCKeyOperations.InputLogNext:
                    // 可動範囲内のとき
                    if (m_inputlogPtr < m_inputlog.Count)
                    {
                        // ポインタ移動
                        m_inputlogPtr++;
                    }

                    if (m_inputlogPtr == m_inputlog.Count)
                    {
                        inputTextBox.Text = string.Empty;
                    }
                    else
                    {
                        // テキストセット
                        inputTextBox.Text = m_inputlog[m_inputlogPtr];
                        // 全体を選択
                        inputTextBox.SelectAll();
                    } 
                    return true;

                case EbIRCKeyOperations.InputLogPrev:
                    // 可動範囲内のとき
                    if (m_inputlogPtr > 0)
                    {
                        // ポインタ移動
                        m_inputlogPtr--;
                        // テキストセット
                        inputTextBox.Text = m_inputlog[m_inputlogPtr];
                        // 全体を選択
                        inputTextBox.SelectAll();
                    }
                    return true;

                case EbIRCKeyOperations.FontSizeUp:
                    SettingManager.Data.FontSize++;
                    logTextBox.Font = SettingManager.Data.GetFont();
                    return true;

                case EbIRCKeyOperations.FontSizeDown:
                    if (SettingManager.Data.FontSize > 1)
                    {
                        SettingManager.Data.FontSize--;
                        logTextBox.Font = SettingManager.Data.GetFont();
                    }
                    return true;

                case EbIRCKeyOperations.NoOperation:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// 選択されているURLを取得する。
        /// </summary>
        /// <returns>開くべきURL。該当がなければ空文字列。</returns>
        private string GetSelectedURL()
        {
            // 正規表現パターン
            MatchCollection matches;  // 正規表現マッチのリスト
            int lineStart, lineEnd;   // 行の開始位置、終了位置
            string line;              // 抽出した行、URL

            // ログが空なら抜ける
            if (logTextBox.TextLength == 0) return string.Empty;

            // 行の範囲を調べる
            if (logTextBox.SelectionStart == logTextBox.TextLength)
            {
                lineStart = logTextBox.Text.LastIndexOf('\r', logTextBox.SelectionStart - 1);
                lineEnd = logTextBox.TextLength;
            }
            else
            {
                lineStart = logTextBox.Text.LastIndexOf('\r', logTextBox.SelectionStart - 1);
                lineEnd = logTextBox.Text.IndexOf('\r', lineStart + 1);
            }

            // 一致しなければ、行頭、行末にする
            if (lineStart == -1) lineStart = 0;
            if (lineEnd == -1) lineEnd = logTextBox.TextLength;

            // 行を抽出する
            line = logTextBox.Text.Substring(lineStart, lineEnd - lineStart);

            // 行の中のURLをさがす
            matches = UrlRegex.Matches(line);

            // なければ抜ける
            if (matches.Count == 0)
                return string.Empty;

            // カーソルの下にあるURLをさがす
            foreach (Match match in matches)
            {
                if (((match.Groups[0].Index + lineStart) <= logTextBox.SelectionStart)
                    && ((match.Groups[0].Index + match.Groups[0].Length + lineStart) >= logTextBox.SelectionStart)
                )
                {
                    // 一致したらこれを開く
                    return match.Groups[0].Value;
                }

            }

            // みつからなかったら最初のやつを開く
            return matches[0].Groups[0].Value;
        }

        /// <summary>
        /// URLを開く
        /// </summary>
        /// <param name="url">開くURL</param>
        private static void OpenUrl(string url)
        {
            try
            {
                // ttp→http 変換
                if (url.StartsWith("ttp:"))
                {
                    url = "h" + url;
                }

                System.Diagnostics.Process.Start(url, "");
            }
            catch (Win32Exception)
            {
                MessageBox.Show(Resources.CannotOpenURL, Resources.FaildBoot,
                    MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        /// <summary>
        /// PONG送信タイマーイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pongTimer_Tick(object sender, EventArgs e)
        {
            if (ircClient.Status == IRCClientStatus.Online)
            {
                ircClient.SendCommand("PONG :" + SettingManager.Data.Profiles.ActiveProfile.Server);
            }
        }

        /// <summary>
        /// ハイライト停止タイマー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearHighlightTimer_Tick(object sender, EventArgs e)
        {
            ClearHighlight();
        }
    }
}