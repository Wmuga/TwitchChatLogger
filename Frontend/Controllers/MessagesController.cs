using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
	public class MessagesController : Controller
	{
		public async Task<Dictionary<string,IEnumerable<ChatMessage>>> Index([FromQuery] long? timestamp = null, [FromQuery] string[]? channels = null)
		{
			channels ??= Array.Empty<string>();
			for (int i = 0; i < channels.Length; i++)
			{
				var channel = channels[i];
				if (!(channel.StartsWith('#')))
				{
					channels[i] = '#'+channel;
				}
			}
			timestamp ??= -1;
			return MessagesService.SortMessages(
				await MessagesService.GetMessages(new List<string>(channels),timestamp),
				channels);
		}
	}
}
