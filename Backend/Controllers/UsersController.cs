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
    public class UsersController : ControllerBase
    {
        private readonly ChatContext _context;
        readonly ILoggerService _logger;

        public UsersController(ChatContext context, ILoggerService logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetGetUsers()
        {
            _logger.Log("GET: /Users");
            return await _context.Users.ToListAsync();
        }
    }
}
