using System;

namespace EbiSoft.EbIRC.IRC 
{
	/// <summary>
	/// KickEventArgs ‚ÌŠT—v‚Ìà–¾‚Å‚·B
	/// </summary>
	public class KickEventArgs : EventArgs
	{

		string        m_user;
		string        m_channel;
		string        m_target;

		public string User
		{
			get{ return m_user;  }
		}

		public string Target
		{
			get{ return m_target;  }
		}

		public string Channel
		{
			get{ return m_channel;  }
		}

		public KickEventArgs(string user, string channel, string target)
		{
			m_user    = user;
			m_target  = target;
			m_channel = channel;
		}
	}

	public delegate void KickEventHandler(object sender, KickEventArgs e);
}


