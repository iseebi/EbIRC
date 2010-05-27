using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace EbiSoft.EbIRC
{
    /// <summary>
    /// 入力テキストボックスのIME入力状態を監視するクラス
    /// </summary>
    class InputBoxInputFilter : TextBoxInputFilter
    {
        #region P/Invoke 宣言

        [DllImport("coredll.dll")]
        public static extern IntPtr FindWindow(string className, string WindowsName);

        [DllImport("coredll.dll")]
        private static extern int IsWindowVisible(IntPtr hWnd);

        // IME変換のウィンドウメッセージ
        private const uint WM_IME_STARTCOMPOSITION = 0x10D; // IME変換開始
        private const uint WM_IME_ENDCOMPOSITION = 0x10E;   // IME変換終了

        // マウスホイール関連
        private const int WM_MOUSEWHEEL = 0x20A;

        // ATOK推測変換ウィンドウのウィンドウクラス
        private const string ATOK_CONJECTURE_CLASS = "ATOKMConjecture";

        #endregion

        /// <summary>
        /// IMEの入力が開始したときに発生するイベント
        /// </summary>
        public event EventHandler StartComposition;

        /// <summary>
        /// IMEの入力が終了したときに発生するイベント
        /// </summary>
        public event EventHandler EndComposition;

        /// <summary>
        /// マウスホイール・ジョグホイールが上方向に動いたときに発生するイベント
        /// </summary>
        public event EventHandler MouseWheelMoveUp;

        /// <summary>
        /// マウスホイール・ジョグホイールが下方向に動いたときに発生するイベント
        /// </summary>
        public event EventHandler MouseWheelMoveDown;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetTextBox">制御対象のテキストボックス</param>
        public InputBoxInputFilter(TextBox targetTextBox) : base(targetTextBox)
        {

        }

        /// <summary>
        /// ウィンドウプロシージャ
        /// </summary>
        protected override int WndProc(IntPtr hwnd, uint msg, uint wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case WM_IME_STARTCOMPOSITION:  // IME 変換開始
                    m_compositioning = true;
                    if (StartComposition != null)
                        StartComposition(this, EventArgs.Empty);
                    break;

                case WM_IME_ENDCOMPOSITION:    // IME 変換終了
                    m_compositioning = false;
                    if (EndComposition != null)
                        EndComposition(this, EventArgs.Empty);
                    break;

                case WM_MOUSEWHEEL:

                    int delta = GetWheelDeltaWParam(wParam);

                    if (WHEEL_DELTA <= delta)
                    {
                        if (MouseWheelMoveUp != null)
                            MouseWheelMoveUp(this, EventArgs.Empty);
                    }
                    else if (delta <= -WHEEL_DELTA)
                    {
                        if (MouseWheelMoveDown != null)
                            MouseWheelMoveDown(this, EventArgs.Empty);
                    }

                    break;

            }

            // デフォルトのプロシージャへ
            return base.WndProc(hwnd, msg, wParam, lParam);
        }

        /// <summary>
        /// ATOK推測変換がアクティブかどうか
        /// </summary>
        /// <returns>ATOK推測変換がアクティブなら true</returns>
        public bool IsAtokConjectureActive()
        {
            IntPtr atokPtr = FindWindow(ATOK_CONJECTURE_CLASS, null);

            if (atokPtr != IntPtr.Zero)
            {
                int hr = IsWindowVisible(atokPtr);
                return hr == 1;
            }
            else
            {
                return false;
            }
        }

        #region マウスホイール(ジョグダイアル)関連

        private int GetWheelDeltaWParam(uint wParam)
        {
            return (int)HiWord(wParam);
        }

        private int HiWord(uint l)
        {
            return (int)l / 65536;
        }

        //int deltaSum = 0;
        int WHEEL_DELTA = 120;

        #endregion  

        #region プロパティ

        /// <summary>
        /// 変換中かどうか
        /// </summary>
        public bool Conpositioning
        {
            get { return m_compositioning || IsAtokConjectureActive(); }
        }
        private bool m_compositioning = false;

        #endregion
    }
}
