using System;
using System.Collections.Generic;
using System.Text;

namespace EbiSoft.EbIRC.IRC
{
    public class NickNameChangeEventArgs : EventArgs
    {
        private string m_before;
        private string m_after;

        /// <summary>
        /// 変更前の名前
        /// </summary>
        public string Before
        {
            get { return m_before; }
            set { m_before = value; }
        }

        /// <summary>
        /// 変更後の名前
        /// </summary>
        public string After
        {
            get { return m_after; }
            set { m_after = value; }
        }
	
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="before">変更前の名前</param>
        /// <param name="after">変更後の名前</param>
        public NickNameChangeEventArgs(string before, string after)
        {
            m_before = before;
            m_after = after;
        }
    }
    public delegate void NickNameChangeEventHandler(object sender, NickNameChangeEventArgs e);
}
