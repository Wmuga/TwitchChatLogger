using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
	public class ListenController : Controller
	{
		public string Index()
		{
			return "Q";
		}

		// POST: Listen/Add
		[HttpPost]
		public async Task<ActionResult> Add([FromQuery] string? channel)
		{
			if (channel is null || channel?.Length == 0)
			{
				return NotFound();
			}

			if (channel!.StartsWith('#')) channel = channel[1..];
			
			HttpClient client = new()
			{
				BaseAddress = new Uri(Shared.AppSettings.BackendURIBase)
			};
			await client.PostAsync($"/Listen?channel={channel}",null);
			return NoContent();
		}

		[HttpDelete]
		public async Task<ActionResult> Remove([FromQuery] string? channel)
		{
			if (channel is null || channel?.Length == 0)
			{
				return NotFound();
			}

			if (channel!.StartsWith('#')) channel = channel[1..];
			
			HttpClient client = new()
			{
				BaseAddress = new Uri(Shared.AppSettings.BackendURIBase)
			};
			await client.DeleteAsync($"/Listen?channel={channel}");
			return NoContent();
		}
	}
}
