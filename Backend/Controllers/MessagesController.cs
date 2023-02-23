using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Context;
using Backend.Models;
using Backend.Service;
using System.Threading.Channels;

namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ChatContext _context;
        readonly ILoggerService _logger;

        public MessagesController(ChatContext context, ILoggerService logger)
        {
            _context = context;
            _logger = logger;
        }
        // GET: Messages
        [HttpGet]
        public async Task<IEnumerable<ChatMessage>> GetMessages(long? timestamp = null, int? count = null, [FromQuery] string[]? channels = null)
        {
            _logger.Log("GET: /messages");

            for (var i = 0; i < channels.Length; i++)
            {
				if (!channels[i].StartsWith('#')) channels[i] = '#' + channels[i];
            }

            count ??= 100;
            timestamp ??= -1;
            count = Math.Min(100, Math.Max(count.Value,0));
            return _context
                .Messages
                .Join(_context.Users, e => e.UserId, e1 => e1.Id, (e, e1) => new
                {
                    e1.Name,
                    e.Color,
                    e.ChannelId,
                    e.Content,
                    e.MsgType,
                    e.Timestamp
                })
                .Join(_context.Channels, e => e.ChannelId, e1 => e1.Id, (e, e1) => new ChatMessage
                {
                    User = e.Name,
                    Channel = e1.Name,
                    Color = e.Color,
                    Content = e.Content,
                    MsgType = e.MsgType,
                    Timestamp = e.Timestamp
                })
                .AsEnumerable()
                .Where(e =>
                    (channels == null) || (channels.Length == 0) || channels.Contains(e.Channel)
                            && ((timestamp < 0) || (e.Timestamp > timestamp))
                )
                .TakeLast(count.Value);
        }
    }
}
