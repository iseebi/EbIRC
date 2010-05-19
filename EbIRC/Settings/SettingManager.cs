using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace EbiSoft.EbIRC.Settings
{
    /// <summary>
    /// �ݒ�
    /// </summary>
    public class SettingManager
    {
        private static readonly string settingFile = Path.Combine(Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName),
        Properties.Resources.ResourceManager.GetString("SettingFileName"));

        private const string REG_KEY = @"Software\EbiSoft\EbIRC\";

        private static Setting m_data;

        /// <summary>
        /// �C���X�^���X
        /// </summary>
        public static Setting Data
        {
            get {
                if (m_data == null)
                    m_data = new Setting();
                return m_data;
            }
        }

        private static bool m_deserializing = false;

        /// <summary>
        /// �f�V���A���C�Y���t���O
        /// </summary>
        public static bool Deserializing
        {
            get { return m_deserializing; }
        }
	

        /// <summary>
        /// �l��ǂݍ���
        /// </summary>
        public static void ReadSetting()
        {
            // �X�N���[�������l�̎擾(�c��ʎ��ł�Width�����ӂɂȂ�悤�ɑ��)
            int screenWidth;
            int screenHeight;
            if (Screen.PrimaryScreen.Bounds.Height < Screen.PrimaryScreen.Bounds.Width)
            {
                // �����
                screenWidth = Screen.PrimaryScreen.Bounds.Width;
                screenHeight = Screen.PrimaryScreen.Bounds.Height;
            }
            else
            {
                // �c���
                screenWidth = Screen.PrimaryScreen.Bounds.Height;
                screenHeight = Screen.PrimaryScreen.Bounds.Width;
            }

            // �ݒ�t�@�C���̑��݂��m�F
            if (File.Exists(settingFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Setting));
                using (FileStream fs = new FileStream(settingFile, FileMode.Open, FileAccess.Read))
                {
                    m_deserializing = true;
                    m_data = (Setting)serializer.Deserialize(fs);
                    m_deserializing = false;

                    // ���ݒ�ϊ�����
                    foreach (ConnectionProfile prof in m_data.Profiles.Profile)
                    {
                        // �G���R�[�h�ݒ肪���ݒ�̏ꍇ�̓f�t�H���g�ݒ����������
                        if (string.IsNullOrEmpty(prof.Encoding))
                        {
                            prof.Encoding = Properties.Resources.DefaultEncoding;
                        }

                        // �`�����l���ݒ肪���݂���ꍇ�͏㏑������
                        if ((m_data.DefaultChannels.Length > 0) && (prof.DefaultChannels.Length == 0))
                        {
                            prof.DefaultChannels = m_data.DefaultChannels;
                        }

                        // �`�����l���ݒ肪���݂���ꍇ�͏㏑������
                        if ((prof.DefaultChannels.Length > 0) && (prof.Channels.Count == 0)) {
                            foreach (string ch in prof.DefaultChannels)
                            {
                                prof.Channels.Add(new ChannelSetting(ch));
                            }
                        }
                    }
                }
            }
            else
            {
                m_data = new Setting();
            }

            // �T�[�o�[�ꗗ�ǂݍ���
            // �܂��A�f�t�H���g�̈ꗗ��ǂݍ���ŁA���̌�t�@�C������ǂݍ��߂��炻��ŏ㏑������B
            m_data.DefaultServers = Properties.Resources.ResourceManager.GetString("DefaultServers").Replace("\r", "").Split("\n".ToCharArray());

            // �ϐ��錾 (�t�@�C�����́A�ꗗ���X�g�A�T�[�o�[��`�t�@�C��)
            StreamReader sr = null;
            List<string> tempServers = new List<string>();
            string serverFile = Path.Combine(Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName),
                    Properties.Resources.ResourceManager.GetString("DefaultServerFileName")
            );
            try
            {
                if (File.Exists(serverFile))
                {
                    sr = new StreamReader(serverFile);
                    while (!sr.EndOfStream)
                    {
                        tempServers.Add(sr.ReadLine());
                    }
                    m_data.DefaultServers = tempServers.ToArray();
                }
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }

        /// <summary>
        /// �l����������
        /// </summary>
        public static void WriteSetting()
        {
            XmlAttributeOverrides overrides;
            XmlAttributes attr;

            // ���ݒ�ϊ��F�f�t�H���g�`�����l���ݒ�𖳎�
            overrides = new XmlAttributeOverrides();
            attr = new XmlAttributes();
            attr.XmlIgnore = true;
            overrides.Add(typeof(Setting), "DefaultChannels", attr);

            // ���ݒ�ϊ��F�f�t�H���g�`�����l���ݒ�𖳎�(�v���t�@�C����)
            overrides = new XmlAttributeOverrides();
            attr = new XmlAttributes();
            attr.XmlIgnore = true;
            overrides.Add(typeof(ConnectionProfile), "DefaultChannels", attr);

            XmlSerializer serializer = new XmlSerializer(typeof(Setting), overrides);
            using (FileStream fs = new FileStream(settingFile, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(fs, m_data);
            }
        }
    }
}