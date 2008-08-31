using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace EbiSoft.EbIRC
{
    /// <summary>
    /// ログボックスのIME入力状態を監視するクラス
    /// </summary>
    class LogBoxInputFilter : TextBoxInputFilter
    {
        #region P/Invoke 宣言

        // ウィンドウメッセージ
        private const uint WM_LBUTTONUP   = 0x202;  // IME変換開始
        private const uint WM_LBUTTONDOWN = 0x201;  // IME変換終了
        private const uint WM_SIZE        = 0x0005; // サイズ変更イベント

        #endregion

        /// <summary>
        /// タップされたときに発生するイベント
        /// </summary>
        public event EventHandler TapDown;

        /// <summary>
        /// タップが離されたときに発生するイベント
        /// </summary>
        public event EventHandler TapUp;

        /// <summary>
        /// リサイズイベント
        /// </summary>
        public event EventHandler Resize;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetTextBox">制御対象のテキストボックス</param>
        public LogBoxInputFilter(TextBox targetTextBox) : base(targetTextBox)
        {

        }

        /// <summary>
        /// ウィンドウプロシージャ
        /// </summary>
        protected override int WndProc(IntPtr hwnd, uint msg, uint wParam, int lParam)
        {
            // デフォルト動作前に発生
            switch (msg)
            {
                case WM_LBUTTONDOWN:  // タップ押した
                    if (TapDown != null)
                        TapDown(this, EventArgs.Empty);
                    break;

                case WM_LBUTTONUP:  // タップ離した
                    if (TapUp != null)
                        TapUp(this, EventArgs.Empty);
                    break;
            }

            // デフォルトのプロシージャへ
            int hr = base.WndProc(hwnd, msg, wParam, lParam);

            // デフォルト動作後に発生
            switch (msg)
            {
                case WM_SIZE:
                    if (Resize != null)
                        Resize(this, EventArgs.Empty);
                    break;
            }

            return hr;
        }
    }
}
