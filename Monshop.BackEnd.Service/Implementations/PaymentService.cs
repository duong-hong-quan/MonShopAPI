using AutoMapper;
using Monshop.BackEnd.Service.Contracts;
using MonShop.BackEnd.DAL.Contracts;
using MonShop.BackEnd.DAL.DTO.Response;
using MonShop.BackEnd.DAL.Models;
using PayPal.v1.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Implementations
{
    public class PaymentService : IPaymentService
    {
        private IPaymentResponseRepository _responseRepository;
        private IUnitOfWork _unitOfWork;
        private AppActionResult _result;
        private IMapper _mapper;

        public PaymentService(IPaymentResponseRepository responseRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _responseRepository = responseRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _result = new();
        }
        public async Task<AppActionResult> AddPaymentRespone(PaymentResponse payment)
        {
            await _responseRepository.Insert(payment);
            await _unitOfWork.SaveChangeAsync();
            return _result;
        }

        public async Task<AppActionResult> GetAllPayment()
        {
            _result.Data = await _responseRepository.GetListByExpression(null, p => p.Order, p => p.PaymentType);
            return _result;
        }

        public async Task<AppActionResult> GetAllPaymentById(string paymentId)
        {
            _result.Data = await _responseRepository.GetListByExpression(p => p.PaymentResponseId == paymentId, p => p.Order, p => p.PaymentType);
            return _result;
        }

        public async Task<AppActionResult> GetPaymentByAccountId(string accountId)
        {
            _result.Data = await _responseRepository.GetListByExpression(p => p.Order.ApplicationUserId == accountId, p => p.Order, p => p.PaymentType);

            return _result;
        }
    }
}
