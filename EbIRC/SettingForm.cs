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
        ConnectionProfileCollection m_editingProfiles;

        public SettingForm()
        {
            InitializeComponent();
            InTheHand.WindowsMobile.Forms.TabControlHelper.EnableVisualStyle(tabControl);
        }

        #region フォームロード/アンロード(設定読み書き)

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

            // 設定を読み込む
            m_editingProfiles = (ConnectionProfileCollection)SettingManager.Data.Profiles.Clone();
            ReloadProfileList();
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
            qsSortHighlightedCheckBox.Checked = SettingManager.Data.QuickSwitchHilightsSort;
            qsSortUnreadCheckBox.Checked = SettingManager.Data.QuickSwitchUnreadCountSort;
            multiMenuFunctionComboBox.SelectedIndex = (int)SettingManager.Data.MultiMenuOperation;
        }

        private void SettingForm_Closing(object sender, CancelEventArgs e)
        {
            // 設定を書き込む
            SettingManager.Data.Profiles = m_editingProfiles;
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
            SettingManager.Data.HighlightMethod = (EbIRCHighlightMethod)highlightMethodComboBox.SelectedIndex;
            SettingManager.Data.HighlightChannelChange = highlightChannelCheckBox.Checked;
            SettingManager.Data.DislikeKeywords = dislikeWordsTextBox.Text.Replace("\r", "").Split('\n');
            SettingManager.Data.UseRegexDislike = dislikeUseRegexCheckBox.Checked;
            SettingManager.Data.LogingEnable = enableLoggingCheckBox.Checked;
            SettingManager.Data.LogDirectory = logDirectoryNameTextBox.Text;
            SettingManager.Data.QuickSwitchHilightsSort = qsSortHighlightedCheckBox.Checked;
            SettingManager.Data.QuickSwitchUnreadCountSort = qsSortUnreadCheckBox.Checked;

            SettingManager.Data.MultiMenuOperation = (EbIRCMultiMenuOperations)multiMenuFunctionComboBox.SelectedIndex;


            SettingManager.WriteSetting();
        }

        #endregion

        #region プロファイルの編集

        /// <summary>
        /// プロファイルの追加ボタン
        /// </summary>
        private void profileAddButton_Click(object sender, EventArgs e)
        {
            using (ServerSettingForm serverSettingForm = new ServerSettingForm())
            {
                ConnectionProfile prof = new ConnectionProfile(Resources.NewProfileName);
                serverSettingForm.Profile = prof;

                if (serverSettingForm.ShowDialog() == DialogResult.OK)
                {
                    m_editingProfiles.Profile.Add(serverSettingForm.Profile);
                    ReloadProfileList();
                }
            }
        }

        /// <summary>
        /// プロファイルの編集ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void profileEditButton_Click(object sender, EventArgs e)
        {
            using (ServerSettingForm serverSettingForm = new ServerSettingForm())
            {
                ConnectionProfile prof = m_editingProfiles.Profile[profileSelectListview.SelectedIndices[0]];
                serverSettingForm.Profile = prof;
                if (serverSettingForm.ShowDialog() == DialogResult.OK)
                {
                    m_editingProfiles.Profile[profileSelectListview.SelectedIndices[0]] = serverSettingForm.Profile;
                    ReloadProfileList();
                }
            }
        }

        /// <summary>
        /// プロファイルの削除ボタン
        /// </summary>
        private void profileRemoveButton_Click(object sender, EventArgs e)
        {
            m_editingProfiles.Profile.RemoveAt(profileSelectListview.SelectedIndices[0]);
            ReloadProfileList();
        }

        /// <summary>
        /// プロファイルの選択ボタン
        /// </summary>
        private void profileMarkActiveButton_Click(object sender, EventArgs e)
        {
            m_editingProfiles.ActiveProfileIndex = profileSelectListview.SelectedIndices[0];
            ReloadProfileList();
        }

        /// <summary>
        /// プロファイルの選択(ボタンの有効状態の切り替え)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void profileSelectListview_SelectedIndexChanged(object sender, EventArgs e)
        {
            profileEditButton.Enabled = (profileSelectListview.SelectedIndices.Count > 0);
            profileRemoveButton.Enabled = ((profileSelectListview.SelectedIndices.Count > 0) && (profileSelectListview.Items.Count > 1) && (m_editingProfiles.ActiveProfileIndex != profileSelectListview.SelectedIndices[0]));
            profileMarkActiveButton.Enabled = ((profileSelectListview.SelectedIndices.Count > 0) && (m_editingProfiles.ActiveProfileIndex != profileSelectListview.SelectedIndices[0]));
        }

        /// <summary>
        /// プロファイル一覧の再読み込み
        /// </summary>
        private void ReloadProfileList()
        {
            profileSelectListview.Items.Clear();
            foreach (ConnectionProfile prof in m_editingProfiles.Profile)
            {
                ListViewItem item = new ListViewItem();

                if (m_editingProfiles.ActiveProfile == prof)
                {
                    item.Text = "* " + prof.ProfileName;
                }
                else
                {
                    item.Text = prof.ProfileName;
                }

                item.Tag = prof;
                profileSelectListview.Items.Add(item);
            }
        }

        #endregion

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

        #endregion

        #region リサイズアクション

        private void inputPanel_EnabledChanged(object sender, EventArgs e)
        {
            ResizeAction();
        }

        private void SettingForm_Resize(object sender, EventArgs e)
        {
            ResizeAction();
        }

        private void ResizeAction()
        {
            if (inputPanel.Enabled)
            {
                tabControl.Height = this.ClientSize.Height - inputPanel.Bounds.Height;
            }
            else
            {
                tabControl.Height = this.ClientSize.Height;
            }
        }

        #endregion

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