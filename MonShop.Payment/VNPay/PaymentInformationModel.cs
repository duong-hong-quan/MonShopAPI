namespace VNPay.Models;

public class PaymentInformationModel
{
    public string OrderID { get; set; }
    public string OrderInfo { get; set; }
    public int AccountID { get; set; }

    public string CustomerName { get; set; }
    public double Amount { get; set; }
}