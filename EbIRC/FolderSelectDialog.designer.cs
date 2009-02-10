namespace EbiSoft.Library.Mobile
{
    partial class FolderSelectDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FolderSelectDialog));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.cancelMenuItem = new System.Windows.Forms.MenuItem();
            this.acceptMenuItem = new System.Windows.Forms.MenuItem();
            this.folderTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.cancelMenuItem);
            this.mainMenu1.MenuItems.Add(this.acceptMenuItem);
            // 
            // cancelMenuItem
            // 
            resources.ApplyResources(this.cancelMenuItem, "cancelMenuItem");
            this.cancelMenuItem.Click += new System.EventHandler(this.cancelMenuItem_Click);
            // 
            // acceptMenuItem
            // 
            resources.ApplyResources(this.acceptMenuItem, "acceptMenuItem");
            this.acceptMenuItem.Click += new System.EventHandler(this.acceptMenuItem_Click);
            // 
            // folderTreeView
            // 
            resources.ApplyResources(this.folderTreeView, "folderTreeView");
            this.folderTreeView.Name = "folderTreeView";
            this.folderTreeView.PathSeparator = "\\";
            this.folderTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.folderTreeView_BeforeExpand);
            // 
            // FolderSelectDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.folderTreeView);
            this.Menu = this.mainMenu1;
            this.Name = "FolderSelectDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem cancelMenuItem;
        private System.Windows.Forms.MenuItem acceptMenuItem;
        private System.Windows.Forms.TreeView folderTreeView;
    }
}