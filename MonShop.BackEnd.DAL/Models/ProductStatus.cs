using System.ComponentModel.DataAnnotations;

namespace MonShop.BackEnd.DAL.Models;

public class ProductStatus
{
    [Key] public int ProductStatusId { get; set; }

    public string Status { get; set; } = null!;
}