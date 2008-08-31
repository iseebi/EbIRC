namespace EbiSoft.EbIRC
{
    partial class InputBoxForm
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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.okMenuItem = new System.Windows.Forms.MenuItem();
            this.CancelMenuItem = new System.Windows.Forms.MenuItem();
            this.label = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.okMenuItem);
            this.mainMenu1.MenuItems.Add(this.CancelMenuItem);
            // 
            // okMenuItem
            // 
            this.okMenuItem.Text = "OK";
            this.okMenuItem.Click += new System.EventHandler(this.okMenuItem1_Click);
            // 
            // CancelMenuItem
            // 
            this.CancelMenuItem.Text = "Cancel";
            this.CancelMenuItem.Click += new System.EventHandler(this.cancelMenuItem_Click);
            // 
            // label
            // 
            this.label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label.Location = new System.Drawing.Point(3, 9);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(474, 91);
            this.label.Text = "label1";
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(6, 103);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(474, 41);
            this.textBox.TabIndex = 1;
            // 
            // InputBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(480, 536);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.label);
            this.Location = new System.Drawing.Point(0, 52);
            this.Menu = this.mainMenu1;
            this.Name = "InputBoxForm";
            this.Text = "InputBox";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem okMenuItem;
        private System.Windows.Forms.MenuItem CancelMenuItem;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.TextBox textBox;
    }
}