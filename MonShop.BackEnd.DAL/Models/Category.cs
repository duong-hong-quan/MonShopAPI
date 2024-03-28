using System.ComponentModel.DataAnnotations;

namespace MonShop.BackEnd.DAL.Models;

public class Category
{
    [Key] public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;
    public string? CategoryDescription { get; set; }
    public string? CategoryImgUrl { get; set; }
    public bool IsDeleted { get; set; }
}