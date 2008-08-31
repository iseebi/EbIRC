using System;

namespace EbiSoft.EbIRC.IRC {
	/// <summary>
	/// メッセージ解析失敗例外
	/// </summary>
	public class MessageParseException : Exception {
		public MessageParseException(Exception innerException) : base("メッセージの解析に失敗しました。", innerException)
		{
			
		}
	}
}
