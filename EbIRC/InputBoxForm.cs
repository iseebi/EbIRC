using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EbiSoft.EbIRC
{
    /// <summary>
    /// テキストの入力を行う画面
    /// </summary>
    public partial class InputBoxForm : Form
    {
        public InputBoxForm()
        {
            InitializeComponent();
        }

        private void okMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 入力されたテキスト
        /// </summary>
        public string Value
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }

        /// <summary>
        /// 入力項目の説明
        /// </summary>
        public string Description
        {
            get { return label.Text; }
            set { label.Text = value; }
        }
    }
}