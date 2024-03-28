using System.ComponentModel.DataAnnotations;

namespace MonShop.BackEnd.DAL.Models;

public class PaymentType
{
    [Key] public int PaymentTypeId { get; set; }

    public string Type { get; set; }
}