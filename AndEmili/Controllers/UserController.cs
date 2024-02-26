using AndEmili.Data;
using AndEmili.Models;
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

        [HttpGet(Name = "GetByEmail")]
        public async Task<User?> GetByEmail(string email)
        {
            return await _andEmiliContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }


    }
}
