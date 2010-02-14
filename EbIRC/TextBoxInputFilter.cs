using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace EbiSoft.EbIRC
{
    /// <summary>
    /// テキストボックス入力フィルタの基底クラス
    /// </summary>
    abstract class TextBoxInputFilter : IDisposable
    {
        #region P/Invoke 宣言

        [DllImport("coredll.dll")]
        private extern static IntPtr SetWindowLong(IntPtr hwnd, int nIndex, IntPtr dwNewLong);
        private const int GWL_WNDPROC = -4;

        [DllImport("coredll.dll")]
        private extern static int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, uint msg, uint wParam, int lParam);

        #endregion

        /// <summary>
        /// サブクラス化するウィンドウプロシージャのデリゲート
        /// </summary>
        private delegate int WndProcDelegate(IntPtr hwnd, uint msg, uint wParam, int lParam);

        private bool disposed = false;    // Dispose が呼ばれたか
        private IntPtr oldWndProc;        // 前のハンドル
        private IntPtr targetHandle;      // テキストボックスのハンドル
        private WndProcDelegate wndProc;
        private IntPtr wndProcPtr;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetTextBox">処理対象のテキストボックス</param>
        public TextBoxInputFilter(TextBox targetTextBox)
        {
            targetHandle = targetTextBox.Handle;
            wndProc = new WndProcDelegate(WndProc);
            wndProcPtr = Marshal.GetFunctionPointerForDelegate(wndProc);

            // サブクラス化する
            oldWndProc = SetWindowLong(targetHandle, GWL_WNDPROC, wndProcPtr);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                // サブクラス化解除する
                SetWindowLong(targetHandle, GWL_WNDPROC, oldWndProc);
                disposed = true;
            }
        }

        /// <summary>
        /// ウィンドウプロシージャ
        /// </summary>
        protected virtual int WndProc(IntPtr hwnd, uint msg, uint wParam, int lParam)
        {
            // デフォルトのプロシージャへ
            return CallWindowProc(oldWndProc, hwnd, msg, wParam, lParam);
        }

        #region プロパティ

        /// <summary>
        /// 処理対象になっているテキストボックス
        /// </summary>
        public TextBox TextBox
        {
            get { return m_textBox; }
        }
        private TextBox m_textBox = null;

        #endregion

    }
}
