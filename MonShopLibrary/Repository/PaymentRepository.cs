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
       

        public async Task<List<VnpayPaymentResponse>> GetAllPaymenVNPay()=> await db.GetAllPaymenVNPay();   
        

    }
}
