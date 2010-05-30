namespace EbiSoft.EbIRC
{
    partial class ServerSettingForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerSettingForm));
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.closeMenuItem = new System.Windows.Forms.MenuItem();
            this.containerPanel = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.basicsTabPage = new System.Windows.Forms.TabPage();
            this.profileNameTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.serverSslNotValidateCheckBox = new System.Windows.Forms.CheckBox();
            this.serverUseSslCheckBox = new System.Windows.Forms.CheckBox();
            this.encodingComboBox = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.nicknameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.serverComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.advancedTabPage = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.nickservPasswordTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.loginNameTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.serverPasswordTextBox = new System.Windows.Forms.TextBox();
            this.realnameTextBox = new System.Windows.Forms.TextBox();
            this.channelsTabPage = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.channelListView = new System.Windows.Forms.ListView();
            this.channelDownButton = new System.Windows.Forms.Button();
            this.channelUpButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.removeChannelButton = new System.Windows.Forms.Button();
            this.addChannelButton = new System.Windows.Forms.Button();
            this.channelIgnoreUnreadSortCheckBox = new System.Windows.Forms.CheckBox();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.channelPasswordTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.inputPanel = new Microsoft.WindowsCE.Forms.InputPanel(this.components);
            this.containerPanel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.basicsTabPage.SuspendLayout();
            this.advancedTabPage.SuspendLayout();
            this.channelsTabPage.SuspendLayout();
            this.panel1.SuspendLayout();
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
            resources.ApplyResources(this.containerPanel, "containerPanel");
            this.containerPanel.Controls.Add(this.tabControl);
            this.containerPanel.Name = "containerPanel";
            // 
            // tabControl
            // 
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Controls.Add(this.basicsTabPage);
            this.tabControl.Controls.Add(this.advancedTabPage);
            this.tabControl.Controls.Add(this.channelsTabPage);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // basicsTabPage
            // 
            resources.ApplyResources(this.basicsTabPage, "basicsTabPage");
            this.basicsTabPage.Controls.Add(this.profileNameTextBox);
            this.basicsTabPage.Controls.Add(this.label10);
            this.basicsTabPage.Controls.Add(this.serverSslNotValidateCheckBox);
            this.basicsTabPage.Controls.Add(this.serverUseSslCheckBox);
            this.basicsTabPage.Controls.Add(this.encodingComboBox);
            this.basicsTabPage.Controls.Add(this.label14);
            this.basicsTabPage.Controls.Add(this.portTextBox);
            this.basicsTabPage.Controls.Add(this.nicknameTextBox);
            this.basicsTabPage.Controls.Add(this.label3);
            this.basicsTabPage.Controls.Add(this.label5);
            this.basicsTabPage.Controls.Add(this.serverComboBox);
            this.basicsTabPage.Controls.Add(this.label7);
            this.basicsTabPage.Name = "basicsTabPage";
            // 
            // profileNameTextBox
            // 
            resources.ApplyResources(this.profileNameTextBox, "profileNameTextBox");
            this.profileNameTextBox.Name = "profileNameTextBox";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // serverSslNotValidateCheckBox
            // 
            resources.ApplyResources(this.serverSslNotValidateCheckBox, "serverSslNotValidateCheckBox");
            this.serverSslNotValidateCheckBox.Name = "serverSslNotValidateCheckBox";
            // 
            // serverUseSslCheckBox
            // 
            resources.ApplyResources(this.serverUseSslCheckBox, "serverUseSslCheckBox");
            this.serverUseSslCheckBox.Name = "serverUseSslCheckBox";
            this.serverUseSslCheckBox.CheckStateChanged += new System.EventHandler(this.serverUseSslCheckBox_CheckStateChanged);
            // 
            // encodingComboBox
            // 
            resources.ApplyResources(this.encodingComboBox, "encodingComboBox");
            this.encodingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.encodingComboBox.Name = "encodingComboBox";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // portTextBox
            // 
            resources.ApplyResources(this.portTextBox, "portTextBox");
            this.portTextBox.Name = "portTextBox";
            // 
            // nicknameTextBox
            // 
            resources.ApplyResources(this.nicknameTextBox, "nicknameTextBox");
            this.nicknameTextBox.Name = "nicknameTextBox";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // serverComboBox
            // 
            resources.ApplyResources(this.serverComboBox, "serverComboBox");
            this.serverComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.serverComboBox.Items.Add(resources.GetString("serverComboBox.Items"));
            this.serverComboBox.Items.Add(resources.GetString("serverComboBox.Items1"));
            this.serverComboBox.Items.Add(resources.GetString("serverComboBox.Items2"));
            this.serverComboBox.Items.Add(resources.GetString("serverComboBox.Items3"));
            this.serverComboBox.Items.Add(resources.GetString("serverComboBox.Items4"));
            this.serverComboBox.Name = "serverComboBox";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // advancedTabPage
            // 
            resources.ApplyResources(this.advancedTabPage, "advancedTabPage");
            this.advancedTabPage.Controls.Add(this.label9);
            this.advancedTabPage.Controls.Add(this.nickservPasswordTextBox);
            this.advancedTabPage.Controls.Add(this.label8);
            this.advancedTabPage.Controls.Add(this.loginNameTextBox);
            this.advancedTabPage.Controls.Add(this.label6);
            this.advancedTabPage.Controls.Add(this.label4);
            this.advancedTabPage.Controls.Add(this.serverPasswordTextBox);
            this.advancedTabPage.Controls.Add(this.realnameTextBox);
            this.advancedTabPage.Name = "advancedTabPage";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // nickservPasswordTextBox
            // 
            resources.ApplyResources(this.nickservPasswordTextBox, "nickservPasswordTextBox");
            this.nickservPasswordTextBox.Name = "nickservPasswordTextBox";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // loginNameTextBox
            // 
            resources.ApplyResources(this.loginNameTextBox, "loginNameTextBox");
            this.loginNameTextBox.Name = "loginNameTextBox";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // serverPasswordTextBox
            // 
            resources.ApplyResources(this.serverPasswordTextBox, "serverPasswordTextBox");
            this.serverPasswordTextBox.Name = "serverPasswordTextBox";
            // 
            // realnameTextBox
            // 
            resources.ApplyResources(this.realnameTextBox, "realnameTextBox");
            this.realnameTextBox.Name = "realnameTextBox";
            // 
            // channelsTabPage
            // 
            resources.ApplyResources(this.channelsTabPage, "channelsTabPage");
            this.channelsTabPage.Controls.Add(this.panel1);
            this.channelsTabPage.Controls.Add(this.panel2);
            this.channelsTabPage.Name = "channelsTabPage";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.channelListView);
            this.panel1.Controls.Add(this.channelDownButton);
            this.panel1.Controls.Add(this.channelUpButton);
            this.panel1.Name = "panel1";
            // 
            // channelListView
            // 
            resources.ApplyResources(this.channelListView, "channelListView");
            this.channelListView.Name = "channelListView";
            this.channelListView.View = System.Windows.Forms.View.List;
            this.channelListView.SelectedIndexChanged += new System.EventHandler(this.channelListView_SelectedIndexChanged);
            // 
            // channelDownButton
            // 
            resources.ApplyResources(this.channelDownButton, "channelDownButton");
            this.channelDownButton.Name = "channelDownButton";
            this.channelDownButton.Click += new System.EventHandler(this.channelDownButton_Click);
            // 
            // channelUpButton
            // 
            resources.ApplyResources(this.channelUpButton, "channelUpButton");
            this.channelUpButton.Name = "channelUpButton";
            this.channelUpButton.Click += new System.EventHandler(this.channelUpButton_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.removeChannelButton);
            this.panel2.Controls.Add(this.addChannelButton);
            this.panel2.Controls.Add(this.channelIgnoreUnreadSortCheckBox);
            this.panel2.Controls.Add(this.channelNameTextBox);
            this.panel2.Controls.Add(this.channelPasswordTextBox);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Name = "panel2";
            // 
            // removeChannelButton
            // 
            resources.ApplyResources(this.removeChannelButton, "removeChannelButton");
            this.removeChannelButton.Name = "removeChannelButton";
            this.removeChannelButton.Click += new System.EventHandler(this.removeChannelButton_Click);
            // 
            // addChannelButton
            // 
            resources.ApplyResources(this.addChannelButton, "addChannelButton");
            this.addChannelButton.Name = "addChannelButton";
            this.addChannelButton.Click += new System.EventHandler(this.addChannelButton_Click);
            // 
            // channelIgnoreUnreadSortCheckBox
            // 
            resources.ApplyResources(this.channelIgnoreUnreadSortCheckBox, "channelIgnoreUnreadSortCheckBox");
            this.channelIgnoreUnreadSortCheckBox.Name = "channelIgnoreUnreadSortCheckBox";
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
            // ServerSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.containerPanel);
            this.Menu = this.mainMenu;
            this.MinimizeBox = false;
            this.Name = "ServerSettingForm";
            this.Load += new System.EventHandler(this.ServerSettingForm_Load);
            this.Resize += new System.EventHandler(this.ChannelSettingForm_Resize);
            this.containerPanel.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.basicsTabPage.ResumeLayout(false);
            this.advancedTabPage.ResumeLayout(false);
            this.channelsTabPage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem closeMenuItem;
        private System.Windows.Forms.Panel containerPanel;
        private Microsoft.WindowsCE.Forms.InputPanel inputPanel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage basicsTabPage;
        private System.Windows.Forms.TabPage channelsTabPage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button channelDownButton;
        private System.Windows.Forms.Button channelUpButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button removeChannelButton;
        private System.Windows.Forms.Button addChannelButton;
        private System.Windows.Forms.CheckBox channelIgnoreUnreadSortCheckBox;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.TextBox channelPasswordTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox serverSslNotValidateCheckBox;
        private System.Windows.Forms.CheckBox serverUseSslCheckBox;
        private System.Windows.Forms.ComboBox encodingComboBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.TextBox nicknameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox serverComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage advancedTabPage;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox loginNameTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox serverPasswordTextBox;
        private System.Windows.Forms.TextBox realnameTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox nickservPasswordTextBox;
        private System.Windows.Forms.TextBox profileNameTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ListView channelListView;

    }
}