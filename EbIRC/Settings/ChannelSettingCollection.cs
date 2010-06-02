using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EbiSoft.EbIRC.Settings
{
    public class ChannelSettingCollection : List<ChannelSetting>
    {
        ChannelNameEqualityComparer m_chNameComparer = new ChannelNameEqualityComparer();

        public ChannelSetting SearchChannel(string channelName)
        {
            foreach (ChannelSetting ch in this)
            {
                if (m_chNameComparer.Equals(ch.Name, channelName))
                {
                    return ch;
                }
            }
            return null;
        }
    }
}
