using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

// EbIRC Custom のソースからいただきました。
// Special Thanks to icchu

namespace EbiSoft.EbIRC
{
    /// <summary>
    /// LED、バイブレーションの制御を行うクラス
    /// </summary>
    public class Led
    {
        //W-ZERO3
        //LED,バイブの制御 APIを呼び出すクラス
        //
        //使い方
        //この.csファイルを「既存項目の追加」でもってプロジェクトに組み込んでやってください。
        //関数の呼び出しはスタティックで。
        //
        //led.SetLedStatus(int wLed, int wStatus)
        //
        // 引数：
        //       wLed       どのLEDかの順番 0 = 電源LEDを黄色に 1 = バイブレーション
        //       wStatus    0 = Off, 1 = On, 2 = Blink 

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern bool NLedGetDeviceInfo(uint h, ref NLED_COUNT_INFO pOutput);
        [DllImport("coredll.dll", SetLastError = true)]
        private static extern bool NLedSetDevice(uint h, ref NLED_SETTINGS_INFO pOutput);

        [StructLayout(LayoutKind.Sequential)]
        private struct NLED_SETTINGS_INFO
        {
            public uint LedNum;
            public int OffOnBlink;
            public long TotalCycleTime;
            public long OnTime;
            public long OffTime;
            public int MetaCycleOn;
            public int MetaCycleOff;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct NLED_COUNT_INFO
        {
            public uint cLeds;
        }

        private const uint NLED_COUNT_INFO_ID = 0;
        private const uint NLED_SETTINGS_INFO_ID = 2;

        public static int GetLedCount()
        {
            NLED_COUNT_INFO countInfo = new NLED_COUNT_INFO();
            int LEDCount = 0;
            if (NLedGetDeviceInfo(NLED_COUNT_INFO_ID, ref countInfo))
            {
                LEDCount = (int)countInfo.cLeds;
            }
            return LEDCount;
        }

        public static bool AvailableLed(LedType led)
        {
            return AvailableLed((int)led);
        }

        public static bool AvailableLed(int led)
        {
            return led < GetLedCount();
        }

        public static void SetLedStatus(LedType led, LedStatus status)
        {
            SetLedStatus((int)led, (int)status);
        }

        public static void SetLedStatus(int wLed, int wStatus)
        {
            if (!AvailableLed(wLed)) throw new InvalidOperationException();

            NLED_SETTINGS_INFO settingInfo = new NLED_SETTINGS_INFO();
            settingInfo.LedNum = System.Convert.ToUInt32(wLed);
            settingInfo.OffOnBlink = System.Convert.ToUInt16(wStatus);
            settingInfo.OnTime = 1000000;
            settingInfo.OffTime = 500000;
            //settingInfo.MetaCycleOn = 1;
            //settingInfo.MetaCycleOff = 20;
            NLedSetDevice(NLED_SETTINGS_INFO_ID, ref settingInfo);

        }
    }

    /// <summary>
    /// LEDの種類
    /// </summary>
    public enum LedType
    {
        /// <summary>
        /// 黄色
        /// </summary>
        Yellow = 0,
        /// <summary>
        /// バイブ
        /// </summary>
        Vibrartion = 1
    }

    public enum LedStatus
    {
        /// <summary>
        /// オフ
        /// </summary>
        Off = 0,
        /// <summary>
        /// オン
        /// </summary>
        On = 1,
        /// <summary>
        /// 点滅
        /// </summary>
        Blink = 2
    }
}
