namespace EbiSoft.EbIRC
{
    partial class ChannelControlDialog
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelControlDialog));
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.closeMenuItem = new System.Windows.Forms.MenuItem();
            this.addSenderMenuItem = new System.Windows.Forms.MenuItem();
            this.openedChannelPanel = new System.Windows.Forms.Panel();
            this.openedChannelListview = new System.Windows.Forms.ListView();
            this.openedChannelHeader = new System.Windows.Forms.ColumnHeader();
            this.openedChannelContextMenu = new System.Windows.Forms.ContextMenu();
            this.joinMenuItem = new System.Windows.Forms.MenuItem();
            this.leaveMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.removeChannelMenuItem = new System.Windows.Forms.MenuItem();
            this.imageList = new System.Windows.Forms.ImageList();
            this.channelLabel = new System.Windows.Forms.Label();
            this.memberPanel = new System.Windows.Forms.Panel();
            this.memberListView = new System.Windows.Forms.ListView();
            this.memberHeader = new System.Windows.Forms.ColumnHeader();
            this.memberContextMenu = new System.Windows.Forms.ContextMenu();
            this.addPMMemberMenuItem = new System.Windows.Forms.MenuItem();
            this.memberLabel = new System.Windows.Forms.Label();
            this.openedChannelPanel.SuspendLayout();
            this.memberPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.Add(this.closeMenuItem);
            this.mainMenu.MenuItems.Add(this.addSenderMenuItem);
            // 
            // closeMenuItem
            // 
            resources.ApplyResources(this.closeMenuItem, "closeMenuItem");
            this.closeMenuItem.Click += new System.EventHandler(this.closeMenuItem_Click);
            // 
            // addSenderMenuItem
            // 
            resources.ApplyResources(this.addSenderMenuItem, "addSenderMenuItem");
            this.addSenderMenuItem.Click += new System.EventHandler(this.addSenderMenuItem_Click);
            // 
            // openedChannelPanel
            // 
            resources.ApplyResources(this.openedChannelPanel, "openedChannelPanel");
            this.openedChannelPanel.Controls.Add(this.openedChannelListview);
            this.openedChannelPanel.Controls.Add(this.channelLabel);
            this.openedChannelPanel.Name = "openedChannelPanel";
            // 
            // openedChannelListview
            // 
            resources.ApplyResources(this.openedChannelListview, "openedChannelListview");
            this.openedChannelListview.Columns.Add(this.openedChannelHeader);
            this.openedChannelListview.ContextMenu = this.openedChannelContextMenu;
            this.openedChannelListview.FullRowSelect = true;
            this.openedChannelListview.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.openedChannelListview.Name = "openedChannelListview";
            this.openedChannelListview.SmallImageList = this.imageList;
            this.openedChannelListview.View = System.Windows.Forms.View.Details;
            this.openedChannelListview.SelectedIndexChanged += new System.EventHandler(this.openedChannelListview_SelectedIndexChanged);
            this.openedChannelListview.GotFocus += new System.EventHandler(this.openedChannelListview_GotFocus);
            this.openedChannelListview.KeyDown += new System.Windows.Forms.KeyEventHandler(this.openedChannelListview_KeyDown);
            // 
            // openedChannelHeader
            // 
            resources.ApplyResources(this.openedChannelHeader, "openedChannelHeader");
            // 
            // openedChannelContextMenu
            // 
            this.openedChannelContextMenu.MenuItems.Add(this.joinMenuItem);
            this.openedChannelContextMenu.MenuItems.Add(this.leaveMenuItem);
            this.openedChannelContextMenu.MenuItems.Add(this.menuItem3);
            this.openedChannelContextMenu.MenuItems.Add(this.removeChannelMenuItem);
            this.openedChannelContextMenu.Popup += new System.EventHandler(this.openedChannelContextMenu_Popup);
            // 
            // joinMenuItem
            // 
            resources.ApplyResources(this.joinMenuItem, "joinMenuItem");
            this.joinMenuItem.Click += new System.EventHandler(this.joinMenuItem_Click);
            // 
            // leaveMenuItem
            // 
            resources.ApplyResources(this.leaveMenuItem, "leaveMenuItem");
            this.leaveMenuItem.Click += new System.EventHandler(this.leaveMenuItem_Click);
            // 
            // menuItem3
            // 
            resources.ApplyResources(this.menuItem3, "menuItem3");
            // 
            // removeChannelMenuItem
            // 
            resources.ApplyResources(this.removeChannelMenuItem, "removeChannelMenuItem");
            this.removeChannelMenuItem.Click += new System.EventHandler(this.removeChannelMenuItem_Click);
            this.imageList.Images.Clear();
            this.imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource"))));
            this.imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource1"))));
            this.imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource2"))));
            this.imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource3"))));
            // 
            // channelLabel
            // 
            resources.ApplyResources(this.channelLabel, "channelLabel");
            this.channelLabel.Name = "channelLabel";
            // 
            // memberPanel
            // 
            resources.ApplyResources(this.memberPanel, "memberPanel");
            this.memberPanel.Controls.Add(this.memberListView);
            this.memberPanel.Controls.Add(this.memberLabel);
            this.memberPanel.Name = "memberPanel";
            // 
            // memberListView
            // 
            resources.ApplyResources(this.memberListView, "memberListView");
            this.memberListView.Columns.Add(this.memberHeader);
            this.memberListView.ContextMenu = this.memberContextMenu;
            this.memberListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.memberListView.Name = "memberListView";
            this.memberListView.SmallImageList = this.imageList;
            this.memberListView.View = System.Windows.Forms.View.Details;
            this.memberListView.GotFocus += new System.EventHandler(this.memberListView_GotFocus);
            this.memberListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.memberListView_KeyDown);
            // 
            // memberHeader
            // 
            resources.ApplyResources(this.memberHeader, "memberHeader");
            // 
            // memberContextMenu
            // 
            this.memberContextMenu.MenuItems.Add(this.addPMMemberMenuItem);
            this.memberContextMenu.Popup += new System.EventHandler(this.memberContextMenu_Popup);
            // 
            // addPMMemberMenuItem
            // 
            resources.ApplyResources(this.addPMMemberMenuItem, "addPMMemberMenuItem");
            this.addPMMemberMenuItem.Click += new System.EventHandler(this.addPMMemberMenuItem_Click);
            // 
            // memberLabel
            // 
            resources.ApplyResources(this.memberLabel, "memberLabel");
            this.memberLabel.Name = "memberLabel";
            // 
            // ChannelControlDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.memberPanel);
            this.Controls.Add(this.openedChannelPanel);
            this.Menu = this.mainMenu;
            this.Name = "ChannelControlDialog";
            this.Load += new System.EventHandler(this.ChannelControlDialog_Load);
            this.Resize += new System.EventHandler(this.ChannelControlDialog_Resize);
            this.openedChannelPanel.ResumeLayout(false);
            this.memberPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel openedChannelPanel;
        private System.Windows.Forms.Label channelLabel;
        private System.Windows.Forms.Panel memberPanel;
        private System.Windows.Forms.Label memberLabel;
        private System.Windows.Forms.ListView openedChannelListview;
        private System.Windows.Forms.ListView memberListView;
        private System.Windows.Forms.MenuItem closeMenuItem;
        private System.Windows.Forms.MenuItem addSenderMenuItem;
        private System.Windows.Forms.ColumnHeader openedChannelHeader;
        private System.Windows.Forms.ColumnHeader memberHeader;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenu openedChannelContextMenu;
        private System.Windows.Forms.MenuItem joinMenuItem;
        private System.Windows.Forms.MenuItem leaveMenuItem;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem removeChannelMenuItem;
        private System.Windows.Forms.ContextMenu memberContextMenu;
        private System.Windows.Forms.MenuItem addPMMemberMenuItem;
    }
}