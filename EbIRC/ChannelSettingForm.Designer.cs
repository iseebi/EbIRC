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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.basicsTabPage = new System.Windows.Forms.TabPage();
            this.serverSslNotValidateCheckBox = new System.Windows.Forms.CheckBox();
            this.serverUseSslCheckBox = new System.Windows.Forms.CheckBox();
            this.encodingSelectBox = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.portInputBox = new System.Windows.Forms.TextBox();
            this.nicknameInputbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.serverInputbox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.advancedTabPage = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.passwordInputBox = new System.Windows.Forms.TextBox();
            this.nameInputbox = new System.Windows.Forms.TextBox();
            this.channelsTabPage = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.channelList = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.removeChannelButton = new System.Windows.Forms.Button();
            this.addChannelButton = new System.Windows.Forms.Button();
            this.disableUnreadSortCheckBox = new System.Windows.Forms.CheckBox();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.channelPasswordTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.inputPanel = new Microsoft.WindowsCE.Forms.InputPanel(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.containerPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
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
            this.containerPanel.Controls.Add(this.tabControl1);
            resources.ApplyResources(this.containerPanel, "containerPanel");
            this.containerPanel.Name = "containerPanel";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.basicsTabPage);
            this.tabControl1.Controls.Add(this.advancedTabPage);
            this.tabControl1.Controls.Add(this.channelsTabPage);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // basicsTabPage
            // 
            resources.ApplyResources(this.basicsTabPage, "basicsTabPage");
            this.basicsTabPage.Controls.Add(this.textBox3);
            this.basicsTabPage.Controls.Add(this.label10);
            this.basicsTabPage.Controls.Add(this.serverSslNotValidateCheckBox);
            this.basicsTabPage.Controls.Add(this.serverUseSslCheckBox);
            this.basicsTabPage.Controls.Add(this.encodingSelectBox);
            this.basicsTabPage.Controls.Add(this.label14);
            this.basicsTabPage.Controls.Add(this.portInputBox);
            this.basicsTabPage.Controls.Add(this.nicknameInputbox);
            this.basicsTabPage.Controls.Add(this.label3);
            this.basicsTabPage.Controls.Add(this.label5);
            this.basicsTabPage.Controls.Add(this.serverInputbox);
            this.basicsTabPage.Controls.Add(this.label7);
            this.basicsTabPage.Name = "basicsTabPage";
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
            // 
            // encodingSelectBox
            // 
            this.encodingSelectBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            resources.ApplyResources(this.encodingSelectBox, "encodingSelectBox");
            this.encodingSelectBox.Name = "encodingSelectBox";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // portInputBox
            // 
            resources.ApplyResources(this.portInputBox, "portInputBox");
            this.portInputBox.Name = "portInputBox";
            // 
            // nicknameInputbox
            // 
            resources.ApplyResources(this.nicknameInputbox, "nicknameInputbox");
            this.nicknameInputbox.Name = "nicknameInputbox";
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
            // serverInputbox
            // 
            resources.ApplyResources(this.serverInputbox, "serverInputbox");
            this.serverInputbox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.serverInputbox.Items.Add(resources.GetString("serverInputbox.Items"));
            this.serverInputbox.Items.Add(resources.GetString("serverInputbox.Items1"));
            this.serverInputbox.Items.Add(resources.GetString("serverInputbox.Items2"));
            this.serverInputbox.Items.Add(resources.GetString("serverInputbox.Items3"));
            this.serverInputbox.Items.Add(resources.GetString("serverInputbox.Items4"));
            this.serverInputbox.Name = "serverInputbox";
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
            this.advancedTabPage.Controls.Add(this.textBox2);
            this.advancedTabPage.Controls.Add(this.label8);
            this.advancedTabPage.Controls.Add(this.textBox1);
            this.advancedTabPage.Controls.Add(this.label6);
            this.advancedTabPage.Controls.Add(this.label4);
            this.advancedTabPage.Controls.Add(this.passwordInputBox);
            this.advancedTabPage.Controls.Add(this.nameInputbox);
            this.advancedTabPage.Name = "advancedTabPage";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
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
            // passwordInputBox
            // 
            resources.ApplyResources(this.passwordInputBox, "passwordInputBox");
            this.passwordInputBox.Name = "passwordInputBox";
            // 
            // nameInputbox
            // 
            resources.ApplyResources(this.nameInputbox, "nameInputbox");
            this.nameInputbox.Name = "nameInputbox";
            // 
            // channelsTabPage
            // 
            this.channelsTabPage.Controls.Add(this.panel1);
            this.channelsTabPage.Controls.Add(this.panel2);
            resources.ApplyResources(this.channelsTabPage, "channelsTabPage");
            this.channelsTabPage.Name = "channelsTabPage";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.channelList);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // channelList
            // 
            resources.ApplyResources(this.channelList, "channelList");
            this.channelList.Name = "channelList";
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
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
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // textBox2
            // 
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.Name = "textBox2";
            // 
            // textBox3
            // 
            resources.ApplyResources(this.textBox3, "textBox3");
            this.textBox3.Name = "textBox3";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
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
            this.Resize += new System.EventHandler(this.ChannelSettingForm_Resize);
            this.containerPanel.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
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
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage basicsTabPage;
        private System.Windows.Forms.TabPage channelsTabPage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox channelList;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button removeChannelButton;
        private System.Windows.Forms.Button addChannelButton;
        private System.Windows.Forms.CheckBox disableUnreadSortCheckBox;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.TextBox channelPasswordTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox serverSslNotValidateCheckBox;
        private System.Windows.Forms.CheckBox serverUseSslCheckBox;
        private System.Windows.Forms.ComboBox encodingSelectBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox portInputBox;
        private System.Windows.Forms.TextBox nicknameInputbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox serverInputbox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage advancedTabPage;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox passwordInputBox;
        private System.Windows.Forms.TextBox nameInputbox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label10;

    }
}