namespace MonShop.Payment;

public class PaymentInformationModel
{
    public string OrderID { get; set; }
    public string OrderInfo { get; set; }
    public string AccountID { get; set; }

    public string CustomerName { get; set; }
    public double Amount { get; set; }
}