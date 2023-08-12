using Microsoft.EntityFrameworkCore;
using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.DAO
{
    public class PaymentDBContext : MonShopContext
    {
        public PaymentDBContext() { }

        public async Task AddPaymentMomo(MomoPaymentResponse momo)
        {
            MomoPaymentResponse momoDTO = await GetPaymentMomoByID(momo.PaymentResponseId);
            if (momoDTO == null)
            {
                await this.MomoPaymentResponses.AddAsync(momo);
                await this.SaveChangesAsync();
            }

        }

        public async Task<List<MomoPaymentResponse>> GetAllPaymentMomo()
        {
            List<MomoPaymentResponse> list = await this.MomoPaymentResponses.ToListAsync();
            return list;
        }

        public async Task AddPaymentVNPay(VnpayPaymentResponse vnpayDTO)
        {
            VnpayPaymentResponse vnpay = await GetPaymentVNPayByID(vnpayDTO.PaymentResponseId);
            if (vnpay == null)
            {
                await this.VnpayPaymentResponses.AddAsync(vnpayDTO);
                await this.SaveChangesAsync();
            }

        }

        public async Task<List<VnpayPaymentResponse>> GetAllPaymentVNPay()
        {
            List<VnpayPaymentResponse> list = await this.VnpayPaymentResponses.ToListAsync();
            return list;
        }
        public async Task AddPaymentPaypal(PayPalPaymentResponse paypalDTO)
        {
            PayPalPaymentResponse paypal = await GetPaymentPaypalByID(paypalDTO.PaymentResponseId);
            if (paypal == null)
            {
                await this.PayPalPaymentResponses.AddAsync(paypalDTO);
                await this.SaveChangesAsync();
            }

        }
        public async Task<List<PayPalPaymentResponse>> GetAllPaymentPayPal()
        {
            List<PayPalPaymentResponse> list = await this.PayPalPaymentResponses.ToListAsync();
            return list;
        }

        public async Task UpdateStatusPaymentPayPal(string PaymentResponseId, bool success)
        {
            PayPalPaymentResponse payment = await GetPaymentPaypalByID(PaymentResponseId);
            if (payment != null)
            {
                payment.Success = success;
                await this.SaveChangesAsync();

            }
        }
        public async Task UpdateStatusPaymentMomo(long PaymentResponseId, bool success)
        {
            MomoPaymentResponse payment = await GetPaymentMomoByID(PaymentResponseId);
            if (payment != null)
            {
                payment.Success = success;
                await this.SaveChangesAsync();

            }
        }
        public async Task UpdateStatusPaymentVNPay(long PaymentResponseId, bool success)
        {
            VnpayPaymentResponse payment = await GetPaymentVNPayByID(PaymentResponseId);
            if (payment != null)
            {
                payment.Success = success;
                await this.SaveChangesAsync();

            }
        }

        public async Task<MomoPaymentResponse> GetPaymentMomoByID (long PaymentResponseId)
        {
            MomoPaymentResponse momo = await this.MomoPaymentResponses.FindAsync(PaymentResponseId);
            return momo;
        }
        public async Task<VnpayPaymentResponse> GetPaymentVNPayByID(long PaymentResponseId)
        {
            VnpayPaymentResponse vnpay = await this.VnpayPaymentResponses.FindAsync(PaymentResponseId);
            return vnpay;
        }
        public async Task<PayPalPaymentResponse> GetPaymentPaypalByID(string PaymentResponseId)
        {
            PayPalPaymentResponse paypal = await this.PayPalPaymentResponses.FindAsync(PaymentResponseId);
            return paypal;
        }

    }
}

