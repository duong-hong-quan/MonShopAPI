using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonShop.BackEnd.DAL.Models;

public class PaymentResponse
{
    [Key] public string PaymentResponseId { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    [ForeignKey("OrderId")] public Order Order { get; set; } = null!;

    public string? Amount { get; set; }
    public string? OrderInfo { get; set; }
    public bool Success { get; set; }

    public int PaymentTypeId { get; set; }

    [ForeignKey("PaymentTypeId")] public PaymentType PaymentType { get; set; } = null!;
}