using Microsoft.AspNetCore.Http;

namespace MonShop.BackEnd.Common.Dto.Request
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? CategoryDescription { get; set; }
        public IFormFile CategoryImgUrl { get; set; }
    }
}
