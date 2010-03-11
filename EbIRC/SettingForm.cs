using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using EbiSoft.EbIRC.Properties;
using EbiSoft.Library.Mobile;
using EbiSoft.EbIRC.Settings;

namespace EbiSoft.EbIRC
{
    public partial class SettingForm : Form
    {
        #region P/Invoke 宣言

#if Win32PInvoke
        [DllImport("user32", CharSet = CharSet.Auto)]
        private extern static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);
        [DllImport("user32", CharSet = CharSet.Auto, EntryPoint="SendMessage")]
        private extern static IntPtr SendMessage2(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern uint keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
#else
        [DllImport("coredll", CharSet = CharSet.Auto)]
        private extern static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);
        [DllImport("coredll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        private extern static IntPtr SendMessage2(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("coredll")]
        private static extern uint keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
#endif

        private const byte VK_TAB = 0x09;
        private const byte VK_SHIFT = 0x10;
        private const uint KEYEVENTF_KEYUP = 2;
        
        private const int WM_NOTIFY = 0x004E;
        private const int DTN_FIRST = -760;
        private const int DTN_DROPDOWN = (DTN_FIRST + 6);
        private const int DTN_CLOSEUP = (DTN_FIRST + 7);

        [StructLayout(LayoutKind.Sequential)]
        private struct NMHDR
        {
            public IntPtr hwndFrom;
            public uint idFrom;
            public int code; //uint
        }

        #endregion

        TextBox currentMultilineBox = null;
        int     lastProfileIndex    = -1;

        public SettingForm()
        {
            InitializeComponent();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            // フォント一覧の読み込み
            fontNameInputBox.Items.Clear();
            using (System.Drawing.Text.InstalledFontCollection ifc = new System.Drawing.Text.InstalledFontCollection())
            {
                foreach (FontFamily ff in ifc.Families)
                {
                    fontNameInputBox.Items.Add(ff.Name);
                    ff.Dispose();
                }
            }

            // デフォルトサーバーリストの読み込み
            serverInputbox.Items.Clear();
            foreach (string server in SettingManager.Data.DefaultServers)
            {
                serverInputbox.Items.Add(server);
            }

            // エンコーディングリストの読み込み
            encodingSelectBox.Items.Clear();
            foreach (string encode in Resources.EncodeList.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n')) {
                encodingSelectBox.Items.Add(encode);
            }

            // 設定を読み込む
            profileSelectBox.Items.Clear();
            foreach (ConnectionProfile prof in SettingManager.Data.Profiles.Profile)
            {
                profileSelectBox.Items.Add(prof);
            }
            profileSelectBox.SelectedIndex = SettingManager.Data.Profiles.ActiveProfileIndex;
            fontNameInputBox.Text = SettingManager.Data.FontName;
            fontSizeComboBox.Text = SettingManager.Data.FontSize.ToString();
            visibleTopicPanelCheckbox.Checked = SettingManager.Data.TopicVisible;
            defaultLoadOnConnectCheckBox.Checked = SettingManager.Data.SelectChannelAtConnect;

            if (SettingManager.Data.VerticalKeyOperation == 5)
            {
                verticalKeySelectBox.SelectedIndex = 0;
            }
            else
            {
                verticalKeySelectBox.SelectedIndex = SettingManager.Data.VerticalKeyOperation + 1;
            }
            horizontalKeySelectBox.SelectedIndex = SettingManager.Data.HorizontalKeyOperation;
            ctrlVerticalKeySelectBox.SelectedIndex = SettingManager.Data.VerticalKeyWithCtrlOperation;
            ctrlHorizontalKeySelectBox.SelectedIndex = SettingManager.Data.HorizontalKeyWithCtrlOperation;

            subNicknameInputBox.Text = string.Join("\r\n", SettingManager.Data.SubNicknames);
            confimDisconnectCheckBox.Checked = SettingManager.Data.ConfimDisconnect;
            confimExitCheckBox.Checked = SettingManager.Data.ConfimExit;
            cacheConnectionCheckBox.Checked = SettingManager.Data.CacheConnection;
            reverseSoftKeyCheckBox.Checked = SettingManager.Data.ReverseSoftKey;
            scrollLinesTextBox.Text = SettingManager.Data.ScrollLines.ToString();
            forcePongCheckBox.Checked = SettingManager.Data.ForcePong;
            highlightWordsTextBox.Text = string.Join("\r\n", SettingManager.Data.HighlightKeywords);
            highlightUseRegexCheckbox.Checked = SettingManager.Data.UseRegexHighlight;
            highlightMethodComboBox.SelectedIndex = (int)SettingManager.Data.HighlightMethod;
            highlightChannelCheckBox.Checked = SettingManager.Data.HighlightChannelChange;
            dislikeWordsTextBox.Text = string.Join("\r\n", SettingManager.Data.DislikeKeywords);
            dislikeUseRegexCheckBox.Checked = SettingManager.Data.UseRegexDislike;
            enableLoggingCheckBox.Checked = SettingManager.Data.LogingEnable;
            logDirectoryNameTextBox.Text = SettingManager.Data.LogDirectory;
            qsSortHilightedCheckBox.Checked = SettingManager.Data.QuickSwitchHilightsSort;
            qsSortUnreadCheckBox.Checked = SettingManager.Data.QuickSwitchUnreadCountSort;
        }

        private void SettingForm_Closing(object sender, CancelEventArgs e)
        {
            // 設定を書き込む
            saveLastProfile();
            ConnectionProfileData data = new ConnectionProfileData();
            foreach (object obj in profileSelectBox.Items)
            {
                ConnectionProfile prof = obj as ConnectionProfile;
                data.Profile.Add(prof);
            }
            data.ActiveProfileIndex = profileSelectBox.SelectedIndex;
            SettingManager.Data.Profiles = data;
            SettingManager.Data.Profiles.ActiveProfile.Password = passwordInputBox.Text;
            SettingManager.Data.SelectChannelAtConnect = defaultLoadOnConnectCheckBox.Checked;
            SettingManager.Data.FontName = fontNameInputBox.Text;
            SettingManager.Data.TopicVisible = visibleTopicPanelCheckbox.Checked;

            if (verticalKeySelectBox.SelectedIndex == 0)
            {
                SettingManager.Data.VerticalKeyOperation = 5;
            }
            else
            {
                SettingManager.Data.VerticalKeyOperation = verticalKeySelectBox.SelectedIndex - 1;
            }
            SettingManager.Data.HorizontalKeyOperation = horizontalKeySelectBox.SelectedIndex;
            SettingManager.Data.VerticalKeyWithCtrlOperation = ctrlVerticalKeySelectBox.SelectedIndex;
            SettingManager.Data.HorizontalKeyWithCtrlOperation = ctrlHorizontalKeySelectBox.SelectedIndex;

            try
            {
                SettingManager.Data.FontSize = int.Parse(fontSizeComboBox.Text);
            }
            catch (Exception) { } // 設定を保存しない
            SettingManager.Data.SubNicknames = subNicknameInputBox.Text.Replace("\r", "").Split('\n');
            SettingManager.Data.ConfimDisconnect = confimDisconnectCheckBox.Checked;
            SettingManager.Data.ConfimExit = confimExitCheckBox.Checked;
            SettingManager.Data.CacheConnection = cacheConnectionCheckBox.Checked;
            SettingManager.Data.ReverseSoftKey = reverseSoftKeyCheckBox.Checked;
            try
            {
                SettingManager.Data.ScrollLines = int.Parse(scrollLinesTextBox.Text);
            }
            catch (Exception) { }
            SettingManager.Data.ForcePong = forcePongCheckBox.Checked;
            SettingManager.Data.HighlightKeywords = highlightWordsTextBox.Text.Replace("\r", "").Split('\n');
            SettingManager.Data.UseRegexHighlight=highlightUseRegexCheckbox.Checked;
            SettingManager.Data.HighlightMethod = (EbIRCHilightMethod)highlightMethodComboBox.SelectedIndex;
            SettingManager.Data.HighlightChannelChange = highlightChannelCheckBox.Checked;
            SettingManager.Data.DislikeKeywords = dislikeWordsTextBox.Text.Replace("\r", "").Split('\n');
            SettingManager.Data.UseRegexDislike = dislikeUseRegexCheckBox.Checked;
            SettingManager.Data.LogingEnable = enableLoggingCheckBox.Checked;
            SettingManager.Data.LogDirectory = logDirectoryNameTextBox.Text;
            SettingManager.Data.QuickSwitchHilightsSort = qsSortHilightedCheckBox.Checked;
            SettingManager.Data.QuickSwitchUnreadCountSort = qsSortUnreadCheckBox.Checked;

            SettingManager.WriteSetting();
        }

        #region キー移動関連

        /// <summary>
        /// 次のコントロールにフォーカスする
        /// </summary>
        private static void FocusNextControl()
        {
            keybd_event(VK_TAB, 0, 0, (UIntPtr)0);
            keybd_event(VK_TAB, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        /// <summary>
        /// 前のコントロールにフォーカスする
        /// </summary>
        private static void FocusPrevControl()
        {
            keybd_event(VK_SHIFT, 0, 0, (UIntPtr)0);
            keybd_event(VK_TAB, 0, 0, (UIntPtr)0);
            keybd_event(VK_TAB, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
            keybd_event(VK_SHIFT, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        private void SettingForm_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                // Up
                if (currentMultilineBox != null)
                {
                    int index = currentMultilineBox.Text.IndexOf('\r');
                    if ((index != -1) && (currentMultilineBox.SelectionStart > index))
                    {
                        return;
                    }
                }
                FocusPrevControl();
                e.Handled = true;
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Down))
            {
                if (currentMultilineBox != null)
                {
                    int index = currentMultilineBox.Text.LastIndexOf('\r');
                    if ((index != -1) && (currentMultilineBox.SelectionStart < index))
                    {
                        return;
                    }
                }
                // Down
                FocusNextControl();
                e.Handled = true;
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Left))
            {
                // Left
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Right))
            {
                // Right
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Enter))
            {
                // Enter
                if (sender is ComboBox)
                {
                    NMHDR data = new NMHDR();
                    data.hwndFrom = this.Handle;
                    data.code = DTN_DROPDOWN;
                    data.idFrom = 0;
                    IntPtr ptr = IntPtr.Zero;
                    Marshal.StructureToPtr(data, ptr, false);
                    SendMessage((sender as ComboBox).Handle, WM_NOTIFY, 0, ptr);
                }
            }
        }

        private void multiLineInputbox_GotFocus(object sender, EventArgs e)
        {
            currentMultilineBox = (sender as TextBox);
        }

        private void multiLineInputbox_LostFocus(object sender, EventArgs e)
        {
            currentMultilineBox = null;
        }

        private void label3_ParentChanged(object sender, EventArgs e)
        {

        }

        #endregion

        /// <summary>
        /// プロファイルの設定をクラスに反映
        /// </summary>
        private void saveLastProfile()
        {
            if (lastProfileIndex < 0) return;

            ConnectionProfile prof = profileSelectBox.Items[lastProfileIndex] as ConnectionProfile;
            prof.Server = serverInputbox.Text;
            try
            {
                prof.Port = int.Parse(portInputBox.Text);
            }
            catch (Exception) { } // 設定を保存しない
            prof.Nickname = nicknameInputbox.Text;
            prof.Realname = nameInputbox.Text;
            prof.Encoding = encodingSelectBox.Text;
            prof.UseSsl = serverUseSslCheckBox.Checked;
        }

        /// <summary>
        /// プロファイルの設定をコントロールに読み込み
        /// </summary>
        private void loadActiveProfile()
        {
            if (profileSelectBox.SelectedIndex < 0) return;

            ConnectionProfile prof = profileSelectBox.SelectedItem as ConnectionProfile;
            serverInputbox.Text = prof.Server;
            portInputBox.Text = prof.Port.ToString();
            nicknameInputbox.Text = prof.Nickname;
            nameInputbox.Text = prof.Realname;
            passwordInputBox.Text = prof.Password;
            encodingSelectBox.Text = prof.Encoding;
            serverUseSslCheckBox.Checked = prof.UseSsl;
        }

        /// <summary>
        /// リストの選択が変わったときに発生するイベント
        /// </summary>
        private void profileSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveLastProfile();
            loadActiveProfile();
            lastProfileIndex = profileSelectBox.SelectedIndex;
        }

        /// <summary>
        /// プロファイルの追加ボタン
        /// </summary>
        private void profileAddButton_Click(object sender, EventArgs e)
        {
            using (InputBoxForm form = new InputBoxForm())
            {
                form.Text = Resources.ProfileAddDialogTitle;
                form.Description = Resources.ProfileAddDialogCaption;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(form.Value))
                    {
                        profileSelectBox.Items.Add(new ConnectionProfile(form.Value));
                        profileSelectBox.SelectedIndex = profileSelectBox.Items.Count - 1;
                    }
                }
            }
        }

        /// <summary>
        /// プロファイルの削除ボタン
        /// </summary>
        private void profileRemoveButton_Click(object sender, EventArgs e)
        {
            if (profileSelectBox.SelectedIndex < 0) return;
            if (profileSelectBox.Items.Count == 1)
            {
                MessageBox.Show(Resources.CannotRemoveAllProfileMessage);
                return;
            }

            // 次にロードするプロファイルを決める
            int nextActiveIndex = lastProfileIndex;
            if (nextActiveIndex <= profileSelectBox.Items.Count - 1)
            {
                nextActiveIndex--;
            }
            lastProfileIndex = -1;

            profileSelectBox.Items.RemoveAt(profileSelectBox.SelectedIndex);
            profileSelectBox.SelectedIndex = nextActiveIndex;
        }

        /// <summary>
        /// 保存して閉じるメニュー
        /// </summary>
        private void saveCloseMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void highlightUseRegexCheckbox_CheckStateChanged(object sender, EventArgs e)
        {
            highlightWordsTextBox.Multiline = !highlightUseRegexCheckbox.Checked;
            if (!highlightWordsTextBox.Multiline)
            {
                highlightWordsTextBox.Text = highlightWordsTextBox.Text.Replace("\r", string.Empty).Replace('\n', '|');
            }
        }

        private void dislikeUseRegexCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            dislikeWordsTextBox.Multiline = !dislikeUseRegexCheckBox.Checked;
            if (!dislikeWordsTextBox.Multiline)
            {
                dislikeWordsTextBox.Text = dislikeWordsTextBox.Text.Replace("\r", string.Empty).Replace('\n', '|');
            }
        }

        private void logDirectoryBrowseButton_Click(object sender, EventArgs e)
        {
            using (FolderSelectDialog dialog = new FolderSelectDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    logDirectoryNameTextBox.Text = dialog.SelectedDirectory;
                }
            }
        }
    }
}