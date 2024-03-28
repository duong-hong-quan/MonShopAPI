using MonShop.BackEnd.DAL.Models;

namespace MonShop.BackEnd.DAL.Common;

public class OrderResponse
{
    public Order Order { get; set; }
    public IEnumerable<OrderItem> Items { get; set; }
    public PaymentResponse Payment { get; set; }
}