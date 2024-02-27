using AndEmili.Data;
using AndEmili.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AndEmili.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserCardController : ControllerBase
    {
        private readonly ILogger<UserCardController> _logger;
        private AndEmiliContext _andEmiliContext;

        public UserCardController(ILogger<UserCardController> logger, AndEmiliContext andEmiliContext)
        {
            _logger = logger;
            _andEmiliContext = andEmiliContext;
        }

        [Route("AddUserCard")]
        [HttpPost]
        public async Task<UserCard?> AddUserCard(string scryfallId, int userId)
        {
            UserCard? userCard = await _andEmiliContext.UserCards.FirstOrDefaultAsync(x => x.ScryfallId == scryfallId && x.UserId == userId);

            if (userCard == null)
            {
                UserCard newUserCard = new UserCard() { ScryfallId = scryfallId, UserId = userId };
                var addedUserCard = await _andEmiliContext.UserCards.AddAsync(newUserCard);
                await _andEmiliContext.SaveChangesAsync();
                newUserCard.Id = addedUserCard.Entity.Id;
                userCard = newUserCard;
            }

            return userCard;
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<IEnumerable<UserCard>> GetAll(int userId)
        {

            IEnumerable<UserCard> userCards = await _andEmiliContext.UserCards.Where(x => x.UserId == userId).ToListAsync();
            return userCards;

        }
    }
}
