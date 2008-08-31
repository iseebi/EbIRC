using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using EbiSoft.EbIRC.Properties;

namespace EbiSoft.EbIRC
{
    public class ConnectionProfileData
    {

        /// <summary>
        /// プロファイルのリスト
        /// </summary>
        [XmlElement]
        public List<ConnectionProfile> Profile
        {
            get { return m_profiles; }
            set { m_profiles = value; }
        }
        private List<ConnectionProfile> m_profiles = new List<ConnectionProfile>();

        /// <summary>
        /// 選択中のプロファイルのインデックス
        /// </summary>
        [XmlAttribute]
        public int ActiveProfileIndex
        {
            get { return m_activeProfileIndex; }
            set {
                /*
                if ((value < 0) || (value > m_profiles.Count))
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    m_activeProfileIndex = value;
                }
                */
                m_activeProfileIndex = value;
            }
        }
        private int m_activeProfileIndex = -1;

        /// <summary>
        /// 選択されているプロファイル
        /// </summary>
        [XmlIgnore]
        public ConnectionProfile ActiveProfile
        {
            get
            {
                if (m_profiles.Count == 0)
                {
                    m_profiles.Add(new ConnectionProfile(Resources.DefaultProfileName));
                    if (m_activeProfileIndex < 0)
                    {
                        m_activeProfileIndex = 0;
                    }
                }
                return Profile[ActiveProfileIndex];
            }
        }
	}
}
