namespace Frontend.Models
{
	public class ChatMessage
	{
		public string User { get; set; }
		public string Channel { get; set; }
		public string Content { get; set; }
		public string MsgType { get; set; }
		public string Color { get; set; }
		public long Timestamp { get; set; }
	}
}
