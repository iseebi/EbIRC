using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EbiSoft.EbIRC.Settings;
using EbiSoft.EbIRC.Properties;
using System.Runtime.InteropServices;

namespace EbiSoft.EbIRC
{
    public partial class ServerSettingForm : Form
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

        private TextBox currentMultilineBox = null;
        private int m_lastSettingIndex;
        private ConnectionProfile m_profile;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ServerSettingForm()
        {
            InitializeComponent();
            InTheHand.WindowsMobile.Forms.TabControlHelper.EnableVisualStyle(tabControl);

            // デフォルトサーバーリストの読み込み
            serverComboBox.Items.Clear();
            foreach (string server in SettingManager.Data.DefaultServers)
            {
                serverComboBox.Items.Add(server);
            }

            // エンコーディングリストの読み込み
            encodingComboBox.Items.Clear();
            foreach (string encode in Resources.EncodeList.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n'))
            {
                encodingComboBox.Items.Add(encode);
            }
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

        private void ServerSettingForm_KeyDown(object sender, KeyEventArgs e)
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

        #region その他イベント

        /// <summary>
        /// 読み込みイベント
        /// </summary>
        private void ServerSettingForm_Load(object sender, EventArgs e)
        {
            m_lastSettingIndex = -1;
            profileNameTextBox.Focus();
        }

        /// <summary>
        /// 閉じるメニュー
        /// </summary>
        private void closeMenuItem_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// SSLチェックの切り替え
        /// </summary>
        private void serverUseSslCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            serverSslNotValidateCheckBox.Enabled = serverUseSslCheckBox.Checked;
        }

        #endregion

        #region リサイズイベント

        private void inputPanel_EnabledChanged(object sender, EventArgs e)
        {
            ResizeAction();
        }

        private void ServerSettingForm_Resize(object sender, EventArgs e)
        {
            ResizeAction();
        }

        void ResizeAction()
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

        #region 外部接続プロパティ

        /// <summary>
        /// 編集中のプロファイル
        /// </summary>
        public ConnectionProfile Profile
        {
            get
            {
                if (m_profile == null)
                {
                    m_profile = new ConnectionProfile();
                }

                // Basic
                m_profile.ProfileName = profileNameTextBox.Text;
                m_profile.Server = serverComboBox.Text;
                m_profile.Port = int.Parse(portTextBox.Text);
                m_profile.UseSsl = serverUseSslCheckBox.Checked;
                m_profile.NoValidation = serverSslNotValidateCheckBox.Checked;
                m_profile.Nickname = nicknameTextBox.Text;
                m_profile.Encoding = encodingComboBox.Text;

                // Advanced
                m_profile.Password = serverPasswordTextBox.Text;
                m_profile.LoginName = loginNameTextBox.Text;
                m_profile.Realname = realnameTextBox.Text;
                m_profile.NickServPassword = nickservPasswordTextBox.Text;

                // Channels
                ApplyChannelEdit();
                m_profile.Channels.Clear();
                foreach (ListViewItem item in channelListView.Items)
                {
                    ChannelSetting ch = (ChannelSetting)item.Tag;
                    if (!string.IsNullOrEmpty(ch.Name.Trim()))
                    {
                        m_profile.Channels.Add(ch);
                    }
                }

                return m_profile;
            }
            set
            {
                m_profile = value;

                // Basic
                profileNameTextBox.Text = m_profile.ProfileName;
                serverComboBox.Text = m_profile.Server;
                portTextBox.Text = m_profile.Port.ToString();
                serverUseSslCheckBox.Checked = m_profile.UseSsl;
                serverSslNotValidateCheckBox.Checked = m_profile.NoValidation;
                nicknameTextBox.Text = m_profile.Nickname;
                encodingComboBox.Text = m_profile.Encoding;

                // Advanced
                serverPasswordTextBox.Text = m_profile.Password;
                loginNameTextBox.Text = m_profile.LoginName;
                realnameTextBox.Text = m_profile.Realname;
                nickservPasswordTextBox.Text = m_profile.NickServPassword;

                // Channels
                channelListView.Items.Clear();
                foreach (ChannelSetting ch in m_profile.Channels)
                {
                    ListViewItem item = new ListViewItem(ch.Name);
                    item.Tag = ch;
                    channelListView.Items.Add(item);
                }
            }
        }

        #endregion

        #region チャンネル編集

        /// <summary>
        /// 選択されているチャンネルが変更されたとき
        /// </summary>
        private void channelListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyChannelEdit();
            if (channelListView.SelectedIndices.Count > 0)
            {
                LoadChannelEdit();
                m_lastSettingIndex = channelListView.SelectedIndices[0];
                channelUpButton.Enabled = (channelListView.SelectedIndices[0] > 0);
                channelDownButton.Enabled = (channelListView.SelectedIndices[0] < channelListView.Items.Count);
                removeChannelButton.Enabled = true;

                channelNameTextBox.Enabled = true;
                channelPasswordTextBox.Enabled = true;
                channelIgnoreUnreadSortCheckBox.Enabled = true;
            }
            else
            {
                m_lastSettingIndex = -1;
                channelUpButton.Enabled = false;
                channelDownButton.Enabled = false;
                removeChannelButton.Enabled = false;

                channelNameTextBox.Enabled = true;
                channelPasswordTextBox.Enabled = true;
                channelIgnoreUnreadSortCheckBox.Enabled = false;
                channelNameTextBox.Text = string.Empty;
                channelPasswordTextBox.Text = string.Empty;
                channelIgnoreUnreadSortCheckBox.Checked = false;
            }
        }

        /// <summary>
        /// 上に移動する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void channelUpButton_Click(object sender, EventArgs e)
        {
            int cur = channelListView.SelectedIndices[0];
            int tg = cur - 1;
            SwapChannel(cur, tg);
        }

        /// <summary>
        /// 下に移動する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void channelDownButton_Click(object sender, EventArgs e)
        {
            int cur = channelListView.SelectedIndices[0];
            int tg = cur + 1;
            SwapChannel(cur, tg);
        }

        /// <summary>
        /// チャンネルを入れ替える
        /// </summary>
        /// <param name="cur">選択中のインデックス</param>
        /// <param name="tg">入れ替え対象のインデックス</param>
        private void SwapChannel(int cur, int tg)
        {
            // 入れ替え
            object swapTemp;
            swapTemp = channelListView.Items[tg].Tag;
            channelListView.Items[tg].Tag = channelListView.Items[cur].Tag;
            channelListView.Items[cur].Tag = swapTemp;

            // 名前反映
            channelListView.Items[cur].Text = ((ChannelSetting)channelListView.Items[cur].Tag).Name;
            channelListView.Items[tg].Text = ((ChannelSetting)channelListView.Items[tg].Tag).Name;

            // 再選択
            m_lastSettingIndex = -1;
            channelListView.Items[tg].Selected = true;
        }

        /// <summary>
        /// 追加する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addChannelButton_Click(object sender, EventArgs e)
        {
            ListViewItem item = new ListViewItem();
            ChannelSetting ch = new ChannelSetting(Resources.ChannelSettingNewChannel);
            item.Tag = ch;
            channelListView.Items.Add(item);
            item.Selected = true;

            Application.DoEvents();

            channelNameTextBox.Focus();
            if (channelNameTextBox.TextLength > 2)
            {
                channelNameTextBox.Select(1, channelNameTextBox.TextLength - 1);
            }
        }

        /// <summary>
        /// 削除する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeChannelButton_Click(object sender, EventArgs e)
        {
            ApplyChannelEdit();
            m_lastSettingIndex = -1;
            channelListView.Items.RemoveAt(channelListView.SelectedIndices[0]);
        }

        /// <summary>
        /// チャンネルの編集を反映する
        /// </summary>
        private void ApplyChannelEdit()
        {
            if (m_lastSettingIndex != -1)
            {
                ChannelSetting ch = (ChannelSetting) channelListView.Items[m_lastSettingIndex].Tag;
                ch.Name = channelNameTextBox.Text;
                ch.Password = channelPasswordTextBox.Text;
                ch.IgnoreInUnreadCountSort = channelIgnoreUnreadSortCheckBox.Checked;
                channelListView.Items[m_lastSettingIndex].Text = ch.Name;
            }
        }
        
        /// <summary>
        /// チャンネルの編集を開始する
        /// </summary>
        private void LoadChannelEdit()
        {
            if (channelListView.SelectedIndices[0] != -1)
            {
                ChannelSetting ch = (ChannelSetting)channelListView.Items[channelListView.SelectedIndices[0]].Tag;
                channelNameTextBox.Text = ch.Name;
                channelPasswordTextBox.Text = ch.Password;
                channelIgnoreUnreadSortCheckBox.Checked = ch.IgnoreInUnreadCountSort;
            }
        }

        private void channelNameTextBox_TextChanged(object sender, EventArgs e)
        {
            ApplyChannelEdit();
        }

        #endregion
    }
}