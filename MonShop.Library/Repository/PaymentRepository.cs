using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MonShopLibrary.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly MonShopContext _db;

        public PaymentRepository(MonShopContext db)
        {
            _db = db;
        }

        public async Task AddPaymentMomo(MomoPaymentResponse momo)
        {
            MomoPaymentResponse momoDTO = await GetPaymentMomoByID(momo.PaymentResponseId);
            if (momoDTO == null)
            {
                await _db.MomoPaymentResponses.AddAsync(momo);
                await _db.SaveChangesAsync();
            }

        }

        public async Task<List<MomoPaymentResponse>> GetAllPaymentMomo()
        {
            List<MomoPaymentResponse> list = await _db.MomoPaymentResponses.ToListAsync();
            return list;
        }

        public async Task AddPaymentVNPay(VnpayPaymentResponse vnpayDTO)
        {
            VnpayPaymentResponse vnpay = await GetPaymentVNPayByID(vnpayDTO.PaymentResponseId);
            if (vnpay == null)
            {
                await _db.VnpayPaymentResponses.AddAsync(vnpayDTO);
                await _db.SaveChangesAsync();
            }

        }

        public async Task<List<VnpayPaymentResponse>> GetAllPaymentVNPay()
        {
            List<VnpayPaymentResponse> list = await _db.VnpayPaymentResponses.ToListAsync();
            return list;
        }
        public async Task AddPaymentPaypal(PayPalPaymentResponse paypalDTO)
        {
            PayPalPaymentResponse paypal = await GetPaymentPaypalByID(paypalDTO.PaymentResponseId);
            if (paypal == null)
            {
                await _db.PayPalPaymentResponses.AddAsync(paypalDTO);
                await _db.SaveChangesAsync();
            }

        }
        public async Task<List<PayPalPaymentResponse>> GetAllPaymentPayPal()
        {
            List<PayPalPaymentResponse> list = await _db.PayPalPaymentResponses.ToListAsync();
            return list;
        }

        public async Task UpdateStatusPaymentPayPal(string PaymentResponseId, bool success)
        {
            PayPalPaymentResponse payment = await GetPaymentPaypalByID(PaymentResponseId);
            if (payment != null)
            {
                payment.Success = success;
                await _db.SaveChangesAsync();

            }
        }
        public async Task UpdateStatusPaymentMomo(long PaymentResponseId, bool success)
        {
            MomoPaymentResponse payment = await GetPaymentMomoByID(PaymentResponseId);
            if (payment != null)
            {
                payment.Success = success;
                await _db.SaveChangesAsync();

            }
        }
        public async Task UpdateStatusPaymentVNPay(long PaymentResponseId, bool success)
        {
            VnpayPaymentResponse payment = await GetPaymentVNPayByID(PaymentResponseId);
            if (payment != null)
            {
                payment.Success = success;
                await _db.SaveChangesAsync();

            }
        }

        public async Task<MomoPaymentResponse> GetPaymentMomoByID(long PaymentResponseId)
        {
            MomoPaymentResponse momo = await _db.MomoPaymentResponses.FirstAsync(p => p.PaymentResponseId == PaymentResponseId);
            return momo;
        }

        public async Task<VnpayPaymentResponse> GetPaymentVNPayByID(long PaymentResponseId)
        {
            VnpayPaymentResponse vnpay = await _db.VnpayPaymentResponses.FirstAsync(p => p.PaymentResponseId == PaymentResponseId);
            return vnpay;
        }
        public async Task<PayPalPaymentResponse> GetPaymentPaypalByID(string PaymentResponseId)
        {
            PayPalPaymentResponse paypal = await _db.PayPalPaymentResponses.FirstAsync(p => p.PaymentResponseId == PaymentResponseId);
            return paypal;
        }

        public async Task<MomoPaymentResponse> GetPaymentMomoByOrderID(string OrderID)
        {
            MomoPaymentResponse momo = await _db.MomoPaymentResponses.Where(m => m.OrderId == OrderID).FirstAsync();
            return momo;
        }
        public async Task<VnpayPaymentResponse> GetPaymentVNPayByOrderID(string OrderID)
        {
            VnpayPaymentResponse vnpay = await _db.VnpayPaymentResponses.Where(v => v.OrderId == OrderID).FirstAsync();
            return vnpay;
        }

        public async Task<PayPalPaymentResponse> GetPaymentPaypalByOrderID(string OrderID)
        {
            PayPalPaymentResponse paypal = await _db.PayPalPaymentResponses.Where(p => p.OrderId == OrderID).FirstAsync();
            return paypal;
        }

        public Task<List<VnpayPaymentResponse>> GetAllPaymenVNPay()
        {
            throw new NotImplementedException();
        }
    }
}
