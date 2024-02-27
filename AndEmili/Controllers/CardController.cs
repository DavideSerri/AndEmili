using AndEmili.Data;
using AndEmili.Dto;
using AndEmili.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using System.Linq;

namespace AndEmili.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private AndEmiliContext _andEmiliContext;
        private readonly IHttpClientFactory _httpClientFactory;

        public CardController(ILogger<UserController> logger, AndEmiliContext andEmiliContext, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _andEmiliContext = andEmiliContext;
            _httpClientFactory = httpClientFactory;
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<IEnumerable<Card>> GetAll()
        {

            List<ScryfallCardDtoResponse> scryfallCards = new List<ScryfallCardDtoResponse>();
            List<Card> cards = new List<Card>();

            for (int page = 1; page < 3; page++)
            {
                scryfallCards = await this.getPage(page);
                cards = this.filterCards(scryfallCards);
            }

            return cards;
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


        private async Task<List<ScryfallCardDtoResponse>> getPage(int page)
        {
            var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://api.scryfall.com/cards/search?format=json&include_variations=false&order=penny&page={page}&q=b%3Ausg+f%3Apenny&unique=cards&dir=desc")
            {
                Headers =
            {
                { HeaderNames.Accept, "application/vnd.github.v3+json" },
                { HeaderNames.UserAgent, "HttpRequestsSample" }
            }
            };

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            List<ScryfallCardDtoResponse> cards = new List<ScryfallCardDtoResponse>();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                ScryfallDtoResponse? responseDto = await JsonSerializer.DeserializeAsync
                     <ScryfallDtoResponse>(contentStream, options);

                if (responseDto != null && responseDto.Data != null)
                {
                    foreach (ScryfallCardDtoResponse card in responseDto.Data)
                    {
                        cards.Add(card);
                    }
                }
            }

            return cards;
        }

        private List<Card> filterCards(List<ScryfallCardDtoResponse> scryfallCards)
        {
            List<Card> cards = new List<Card>();
            foreach (ScryfallCardDtoResponse scryfallCard in scryfallCards)
            {
                if (scryfallCard.Penny_Rank != null)
                {
                    Card cardToAdd = new Card
                    {
                        PennyRank = scryfallCard.Penny_Rank,
                        ScryfallId = scryfallCard.Id,
                        Name = scryfallCard.Name
                    };
                    cards.Add(cardToAdd);
                }
            }
            return cards;
        }


    }
}
