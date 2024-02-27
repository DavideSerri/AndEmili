using AndEmili.Models;
using Microsoft.AspNetCore.Mvc;

namespace AndEmili.Dto
{
    public class ScryfallDtoResponse
    {
        public string? Object {  get; set; }
        public int? Total_Cards { get; set; }
        public bool? Has_More {  get; set; }
        public string? Next_Page { get; set; }
        public List<ScryfallCardDtoResponse>? Data { get; set; }
    }
}
