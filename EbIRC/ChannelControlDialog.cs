using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EbiSoft.EbIRC.Properties;
using EbiSoft.EbIRC.Settings;

namespace EbiSoft.EbIRC
{
    public partial class ChannelControlDialog : Form
    {
        const int SPLITTER_SIZE = 4;
        const int SCROLLBAR_MARGIN = 30;

        const int IMAGE_NORMAL_MEMBER = 0;
        const int IMAGE_OPERATOR_MEMBER = 1;
        const int IMAGE_OFFLINE_CHANNEL = 2;
        const int IMAGE_ONLINE_CHANNEL = 3;

        #region 初期化

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ChannelControlDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォーム読み込み時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelControlDialog_Load(object sender, EventArgs e)
        {
            LoadChannelList();
            openedChannelListview.Focus();
            SelectedIndexReset();
        }

        #endregion

        #region 一覧作成

        /// <summary>
        /// 読み込み済みチャンネルの一覧を作成します。
        /// </summary>
        private void LoadChannelList()
        {
            // チャンネルの一覧を作成
            openedChannelListview.Items.Clear();
            addChannel((Owner as EbIrcMainForm).ServerChannel);
            foreach (Channel ch in (Owner as EbIrcMainForm).Channels.Values)
            {
                addChannel(ch);
            }
        }

        /// <summary>
        /// リストにチャンネルを追加する
        /// </summary>
        /// <param name="ch"></param>
        private void addChannel(Channel ch)
        {
            ListViewItem item = new ListViewItem(ch.Name);
            item.Tag = ch;
            if (ch.IsChannel)
            {
                item.ImageIndex = ch.IsJoin ? IMAGE_ONLINE_CHANNEL : IMAGE_OFFLINE_CHANNEL;
            }
            else
            {
                item.ImageIndex = IMAGE_NORMAL_MEMBER;
            }
            openedChannelListview.Items.Add(item);

            // 選択されていたらメンバー一覧先出しで作成
            if (((Owner as EbIrcMainForm).CurrentChannel == ch))
            {
                item.Selected = true;
                LoadMemberList();
            }
        }

        /// <summary>
        /// チャンネル選択リストで選択されているチャンネルのメンバー一覧を作成します。
        /// </summary>
        private void LoadMemberList()
        {
            memberListView.Items.Clear();
            
            // チャンネルが見選択の場合は抜ける
            if (openedChannelListview.SelectedIndices.Count == 0) return;

            // チャンネルを取得
            Channel ch = openedChannelListview.Items[openedChannelListview.SelectedIndices[0]].Tag as Channel;

            // チャンネル内のメンバ一覧を作成
            memberListView.SuspendLayout();
            List<string> members = new List<string>(ch.Members);
            members.Sort();
            foreach (string member in members)
            {
                ListViewItem item;
                if (member.StartsWith("@"))
                {
                    item = new ListViewItem(member.Substring(1));
                    item.ImageIndex = IMAGE_OPERATOR_MEMBER;
                }
                else
                {
                    item = new ListViewItem(member);
                    item.ImageIndex = IMAGE_NORMAL_MEMBER;
                }

                memberListView.Items.Add(item);
            }
            memberListView.ResumeLayout();
        }

        #endregion

        /// <summary>
        /// 閉じるメニュー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// チャンネル追加メニュー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addSenderMenuItem_Click(object sender, EventArgs e)
        {
            using (InputBoxForm form = new InputBoxForm()) {
                form.Text = Resources.ChannelAddDialogTitle;
                form.Description = Resources.ChannelAddDialogCaption;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    (Owner as EbIrcMainForm).AddChannel(form.Value, false, null);
                    LoadChannelList();
                }
            }
        }

        #region 開いているチャンネル/PM コンテキストメニュー

        /// <summary>
        /// チャンネル / PM 一覧
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openedChannelContextMenu_Popup(object sender, EventArgs e)
        {
            if (openedChannelListview.SelectedIndices.Count == 0)
            {
                // チャンネルが選択されてない場合
                joinMenuItem.Enabled = false;
                leaveMenuItem.Enabled = false;
                removeChannelMenuItem.Enabled = false;
            }
            else
            {
                // チャンネルが選択されている場合
                Channel ch = openedChannelListview.Items[openedChannelListview.SelectedIndices[0]].Tag as Channel;
                if (ch.IsChannel)
                {
                    // チャンネルがチャンネルの場合
                    joinMenuItem.Enabled = !ch.IsJoin;
                    leaveMenuItem.Enabled = ch.IsJoin;
                    removeChannelMenuItem.Enabled = !ch.IsDefaultChannel;
                }
                else
                {
                    // チャンネルがPMの場合
                    joinMenuItem.Enabled = false;
                    leaveMenuItem.Enabled = false;
                    removeChannelMenuItem.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 入室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void joinMenuItem_Click(object sender, EventArgs e)
        {
            // チャンネルが選択されていない場合、抜ける
            if (openedChannelListview.SelectedIndices.Count == 0) return;
            Channel ch = openedChannelListview.Items[openedChannelListview.SelectedIndices[0]].Tag as Channel;
            if (ch.IsChannel && (!ch.IsJoin))
            {
                // チャンネルパスワード指定接続
                ChannelSetting chSetting = SettingManager.Data.Profiles.ActiveProfile.Channels.SearchChannel(ch.Name);
                if ((chSetting != null) && (!string.IsNullOrEmpty(chSetting.Password)))
                {
                    (Owner as EbIrcMainForm).IRCClient.JoinChannel(ch.Name, chSetting.Password);
                }
                else
                {
                    (Owner as EbIrcMainForm).IRCClient.JoinChannel(ch.Name, ch.Password);
                }
            }
        }

        /// <summary>
        /// 退室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void leaveMenuItem_Click(object sender, EventArgs e)
        {
            // チャンネルが選択されていない場合、抜ける
            if (openedChannelListview.SelectedIndices.Count == 0) return;
            Channel ch = openedChannelListview.Items[openedChannelListview.SelectedIndices[0]].Tag as Channel;
            if (ch.IsChannel && ch.IsJoin)
            {
                (Owner as EbIrcMainForm).IRCClient.LeaveChannel(ch.Name);
            }
        }

        /// <summary>
        /// リストから削除する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeChannelMenuItem_Click(object sender, EventArgs e)
        {
            if (openedChannelListview.SelectedIndices.Count == 0) return;
            Channel ch = openedChannelListview.Items[openedChannelListview.SelectedIndices[0]].Tag as Channel;

            // チャンネルにJoinしてる場合は抜ける
            if (ch.IsChannel && ch.IsJoin)
            {
                (Owner as EbIrcMainForm).IRCClient.LeaveChannel(ch.Name);
            }

            // チャンネル一覧から削除する
            (Owner as EbIrcMainForm).RemoveChannel(ch.Name);

            // チャンネル一覧を再構築する
            LoadChannelList();
            memberListView.Clear();
        }

        #endregion

        #region チャンネルの参加者 コンテキストメニュー

        /// <summary>
        /// チャンネルの参加者 コンテキストメニュー開くときのイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void memberContextMenu_Popup(object sender, EventArgs e)
        {
            addPMMemberMenuItem.Enabled = (memberListView.SelectedIndices.Count != 0);
        }

        /// <summary>
        /// PM送信先追加メニュー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addPMMemberMenuItem_Click(object sender, EventArgs e)
        {
            if (memberListView.SelectedIndices.Count == 0) return; 
            if (openedChannelListview.SelectedIndices.Count == 0) return;
            
            // 現在選択中のチャンネルを取得する
            Channel ch = openedChannelListview.Items[openedChannelListview.SelectedIndices[0]].Tag as Channel;

            // チャンネルを追加する
            (Owner as EbIrcMainForm).AddChannel(memberListView.Items[memberListView.SelectedIndices[0]].Text, false, null);

            // チャンネル一覧の再構築
            LoadChannelList();

            // チャンネルを再選択する
            for (int i = 0; i < openedChannelListview.Items.Count; i++)
            {
                if ((openedChannelListview.Items[i].Tag as Channel) == ch)
                {
                    openedChannelListview.Items[i].Selected = true;
                    break;
                }
            }
        }

        #endregion

        #region リサイズイベント

        /// <summary>
        /// フォームリサイズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelControlDialog_Resize(object sender, EventArgs e)
        {
            if (Width < Height)
            {
                // 縦向き
                openedChannelPanel.Dock = DockStyle.Top;
                openedChannelPanel.Height = (int)(Height / 3);
            }
            else
            {
                // 横向き
                openedChannelPanel.Dock = DockStyle.Left;
                openedChannelPanel.Width = (int)(Width / 2);
            }
            ListBoxImageResize();
        }

        /// <summary>
        /// スプリットサイズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitter_Resize(object sender, EventArgs e)
        {
            ListBoxImageResize();
        }

        /// <summary>
        /// リストボックスのカラムの調整
        /// </summary>
        void ListBoxImageResize()
        {
            memberHeader.Width = memberListView.Width - SCROLLBAR_MARGIN;
            openedChannelHeader.Width = openedChannelListview.Width - SCROLLBAR_MARGIN;
        }

        #endregion

        /// <summary>
        /// チャンネル選択イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openedChannelListview_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMemberList();
        }

        /// <summary>
        /// チャンネル一覧キー押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openedChannelListview_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Left:
                case Keys.Right:
                    memberListView.Focus();
                    break;
                case Keys.Enter:
                    openedChannelContextMenu.Show(openedChannelListview, new Point(0, 0));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// メンバー一覧押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void memberListView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Left:
                case Keys.Right:
                    openedChannelListview.Focus();
                    break;
                case Keys.Enter:
                    memberContextMenu.Show(memberListView, new Point(0, 0));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// チャンネル一覧にフォーカス
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openedChannelListview_GotFocus(object sender, EventArgs e)
        {
            SelectedIndexReset();
        }

        /// <summary>
        /// メンバ一覧にフォーカス
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void memberListView_GotFocus(object sender, EventArgs e)
        {
            SelectedIndexReset();
        }

        /// <summary>
        /// 選択インデックスを初期化
        /// </summary>
        private void SelectedIndexReset()
        {
            if ((openedChannelListview.Items.Count != 0))
            {
                if (openedChannelListview.SelectedIndices.Count == 0)
                {
                    openedChannelListview.Items[0].Selected = true;
                }
                openedChannelListview.Items[openedChannelListview.SelectedIndices[0]].Focused = true;
            }
            if ((memberListView.Items.Count != 0))
            {
                if (memberListView.SelectedIndices.Count == 0)
                {
                    memberListView.Items[0].Selected = true;
                }
                memberListView.Items[memberListView.SelectedIndices[0]].Focused = true;
            }
        }

        /// <summary>
        /// 選択されたチャンネル
        /// </summary>
        internal Channel SelectedChannel
        {
            get
            {
                if (openedChannelListview.SelectedIndices.Count != 0)
                {
                    return openedChannelListview.Items[openedChannelListview.SelectedIndices[0]].Tag as Channel;
                }
                else
                {
                    return null;
                }
            }
        }
	

    }
}