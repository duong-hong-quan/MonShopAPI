using MonShop.BackEnd.DAL.DTO.Response;
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
        public Task<AppActionResult> AddPaymentRespone(PaymentResponse payment);
        public Task<AppActionResult> GetAllPayment();
        public Task<AppActionResult> GetAllPaymentById(string paymentId);

        public Task<AppActionResult> GetPaymentByAccountId(string accountId);
    }
}
