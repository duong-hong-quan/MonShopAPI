using MonShopLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.Repository
{
    public interface IPaymentRepository
    {
        public Task AddPaymentMomo(MomoPaymentResponse momo);
        public Task<List<MomoPaymentResponse>> GetAllPaymentMomo();
        public Task AddPaymentVNPay(VnpayPaymentResponse vnpayDTO);
        public Task<List<VnpayPaymentResponse>> GetAllPaymenVNPay();
        public Task AddPaymentPaypal(PayPalPaymentResponse paypalDTO);
        public Task<List<PayPalPaymentResponse>> GetAllPaymentPayPal();

        public  Task UpdateStatusPaymentPayPal(string PaymentResponseId, bool success) ;

        public  Task UpdateStatusPaymentMomo(long PaymentResponseId, bool success);

        public  Task UpdateStatusPaymentVNPay(long PaymentResponseId, bool success);
        public  Task<MomoPaymentResponse> GetPaymentMomoByID(long PaymentResponseId) ;
        public  Task<VnpayPaymentResponse> GetPaymentVNPayByID(long PaymentResponseId) ;
        public  Task<PayPalPaymentResponse> GetPaymentPaypalByID(string PaymentResponseId) ;
    }
}
