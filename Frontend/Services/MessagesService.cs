using Frontend.Models;

namespace Frontend.Services
{
	public static class MessagesService
	{
		public static async Task<IEnumerable<ChatMessage>?> GetMessages(IEnumerable<string> channels, long? timestamp = null)
		{
			HttpClient client = new()
			{
				BaseAddress = new Uri(Shared.AppSettings.BackendURIBase)
			};

			List<string> channelsList = new();
			foreach (var c in channels)
			{
				var channel = c;
				if (channel.StartsWith('#')) channel = channel[1..];
				channelsList.Add($"channels={channel}");
			}
			var queryString = string.Join("&", channelsList);
			if (timestamp is not null)
			{
				queryString+= $"&timestamp={timestamp}";
			}
			IEnumerable<ChatMessage>? msgs = await client.GetFromJsonAsync<IEnumerable<ChatMessage>>($"/Messages?{queryString}");
			return msgs;
		}

		public static Dictionary<string, IEnumerable<ChatMessage>> SortMessages(IEnumerable<ChatMessage>? msgs, IEnumerable<string> channels)
		{
			Dictionary<string, IEnumerable<ChatMessage>> msgsSorted = new();
			if (msgs is null) return msgsSorted;
			foreach (var channel in channels)
			{
				msgsSorted.Add(channel, msgs.Where(e => e.Channel == channel).AsEnumerable());
			}
			return msgsSorted;
		}
	}
}
