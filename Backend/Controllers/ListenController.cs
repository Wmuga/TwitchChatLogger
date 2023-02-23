using Backend.ChatListener;
using Backend.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ListenController : ControllerBase
    {
        readonly Listener _listener;
		readonly ILoggerService _logger;
		public ListenController(IListener listener, ILoggerService logger)
        {
            _listener = (Listener)listener;
			_logger = logger;
		}

		[HttpPost]
        public async Task<NoContentResult> SubscribeToChat(string channel)
        {
            await _listener.Join(channel);
            _logger.Log("POST: /Listen");
            return NoContent();
        }

        [HttpDelete]
        public async Task<NoContentResult> UnSubscribeToChat(string channel)
        {
			_listener.Part(channel);
			_logger.Log("DELETE: /Listen");
			return NoContent();
        }
    }
}
