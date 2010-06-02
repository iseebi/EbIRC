namespace EbiSoft.EbIRC
{
    partial class SettingForm
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.saveCloseMenuItem = new System.Windows.Forms.MenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.serverTabPage = new System.Windows.Forms.TabPage();
            this.communicationTabPage = new System.Windows.Forms.TabPage();
            this.forcePongCheckBox = new System.Windows.Forms.CheckBox();
            this.cacheConnectionCheckBox = new System.Windows.Forms.CheckBox();
            this.nickNameTabPage = new System.Windows.Forms.TabPage();
            this.subNicknameInputBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.viewTabPage = new System.Windows.Forms.TabPage();
            this.defaultLoadOnConnectCheckBox = new System.Windows.Forms.CheckBox();
            this.visibleTopicPanelCheckbox = new System.Windows.Forms.CheckBox();
            this.fontSizeComboBox = new System.Windows.Forms.ComboBox();
            this.fontNameInputBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.controlTabPage = new System.Windows.Forms.TabPage();
            this.scrollLinesTextBox = new System.Windows.Forms.TextBox();
            this.reverseSoftKeyCheckBox = new System.Windows.Forms.CheckBox();
            this.confimExitCheckBox = new System.Windows.Forms.CheckBox();
            this.confimDisconnectCheckBox = new System.Windows.Forms.CheckBox();
            this.ctrlHorizontalKeySelectBox = new System.Windows.Forms.ComboBox();
            this.scrollLinesLabel = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.ctrlVerticalKeySelectBox = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.horizontalKeySelectBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.verticalKeySelectBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.quickSwitchShortcutTabPage = new System.Windows.Forms.TabPage();
            this.qsSortHighlightedCheckBox = new System.Windows.Forms.CheckBox();
            this.qsSortUnreadCheckBox = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.keywordsTabPage = new System.Windows.Forms.TabPage();
            this.highlightChannelCheckBox = new System.Windows.Forms.CheckBox();
            this.highlightMethodComboBox = new System.Windows.Forms.ComboBox();
            this.highlightWordsTextBox = new System.Windows.Forms.TextBox();
            this.highlightUseRegexCheckbox = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.filterTabPage = new System.Windows.Forms.TabPage();
            this.dislikeWordsTextBox = new System.Windows.Forms.TextBox();
            this.dislikeUseRegexCheckBox = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.logTabPage = new System.Windows.Forms.TabPage();
            this.enableLoggingCheckBox = new System.Windows.Forms.CheckBox();
            this.logDirectoryBrowseButton = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.logDirectoryNameTextBox = new System.Windows.Forms.TextBox();
            this.inputPanel = new Microsoft.WindowsCE.Forms.InputPanel(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.profileSelectListview = new System.Windows.Forms.ListView();
            this.profileEditButton = new System.Windows.Forms.Button();
            this.profileMarkActiveButton = new System.Windows.Forms.Button();
            this.profileRemoveButton = new System.Windows.Forms.Button();
            this.profileAddButton = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.serverTabPage.SuspendLayout();
            this.communicationTabPage.SuspendLayout();
            this.nickNameTabPage.SuspendLayout();
            this.viewTabPage.SuspendLayout();
            this.controlTabPage.SuspendLayout();
            this.quickSwitchShortcutTabPage.SuspendLayout();
            this.keywordsTabPage.SuspendLayout();
            this.filterTabPage.SuspendLayout();
            this.logTabPage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.saveCloseMenuItem);
            // 
            // saveCloseMenuItem
            // 
            resources.ApplyResources(this.saveCloseMenuItem, "saveCloseMenuItem");
            this.saveCloseMenuItem.Click += new System.EventHandler(this.saveCloseMenuItem_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.serverTabPage);
            this.tabControl.Controls.Add(this.communicationTabPage);
            this.tabControl.Controls.Add(this.nickNameTabPage);
            this.tabControl.Controls.Add(this.viewTabPage);
            this.tabControl.Controls.Add(this.controlTabPage);
            this.tabControl.Controls.Add(this.quickSwitchShortcutTabPage);
            this.tabControl.Controls.Add(this.keywordsTabPage);
            this.tabControl.Controls.Add(this.filterTabPage);
            this.tabControl.Controls.Add(this.logTabPage);
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // serverTabPage
            // 
            resources.ApplyResources(this.serverTabPage, "serverTabPage");
            this.serverTabPage.Controls.Add(this.panel1);
            this.serverTabPage.Name = "serverTabPage";
            // 
            // communicationTabPage
            // 
            this.communicationTabPage.Controls.Add(this.forcePongCheckBox);
            this.communicationTabPage.Controls.Add(this.cacheConnectionCheckBox);
            resources.ApplyResources(this.communicationTabPage, "communicationTabPage");
            this.communicationTabPage.Name = "communicationTabPage";
            // 
            // forcePongCheckBox
            // 
            resources.ApplyResources(this.forcePongCheckBox, "forcePongCheckBox");
            this.forcePongCheckBox.Name = "forcePongCheckBox";
            // 
            // cacheConnectionCheckBox
            // 
            resources.ApplyResources(this.cacheConnectionCheckBox, "cacheConnectionCheckBox");
            this.cacheConnectionCheckBox.Name = "cacheConnectionCheckBox";
            // 
            // nickNameTabPage
            // 
            this.nickNameTabPage.Controls.Add(this.subNicknameInputBox);
            this.nickNameTabPage.Controls.Add(this.label10);
            resources.ApplyResources(this.nickNameTabPage, "nickNameTabPage");
            this.nickNameTabPage.Name = "nickNameTabPage";
            // 
            // subNicknameInputBox
            // 
            resources.ApplyResources(this.subNicknameInputBox, "subNicknameInputBox");
            this.subNicknameInputBox.Name = "subNicknameInputBox";
            this.subNicknameInputBox.GotFocus += new System.EventHandler(this.multiLineInputbox_GotFocus);
            this.subNicknameInputBox.LostFocus += new System.EventHandler(this.multiLineInputbox_LostFocus);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // viewTabPage
            // 
            resources.ApplyResources(this.viewTabPage, "viewTabPage");
            this.viewTabPage.Controls.Add(this.defaultLoadOnConnectCheckBox);
            this.viewTabPage.Controls.Add(this.visibleTopicPanelCheckbox);
            this.viewTabPage.Controls.Add(this.fontSizeComboBox);
            this.viewTabPage.Controls.Add(this.fontNameInputBox);
            this.viewTabPage.Controls.Add(this.label7);
            this.viewTabPage.Name = "viewTabPage";
            // 
            // defaultLoadOnConnectCheckBox
            // 
            resources.ApplyResources(this.defaultLoadOnConnectCheckBox, "defaultLoadOnConnectCheckBox");
            this.defaultLoadOnConnectCheckBox.Name = "defaultLoadOnConnectCheckBox";
            // 
            // visibleTopicPanelCheckbox
            // 
            resources.ApplyResources(this.visibleTopicPanelCheckbox, "visibleTopicPanelCheckbox");
            this.visibleTopicPanelCheckbox.Name = "visibleTopicPanelCheckbox";
            // 
            // fontSizeComboBox
            // 
            this.fontSizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.fontSizeComboBox.Items.Add(resources.GetString("fontSizeComboBox.Items"));
            this.fontSizeComboBox.Items.Add(resources.GetString("fontSizeComboBox.Items1"));
            this.fontSizeComboBox.Items.Add(resources.GetString("fontSizeComboBox.Items2"));
            this.fontSizeComboBox.Items.Add(resources.GetString("fontSizeComboBox.Items3"));
            this.fontSizeComboBox.Items.Add(resources.GetString("fontSizeComboBox.Items4"));
            this.fontSizeComboBox.Items.Add(resources.GetString("fontSizeComboBox.Items5"));
            this.fontSizeComboBox.Items.Add(resources.GetString("fontSizeComboBox.Items6"));
            this.fontSizeComboBox.Items.Add(resources.GetString("fontSizeComboBox.Items7"));
            this.fontSizeComboBox.Items.Add(resources.GetString("fontSizeComboBox.Items8"));
            this.fontSizeComboBox.Items.Add(resources.GetString("fontSizeComboBox.Items9"));
            resources.ApplyResources(this.fontSizeComboBox, "fontSizeComboBox");
            this.fontSizeComboBox.Name = "fontSizeComboBox";
            // 
            // fontNameInputBox
            // 
            this.fontNameInputBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.fontNameInputBox.Items.Add(resources.GetString("fontNameInputBox.Items"));
            this.fontNameInputBox.Items.Add(resources.GetString("fontNameInputBox.Items1"));
            this.fontNameInputBox.Items.Add(resources.GetString("fontNameInputBox.Items2"));
            this.fontNameInputBox.Items.Add(resources.GetString("fontNameInputBox.Items3"));
            this.fontNameInputBox.Items.Add(resources.GetString("fontNameInputBox.Items4"));
            resources.ApplyResources(this.fontNameInputBox, "fontNameInputBox");
            this.fontNameInputBox.Name = "fontNameInputBox";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // controlTabPage
            // 
            resources.ApplyResources(this.controlTabPage, "controlTabPage");
            this.controlTabPage.Controls.Add(this.scrollLinesTextBox);
            this.controlTabPage.Controls.Add(this.reverseSoftKeyCheckBox);
            this.controlTabPage.Controls.Add(this.confimExitCheckBox);
            this.controlTabPage.Controls.Add(this.confimDisconnectCheckBox);
            this.controlTabPage.Controls.Add(this.ctrlHorizontalKeySelectBox);
            this.controlTabPage.Controls.Add(this.scrollLinesLabel);
            this.controlTabPage.Controls.Add(this.label11);
            this.controlTabPage.Controls.Add(this.ctrlVerticalKeySelectBox);
            this.controlTabPage.Controls.Add(this.label12);
            this.controlTabPage.Controls.Add(this.horizontalKeySelectBox);
            this.controlTabPage.Controls.Add(this.label9);
            this.controlTabPage.Controls.Add(this.verticalKeySelectBox);
            this.controlTabPage.Controls.Add(this.label8);
            this.controlTabPage.Name = "controlTabPage";
            // 
            // scrollLinesTextBox
            // 
            resources.ApplyResources(this.scrollLinesTextBox, "scrollLinesTextBox");
            this.scrollLinesTextBox.Name = "scrollLinesTextBox";
            // 
            // reverseSoftKeyCheckBox
            // 
            resources.ApplyResources(this.reverseSoftKeyCheckBox, "reverseSoftKeyCheckBox");
            this.reverseSoftKeyCheckBox.Name = "reverseSoftKeyCheckBox";
            // 
            // confimExitCheckBox
            // 
            resources.ApplyResources(this.confimExitCheckBox, "confimExitCheckBox");
            this.confimExitCheckBox.Name = "confimExitCheckBox";
            // 
            // confimDisconnectCheckBox
            // 
            resources.ApplyResources(this.confimDisconnectCheckBox, "confimDisconnectCheckBox");
            this.confimDisconnectCheckBox.Name = "confimDisconnectCheckBox";
            // 
            // ctrlHorizontalKeySelectBox
            // 
            this.ctrlHorizontalKeySelectBox.Items.Add(resources.GetString("ctrlHorizontalKeySelectBox.Items"));
            this.ctrlHorizontalKeySelectBox.Items.Add(resources.GetString("ctrlHorizontalKeySelectBox.Items1"));
            this.ctrlHorizontalKeySelectBox.Items.Add(resources.GetString("ctrlHorizontalKeySelectBox.Items2"));
            this.ctrlHorizontalKeySelectBox.Items.Add(resources.GetString("ctrlHorizontalKeySelectBox.Items3"));
            this.ctrlHorizontalKeySelectBox.Items.Add(resources.GetString("ctrlHorizontalKeySelectBox.Items4"));
            resources.ApplyResources(this.ctrlHorizontalKeySelectBox, "ctrlHorizontalKeySelectBox");
            this.ctrlHorizontalKeySelectBox.Name = "ctrlHorizontalKeySelectBox";
            // 
            // scrollLinesLabel
            // 
            resources.ApplyResources(this.scrollLinesLabel, "scrollLinesLabel");
            this.scrollLinesLabel.Name = "scrollLinesLabel";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // ctrlVerticalKeySelectBox
            // 
            this.ctrlVerticalKeySelectBox.Items.Add(resources.GetString("ctrlVerticalKeySelectBox.Items"));
            this.ctrlVerticalKeySelectBox.Items.Add(resources.GetString("ctrlVerticalKeySelectBox.Items1"));
            this.ctrlVerticalKeySelectBox.Items.Add(resources.GetString("ctrlVerticalKeySelectBox.Items2"));
            this.ctrlVerticalKeySelectBox.Items.Add(resources.GetString("ctrlVerticalKeySelectBox.Items3"));
            this.ctrlVerticalKeySelectBox.Items.Add(resources.GetString("ctrlVerticalKeySelectBox.Items4"));
            resources.ApplyResources(this.ctrlVerticalKeySelectBox, "ctrlVerticalKeySelectBox");
            this.ctrlVerticalKeySelectBox.Name = "ctrlVerticalKeySelectBox";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // horizontalKeySelectBox
            // 
            this.horizontalKeySelectBox.Items.Add(resources.GetString("horizontalKeySelectBox.Items"));
            this.horizontalKeySelectBox.Items.Add(resources.GetString("horizontalKeySelectBox.Items1"));
            this.horizontalKeySelectBox.Items.Add(resources.GetString("horizontalKeySelectBox.Items2"));
            this.horizontalKeySelectBox.Items.Add(resources.GetString("horizontalKeySelectBox.Items3"));
            this.horizontalKeySelectBox.Items.Add(resources.GetString("horizontalKeySelectBox.Items4"));
            resources.ApplyResources(this.horizontalKeySelectBox, "horizontalKeySelectBox");
            this.horizontalKeySelectBox.Name = "horizontalKeySelectBox";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // verticalKeySelectBox
            // 
            this.verticalKeySelectBox.Items.Add(resources.GetString("verticalKeySelectBox.Items"));
            this.verticalKeySelectBox.Items.Add(resources.GetString("verticalKeySelectBox.Items1"));
            this.verticalKeySelectBox.Items.Add(resources.GetString("verticalKeySelectBox.Items2"));
            this.verticalKeySelectBox.Items.Add(resources.GetString("verticalKeySelectBox.Items3"));
            this.verticalKeySelectBox.Items.Add(resources.GetString("verticalKeySelectBox.Items4"));
            this.verticalKeySelectBox.Items.Add(resources.GetString("verticalKeySelectBox.Items5"));
            resources.ApplyResources(this.verticalKeySelectBox, "verticalKeySelectBox");
            this.verticalKeySelectBox.Name = "verticalKeySelectBox";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // quickSwitchShortcutTabPage
            // 
            resources.ApplyResources(this.quickSwitchShortcutTabPage, "quickSwitchShortcutTabPage");
            this.quickSwitchShortcutTabPage.Controls.Add(this.qsSortHighlightedCheckBox);
            this.quickSwitchShortcutTabPage.Controls.Add(this.qsSortUnreadCheckBox);
            this.quickSwitchShortcutTabPage.Controls.Add(this.label20);
            this.quickSwitchShortcutTabPage.Controls.Add(this.label19);
            this.quickSwitchShortcutTabPage.Name = "quickSwitchShortcutTabPage";
            // 
            // qsSortHighlightedCheckBox
            // 
            resources.ApplyResources(this.qsSortHighlightedCheckBox, "qsSortHighlightedCheckBox");
            this.qsSortHighlightedCheckBox.Name = "qsSortHighlightedCheckBox";
            // 
            // qsSortUnreadCheckBox
            // 
            resources.ApplyResources(this.qsSortUnreadCheckBox, "qsSortUnreadCheckBox");
            this.qsSortUnreadCheckBox.Name = "qsSortUnreadCheckBox";
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // keywordsTabPage
            // 
            resources.ApplyResources(this.keywordsTabPage, "keywordsTabPage");
            this.keywordsTabPage.Controls.Add(this.highlightChannelCheckBox);
            this.keywordsTabPage.Controls.Add(this.highlightMethodComboBox);
            this.keywordsTabPage.Controls.Add(this.highlightWordsTextBox);
            this.keywordsTabPage.Controls.Add(this.highlightUseRegexCheckbox);
            this.keywordsTabPage.Controls.Add(this.label16);
            this.keywordsTabPage.Controls.Add(this.label15);
            this.keywordsTabPage.Name = "keywordsTabPage";
            // 
            // highlightChannelCheckBox
            // 
            resources.ApplyResources(this.highlightChannelCheckBox, "highlightChannelCheckBox");
            this.highlightChannelCheckBox.Name = "highlightChannelCheckBox";
            // 
            // highlightMethodComboBox
            // 
            this.highlightMethodComboBox.Items.Add(resources.GetString("highlightMethodComboBox.Items"));
            this.highlightMethodComboBox.Items.Add(resources.GetString("highlightMethodComboBox.Items1"));
            this.highlightMethodComboBox.Items.Add(resources.GetString("highlightMethodComboBox.Items2"));
            this.highlightMethodComboBox.Items.Add(resources.GetString("highlightMethodComboBox.Items3"));
            resources.ApplyResources(this.highlightMethodComboBox, "highlightMethodComboBox");
            this.highlightMethodComboBox.Name = "highlightMethodComboBox";
            // 
            // highlightWordsTextBox
            // 
            resources.ApplyResources(this.highlightWordsTextBox, "highlightWordsTextBox");
            this.highlightWordsTextBox.Name = "highlightWordsTextBox";
            // 
            // highlightUseRegexCheckbox
            // 
            resources.ApplyResources(this.highlightUseRegexCheckbox, "highlightUseRegexCheckbox");
            this.highlightUseRegexCheckbox.Name = "highlightUseRegexCheckbox";
            this.highlightUseRegexCheckbox.CheckStateChanged += new System.EventHandler(this.highlightUseRegexCheckbox_CheckStateChanged);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // filterTabPage
            // 
            this.filterTabPage.Controls.Add(this.dislikeWordsTextBox);
            this.filterTabPage.Controls.Add(this.dislikeUseRegexCheckBox);
            this.filterTabPage.Controls.Add(this.label17);
            resources.ApplyResources(this.filterTabPage, "filterTabPage");
            this.filterTabPage.Name = "filterTabPage";
            // 
            // dislikeWordsTextBox
            // 
            resources.ApplyResources(this.dislikeWordsTextBox, "dislikeWordsTextBox");
            this.dislikeWordsTextBox.Name = "dislikeWordsTextBox";
            // 
            // dislikeUseRegexCheckBox
            // 
            resources.ApplyResources(this.dislikeUseRegexCheckBox, "dislikeUseRegexCheckBox");
            this.dislikeUseRegexCheckBox.Name = "dislikeUseRegexCheckBox";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // logTabPage
            // 
            this.logTabPage.Controls.Add(this.enableLoggingCheckBox);
            this.logTabPage.Controls.Add(this.logDirectoryBrowseButton);
            this.logTabPage.Controls.Add(this.label18);
            this.logTabPage.Controls.Add(this.logDirectoryNameTextBox);
            resources.ApplyResources(this.logTabPage, "logTabPage");
            this.logTabPage.Name = "logTabPage";
            // 
            // enableLoggingCheckBox
            // 
            resources.ApplyResources(this.enableLoggingCheckBox, "enableLoggingCheckBox");
            this.enableLoggingCheckBox.Name = "enableLoggingCheckBox";
            // 
            // logDirectoryBrowseButton
            // 
            resources.ApplyResources(this.logDirectoryBrowseButton, "logDirectoryBrowseButton");
            this.logDirectoryBrowseButton.Name = "logDirectoryBrowseButton";
            this.logDirectoryBrowseButton.Click += new System.EventHandler(this.logDirectoryBrowseButton_Click);
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // logDirectoryNameTextBox
            // 
            resources.ApplyResources(this.logDirectoryNameTextBox, "logDirectoryNameTextBox");
            this.logDirectoryNameTextBox.Name = "logDirectoryNameTextBox";
            // 
            // inputPanel
            // 
            this.inputPanel.EnabledChanged += new System.EventHandler(this.inputPanel_EnabledChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.profileSelectListview);
            this.panel1.Controls.Add(this.profileEditButton);
            this.panel1.Controls.Add(this.profileMarkActiveButton);
            this.panel1.Controls.Add(this.profileRemoveButton);
            this.panel1.Controls.Add(this.profileAddButton);
            this.panel1.Controls.Add(this.label13);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // profileSelectListview
            // 
            resources.ApplyResources(this.profileSelectListview, "profileSelectListview");
            listViewItem2.Text = resources.GetString("profileSelectListview.Items");
            this.profileSelectListview.Items.Add(listViewItem2);
            this.profileSelectListview.Name = "profileSelectListview";
            this.profileSelectListview.View = System.Windows.Forms.View.List;
            this.profileSelectListview.SelectedIndexChanged += new System.EventHandler(this.profileSelectListview_SelectedIndexChanged);
            // 
            // profileEditButton
            // 
            resources.ApplyResources(this.profileEditButton, "profileEditButton");
            this.profileEditButton.Name = "profileEditButton";
            this.profileEditButton.Click += new System.EventHandler(this.profileEditButton_Click);
            // 
            // profileMarkActiveButton
            // 
            resources.ApplyResources(this.profileMarkActiveButton, "profileMarkActiveButton");
            this.profileMarkActiveButton.Name = "profileMarkActiveButton";
            this.profileMarkActiveButton.Click += new System.EventHandler(this.profileMarkActiveButton_Click);
            // 
            // profileRemoveButton
            // 
            resources.ApplyResources(this.profileRemoveButton, "profileRemoveButton");
            this.profileRemoveButton.Name = "profileRemoveButton";
            this.profileRemoveButton.Click += new System.EventHandler(this.profileRemoveButton_Click);
            // 
            // profileAddButton
            // 
            resources.ApplyResources(this.profileAddButton, "profileAddButton");
            this.profileAddButton.Name = "profileAddButton";
            this.profileAddButton.Click += new System.EventHandler(this.profileAddButton_Click);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tabControl);
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.Name = "SettingForm";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.SettingForm_Closing);
            this.Resize += new System.EventHandler(this.SettingForm_Resize);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SettingForm_KeyDown);
            this.tabControl.ResumeLayout(false);
            this.serverTabPage.ResumeLayout(false);
            this.communicationTabPage.ResumeLayout(false);
            this.nickNameTabPage.ResumeLayout(false);
            this.viewTabPage.ResumeLayout(false);
            this.controlTabPage.ResumeLayout(false);
            this.quickSwitchShortcutTabPage.ResumeLayout(false);
            this.keywordsTabPage.ResumeLayout(false);
            this.filterTabPage.ResumeLayout(false);
            this.logTabPage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage serverTabPage;
        private System.Windows.Forms.TabPage viewTabPage;
        private System.Windows.Forms.ComboBox fontSizeComboBox;
        private System.Windows.Forms.ComboBox fontNameInputBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox visibleTopicPanelCheckbox;
        private System.Windows.Forms.TabPage controlTabPage;
        private System.Windows.Forms.ComboBox verticalKeySelectBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox horizontalKeySelectBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabPage nickNameTabPage;
        private System.Windows.Forms.TextBox subNicknameInputBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox ctrlHorizontalKeySelectBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox ctrlVerticalKeySelectBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox confimDisconnectCheckBox;
        private System.Windows.Forms.CheckBox confimExitCheckBox;
        private System.Windows.Forms.CheckBox reverseSoftKeyCheckBox;
        private System.Windows.Forms.TextBox scrollLinesTextBox;
        private System.Windows.Forms.Label scrollLinesLabel;
        private System.Windows.Forms.TabPage communicationTabPage;
        private System.Windows.Forms.CheckBox forcePongCheckBox;
        private System.Windows.Forms.CheckBox cacheConnectionCheckBox;
        private System.Windows.Forms.MenuItem saveCloseMenuItem;
        private System.Windows.Forms.CheckBox defaultLoadOnConnectCheckBox;
        private System.Windows.Forms.TabPage keywordsTabPage;
        private System.Windows.Forms.TextBox highlightWordsTextBox;
        private System.Windows.Forms.CheckBox highlightUseRegexCheckbox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox highlightChannelCheckBox;
        private System.Windows.Forms.ComboBox highlightMethodComboBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TabPage logTabPage;
        private System.Windows.Forms.Button logDirectoryBrowseButton;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox logDirectoryNameTextBox;
        private System.Windows.Forms.CheckBox enableLoggingCheckBox;
        private System.Windows.Forms.TabPage quickSwitchShortcutTabPage;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox qsSortHighlightedCheckBox;
        private System.Windows.Forms.CheckBox qsSortUnreadCheckBox;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TabPage filterTabPage;
        private System.Windows.Forms.TextBox dislikeWordsTextBox;
        private System.Windows.Forms.CheckBox dislikeUseRegexCheckBox;
        private System.Windows.Forms.Label label17;
        private Microsoft.WindowsCE.Forms.InputPanel inputPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView profileSelectListview;
        private System.Windows.Forms.Button profileEditButton;
        private System.Windows.Forms.Button profileMarkActiveButton;
        private System.Windows.Forms.Button profileRemoveButton;
        private System.Windows.Forms.Button profileAddButton;
        private System.Windows.Forms.Label label13;
    }
}