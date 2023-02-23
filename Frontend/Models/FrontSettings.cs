namespace Frontend.Models
{
	public class FrontSettings
	{
		public IEnumerable<string> Channels { get; set; }
		public Dictionary<string, IEnumerable<ChatMessage>> Messages { get; set;}
		public long Timestamp { get; set; }
		public string CurrentChannel { get; set; }
	}
}
