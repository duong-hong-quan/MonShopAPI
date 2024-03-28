namespace Monshop.BackEnd.Service.Payment.PaymentRequest;

public class PaymentInformationRequest
{
    public string OrderID { get; set; }
    public string AccountID { get; set; }

    public string CustomerName { get; set; }
    public double Amount { get; set; }
}