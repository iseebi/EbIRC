using System;
using System.Collections.Generic;
using System.Text;

namespace SslTest
{
    public enum SslValidateErrors
    {
        /// <summary>
        /// 不明 or 処理を行っていない
        /// </summary>
        Unknown,
        /// <summary>
        /// エラーなし
        /// </summary>
        Okay,
        /// <summary>
        /// 検証していない
        /// </summary>
        NotValidate,
        /// <summary>
        /// 発行機関がおかしい。たぶんオレオレ証明書
        /// </summary>
        InvalidIssuer,
        /// <summary>
        /// 証明書がパースできない
        /// </summary>
        BadData,
        /// <summary>
        /// 有効期限切れ
        /// </summary>
        Expired,
        /// <summary>
        /// 期間が始まっていない
        /// </summary>
        NotEffectDate,
        /// <summary>
        /// 他のサイトの証明書
        /// </summary>
        OtherSite
    }
}
