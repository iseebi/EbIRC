namespace EbiSoft.EbIRC
{
    partial class ChannelSettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelSettingForm));
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.closeMenuItem = new System.Windows.Forms.MenuItem();
            this.containerPanel = new System.Windows.Forms.Panel();
            this.channelList = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.removeChannelButton = new System.Windows.Forms.Button();
            this.addChannelButton = new System.Windows.Forms.Button();
            this.disableUnreadSortCheckBox = new System.Windows.Forms.CheckBox();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.channelPasswordTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.inputPanel = new Microsoft.WindowsCE.Forms.InputPanel();
            this.containerPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.Add(this.closeMenuItem);
            // 
            // closeMenuItem
            // 
            resources.ApplyResources(this.closeMenuItem, "closeMenuItem");
            this.closeMenuItem.Click += new System.EventHandler(this.closeMenuItem_Click);
            // 
            // containerPanel
            // 
            this.containerPanel.Controls.Add(this.channelList);
            this.containerPanel.Controls.Add(this.panel2);
            resources.ApplyResources(this.containerPanel, "containerPanel");
            this.containerPanel.Name = "containerPanel";
            // 
            // channelList
            // 
            resources.ApplyResources(this.channelList, "channelList");
            this.channelList.Name = "channelList";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.removeChannelButton);
            this.panel2.Controls.Add(this.addChannelButton);
            this.panel2.Controls.Add(this.disableUnreadSortCheckBox);
            this.panel2.Controls.Add(this.channelNameTextBox);
            this.panel2.Controls.Add(this.channelPasswordTextBox);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // removeChannelButton
            // 
            resources.ApplyResources(this.removeChannelButton, "removeChannelButton");
            this.removeChannelButton.Name = "removeChannelButton";
            // 
            // addChannelButton
            // 
            resources.ApplyResources(this.addChannelButton, "addChannelButton");
            this.addChannelButton.Name = "addChannelButton";
            // 
            // disableUnreadSortCheckBox
            // 
            resources.ApplyResources(this.disableUnreadSortCheckBox, "disableUnreadSortCheckBox");
            this.disableUnreadSortCheckBox.Name = "disableUnreadSortCheckBox";
            // 
            // channelNameTextBox
            // 
            resources.ApplyResources(this.channelNameTextBox, "channelNameTextBox");
            this.channelNameTextBox.Name = "channelNameTextBox";
            // 
            // channelPasswordTextBox
            // 
            resources.ApplyResources(this.channelPasswordTextBox, "channelPasswordTextBox");
            this.channelPasswordTextBox.Name = "channelPasswordTextBox";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ChannelSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.containerPanel);
            this.Menu = this.mainMenu;
            this.MinimizeBox = false;
            this.Name = "ChannelSettingForm";
            this.Resize += new System.EventHandler(this.ChannelSettingForm_Resize);
            this.containerPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem closeMenuItem;
        private System.Windows.Forms.Panel containerPanel;
        private Microsoft.WindowsCE.Forms.InputPanel inputPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button removeChannelButton;
        private System.Windows.Forms.Button addChannelButton;
        private System.Windows.Forms.CheckBox disableUnreadSortCheckBox;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.TextBox channelPasswordTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox channelList;

    }
}