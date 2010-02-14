using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EbiSoft.EbIRC
{
    public partial class ChannelSettingForm : Form
    {
        public ChannelSettingForm()
        {
            InitializeComponent();
        }

        private void ChannelSettingForm_Resize(object sender, EventArgs e)
        {
            OnResizeAction();
        }

        void OnResizeAction()
        {
            if (inputPanel.Enabled)
            {
                containerPanel.Height = this.ClientSize.Height - inputPanel.Bounds.Height;
            }
            else
            {
                containerPanel.Height = this.ClientSize.Height;
            }
        }

        private void closeMenuItem_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}