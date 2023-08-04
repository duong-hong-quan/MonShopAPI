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
        public  Task AddPaymentMomo(MomoPaymentResponse momo) ;

        public  Task<List<MomoPaymentResponse>> GetAllPaymentMomo();
        public  Task AddPaymentVNPay(VnpayPaymentResponse vnpayDTO);


        public  Task<List<VnpayPaymentResponse>> GetAllPaymenVNPay() ;
    }
}
