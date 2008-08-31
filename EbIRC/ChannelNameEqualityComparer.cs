using System;
using System.Collections.Generic;
using System.Text;

namespace EbiSoft.EbIRC
{
    /// <summary>
    /// チャンネル名比較用
    /// </summary>
    class ChannelNameEqualityComparer : IEqualityComparer<string>
    {
        #region IEqualityComparer<string> メンバ

        public bool Equals(string x, string y)
        {
            return x.ToLower().Equals(y.ToLower());
        }

        public int GetHashCode(string obj)
        {
            return obj.ToLower().GetHashCode();
        }

        #endregion
    }
}
