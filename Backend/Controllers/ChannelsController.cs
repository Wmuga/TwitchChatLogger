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

namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        readonly ChatContext _context;
        readonly ILoggerService _logger;
        public ChannelsController(ChatContext context, ILoggerService logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Channels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Channel>>> GetGetChannels()
        {
            _logger.Log("Channels");
            return await _context.Channels.ToListAsync();
        }
    }
}
