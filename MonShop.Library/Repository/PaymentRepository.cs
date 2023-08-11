using MonShopLibrary.DAO;
using MonShopLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        PaymentDBContext db = new PaymentDBContext();

        public async Task AddPaymentMomo(MomoPaymentResponse momo) => await db.AddPaymentMomo(momo);
       
        public async Task<List<MomoPaymentResponse>> GetAllPaymentMomo() => await db.GetAllPaymentMomo();
        public async Task AddPaymentVNPay(VnpayPaymentResponse vnpayDTO) => await db.AddPaymentVNPay(vnpayDTO);
        public async Task<List<VnpayPaymentResponse>> GetAllPaymenVNPay()=> await db.GetAllPaymentVNPay();
        public async Task AddPaymentPaypal(PayPalPaymentResponse paypalDTO) => await db.AddPaymentPaypal(paypalDTO);      
        public async Task<List<PayPalPaymentResponse>> GetAllPaymentPayPal()=> await db.GetAllPaymentPayPal();
        public async Task UpdateStatusPaymentPayPal(string PaymentResponseId, bool success) => await db.UpdateStatusPaymentPayPal(PaymentResponseId, success);        
        public async Task UpdateStatusPaymentMomo(long PaymentResponseId, bool success) => await db.UpdateStatusPaymentMomo(PaymentResponseId, success);
        public async Task UpdateStatusPaymentVNPay(long PaymentResponseId, bool success) => await db.UpdateStatusPaymentVNPay(PaymentResponseId, success);
        public async Task<MomoPaymentResponse> GetPaymentMomoByID(long PaymentResponseId) => await db.GetPaymentMomoByID(PaymentResponseId);
        public async Task<VnpayPaymentResponse> GetPaymentVNPayByID(long PaymentResponseId) => await db.GetPaymentVNPayByID(PaymentResponseId);
        public async Task<PayPalPaymentResponse> GetPaymentPaypalByID(string PaymentResponseId) => await db.GetPaymentPaypalByID(PaymentResponseId);

    }
}
