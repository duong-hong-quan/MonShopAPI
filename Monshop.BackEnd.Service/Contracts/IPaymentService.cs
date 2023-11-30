using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Contracts
{
    public interface IPaymentService
    {
        public Task AddPaymentRespone(PaymentResponse payment);
        public Task<IEnumerable<PaymentResponse>> GetAllPayment();
        public Task<IEnumerable<PaymentResponse>> GetAllPaymentById(string paymentId);

        public Task<IEnumerable<PaymentResponse>> GetPaymentByAccountId(string accountId);
    }
}
