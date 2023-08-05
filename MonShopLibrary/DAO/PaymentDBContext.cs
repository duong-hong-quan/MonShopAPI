using Microsoft.EntityFrameworkCore;
using MonShopLibrary.Models;
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
            await this.MomoPaymentResponses.AddAsync(momo);
            await this.SaveChangesAsync();
        }

        public async Task<List<MomoPaymentResponse>> GetAllPaymentMomo()
        {
            List<MomoPaymentResponse> list = await this.MomoPaymentResponses.ToListAsync();
            return list;
        }

        public async Task AddPaymentVNPay(VnpayPaymentResponse vnpayDTO)
        {
            await this.VnpayPaymentResponses.AddAsync(vnpayDTO);
            await this.SaveChangesAsync();
        }
     
        public async Task<List<VnpayPaymentResponse>> GetAllPaymentVNPay()
        {
            List<VnpayPaymentResponse> list = await this.VnpayPaymentResponses.ToListAsync();
            return list;
        }
        public async Task AddPaymentPaypal(PayPalPaymentResponse paypalDTO)
        {
            await this.PayPalPaymentResponses.AddAsync(paypalDTO);
            await this.SaveChangesAsync();
        }
        public async Task<List<PayPalPaymentResponse>> GetAllPaymentPayPal()
        {
            List<PayPalPaymentResponse> list = await this.PayPalPaymentResponses.ToListAsync();
            return list;
        }

    }
}

