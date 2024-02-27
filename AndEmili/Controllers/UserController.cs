using AndEmili.Data;
using AndEmili.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AndEmili.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private AndEmiliContext _andEmiliContext;

        public UserController(ILogger<UserController> logger, AndEmiliContext andEmiliContext)
        {
            _logger = logger;
            _andEmiliContext = andEmiliContext;
        }

        [Route("GetByEmail")]
        [HttpGet]
        public async Task<User?> GetByEmail(string email)
        {
            return await _andEmiliContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        [Route("CreateUser")]
        [HttpPost]
        public async Task<User?> CreateUser(string email)
        {
            User newUser = new User() { Email = email };
            var addedUser = await _andEmiliContext.Users.AddAsync(newUser);
            await _andEmiliContext.SaveChangesAsync();
            newUser.Id = addedUser.Entity.Id;
            return newUser;
        }

        [Route("GetOrCreateUser")]
        [HttpPost]
        public async Task<User?> GetOrCreateUser(string email)
        {
           User? user = await _andEmiliContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                user = await this.CreateUser(email);
            }

            return user;
        }

    }
}
