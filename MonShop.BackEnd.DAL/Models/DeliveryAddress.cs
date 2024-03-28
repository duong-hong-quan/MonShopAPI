using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonShop.BackEnd.DAL.Models;

public class DeliveryAddress
{
    [Key] public string DeliveryAddressId { get; set; }

    public string Address { get; set; }

    public string ApplicationUserId { get; set; }

    [ForeignKey("ApplicationUserId")] public ApplicationUser ApplicationUser { get; set; }
}