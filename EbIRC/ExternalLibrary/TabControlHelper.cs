// In The Hand - .NET Components for Mobility
//
// InTheHand.WindowsMobile.Forms.TabControlHelper
// 
// Copyright (c) 2009 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace InTheHand.WindowsMobile.Forms
{
    /// <summary>
    /// Provides helper methods for the <see cref="TabControl"/> on Windows Mobile 6.5.
    /// </summary>
    public static class TabControlHelper
    {
        [DllImport("coredll")]
        private static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        private const int GWL_STYLE = -16;

        [DllImport("coredll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("coredll")]
        internal static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);

        private const int GW_CHILD = 5;

        private const int TCS_TOOLTIPS = 0x4000;

        /// <summary>
        /// Updates the selected <see cref="TabControl"/> with the Windows Mobile 6.5 style.
        /// </summary>
        /// <param name="tabControl"></param>
        public static void EnableVisualStyle(TabControl tabControl)
        {
            //get handle of native control
            IntPtr hNativeTab = GetWindow(tabControl.Handle, GW_CHILD);
            //get current style flags
            int style = GetWindowLong(hNativeTab, GWL_STYLE);
            //add tooltips style
            style = SetWindowLong(hNativeTab, GWL_STYLE, style | TCS_TOOLTIPS);
        }
    }
}