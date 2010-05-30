using System;
using System.Collections.Generic;
using System.Text;

namespace EbiSoft.EbIRC
{
    /// <summary>
    /// キーオペレーションの種類
    /// </summary>
    public enum EbIRCKeyOperations
    {
        /// <summary>
        /// デフォルト動作
        /// </summary>
        Default,
        /// <summary>
        /// クイックチャンネルセレクト、次へ
        /// </summary>
        QuickChannelNext,
        /// <summary>
        /// クイックチャンネルセレクト、前へ
        /// </summary>
        QuickChannelPrev,
        /// <summary>
        /// 次ページへ
        /// </summary>
        PageUp,
        /// <summary>
        /// 前ページへ
        /// </summary>
        PageDown,
        /// <summary>
        /// 入力ログ巻き戻し
        /// </summary>
        InputLogPrev,
        /// <summary>
        /// 入力ログ先送り
        /// </summary>
        InputLogNext,
        /// <summary>
        /// フォントサイズ拡大
        /// </summary>
        FontSizeUp,
        /// <summary>
        /// フォントサイズ縮小
        /// </summary>
        FontSizeDown,
        /// <summary>
        /// 動作なし
        /// </summary>
        NoOperation
    }

    /// <summary>
    /// キーワード反応の方法
    /// </summary>
    public enum EbIRCHighlightMethod
    {
        /// <summary>
        /// 反応なし
        /// </summary>
        None,
        /// <summary>
        /// バイブレーション
        /// </summary>
        Vibration,
        /// <summary>
        /// LED点灯
        /// </summary>
        Led,
        /// <summary>
        /// バイブ＋LED
        /// </summary>
        VibrationAndLed
    }

}
