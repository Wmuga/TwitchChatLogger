using Frontend.Services;
using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Reflection.Metadata.Ecma335;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        readonly ILoggerService _logger;
        public HomeController(ILoggerService logger) 
        {
            _logger = logger;
        }

		public async Task<IActionResult> Index()
        {
            await Shared.AddListen(_logger);
            
            // Channels
            var msgs = await MessagesService.GetMessages(Shared.FrontSettings.Channels);
            if (msgs == null)
            {
                // TODO:Add view with error
                return NotFound();
            }
            var channels = Shared.FrontSettings.Channels;
            ViewData["Channels"] = channels;

            var timestamps = msgs.Select(e => e.Timestamp).ToList();
            timestamps.Sort();
            long lastTime = timestamps.Last();
            ViewData["Timestamp"] = lastTime;
            Shared.FrontSettings.Timestamp = lastTime;

            var msgsSorted = MessagesService.SortMessages(msgs, Shared.FrontSettings.Channels);
			Shared.FrontSettings.Messages = msgsSorted;

            string curChannel = Shared.FrontSettings.Channels.First();
            ViewData["Channel"] = curChannel;
			Shared.FrontSettings.CurrentChannel = curChannel;

			ViewData["Messages"] = msgsSorted;
            return View();
        }
	}
}
