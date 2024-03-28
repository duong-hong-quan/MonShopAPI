using System.ComponentModel.DataAnnotations;

namespace MonShop.BackEnd.DAL.Models;

public class OrderStatus
{
    [Key] public int OrderStatusId { get; set; }

    public string Status { get; set; } = null!;
}