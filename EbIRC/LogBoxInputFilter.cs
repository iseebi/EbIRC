using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

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
        private const int WM_GESTURE = 0x0119;

        // ジェスチャ関連
        private const int GID_BEGIN = 1;
        private const int GID_END = 2;
        private const int GID_PAN = 4;
        private const int GID_SCROLL = 8;
        private const int GID_HOLD = 9;
        private const int GID_SELECT = 10;
        private const int GID_DOUBLESELECT = 11;
        private const int GID_LAST = 11;

        private const int ARG_SCROLL_NONE = 0;
        private const int ARG_SCROLL_DOWN = 1;
        private const int ARG_SCROLL_LEFT = 2;
        private const int ARG_SCROLL_UP = 3;
        private const int ARG_SCROLL_RIGHT = 4;

        [StructLayout(LayoutKind.Sequential)]
        private struct GESTUREINFO
        {
            public uint cbSize;
            public uint dwFlags;              /* Gesture Flags */
            public uint dwID;                 /* Gesture ID */
            public IntPtr hwndTarget;            /* HWND of target window */
            public POINTS ptsLocation;         /* Coordinates of start of gesture */
            public uint dwInstanceID;         /* Gesture Instance ID */
            public uint dwSequenceID;         /* Gesture Sequence ID */
            public ulong ullArguments;     /* Arguments specific to gesture */
            public uint cbExtraArguments;      /* Size of extra arguments in bytes */
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINTS
        {
            public short x;
            public short y;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("coredll", SetLastError = true)]
        private static extern bool GetGestureInfo(IntPtr hGestureInfo, ref GESTUREINFO pGestureInfo);

        private int GetScrollAngle(ulong x)
        {
            unchecked
            {
                return (ushort)((ushort)(x >> 48) & 0xfff0);
            }
        }

        private int GetScrollDirection(ulong x)
        {
            unchecked
            {
                return (ushort)((ushort)(x >> 48) & 0x000f);
            }
        }

        private int GetScrollVelocity(ulong x)
        {
            unchecked
            {
                return (int)(short)(ushort)(x >> 32);
            }
        }

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
        /// タップが離されたときに発生するイベント
        /// </summary>
        public event EventHandler<FlickEventArgs> Flick;


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
        protected override int WndProc(IntPtr hwnd, uint msg, uint wParam, IntPtr lParam)
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
                case WM_GESTURE:
                    ProcessGuesture(hwnd, lParam);
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

        private void ProcessGuesture(IntPtr hwnd, IntPtr lParam)
        {
            GESTUREINFO gestureInfo = new GESTUREINFO();
            gestureInfo.cbSize = (uint)Marshal.SizeOf(typeof(GESTUREINFO));

            // Windows Mobile 6.5 未満では動かないのでreturn。
            // TODO ちゃんとビルド調べる
            if (System.Environment.OSVersion.Version.Build < 20000)
                return;

            if (GetGestureInfo(lParam, ref gestureInfo))
            {
                switch (gestureInfo.dwFlags)
                {
                    // フリック
                    case GID_SCROLL:
                        FlickDirection direction = (FlickDirection)GetScrollDirection(gestureInfo.ullArguments);
                        if (this.Flick != null)
                            Flick(this, new FlickEventArgs(direction));
                        break;
                }
            }
        }
    }

    /// <summary>
    /// フリックイベントのデータ
    /// </summary>
    public class FlickEventArgs : EventArgs
    {
        public readonly FlickDirection Direction;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FlickEventArgs(FlickDirection direction)
        {
            this.Direction = direction;
        }
    }

    /// <summary>
    /// フリックの向き
    /// </summary>
    public enum FlickDirection
    {
        None,
        Down,
        Left,
        Up,
        Right
    }
}
