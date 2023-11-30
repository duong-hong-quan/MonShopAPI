using AutoMapper;
using Monshop.BackEnd.Service.Contracts;
using MonShop.BackEnd.DAL.Contracts;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;
using MonShop.BackEnd.DAL.IRepository;
using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Implementations
{
    public class MessageService : GenericBackEndService, IMessageService
    {
        private IMessageRepository _messageRepository;
        private IUnitOfWork _unitOfWork;
        private AppActionResult _result;
        private IMapper _mapper;
        public MessageService(IMessageRepository messageRepository, IUnitOfWork unitOfWork, IMapper mapper, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
            _result = new();
            _mapper = mapper;
        }

        public async Task<AppActionResult> AddMessage(MessageRequest message)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var mess = await _messageRepository.GetByExpression(m => m.ApplicationUserId == message.AccountID);
                if (mess != null)
                {
                    Message DTO = new Message
                    {
                        RoomId = mess.RoomId,
                        ApplicationUserId = message.AccountID,
                        Content = message.Message,
                        SendTime = MonShop.BackEnd.Utility.Utils.Utility.GetInstance().GetCurrentDateTimeInTimeZone(),

                    };
                    await _messageRepository.Insert(DTO);
                    await _unitOfWork.SaveChangeAsync();


                }
                else
                {
                    var accountRepository = Resolve<IAccountRepository>();
                    var roomRepository = Resolve<IRoomRepository>();
                    ApplicationUser account = await accountRepository.GetByExpression(a => a.Id == message.AccountID);
                    Room room = new Room { RoomName = $"{account.FirstName} {account.LastName}", RoomImg = "" };
                    await roomRepository.Insert(room);
                    await _unitOfWork.SaveChangeAsync();
                    int roomID = room.RoomId;
                    Message DTO = new Message
                    {
                        RoomId = roomID,
                        ApplicationUserId = message.AccountID,
                        Content = message.Message,
                        SendTime = MonShop.BackEnd.Utility.Utils.Utility.GetInstance().GetCurrentDateTimeInTimeZone(),

                    };
                    await _messageRepository.Insert(DTO);
                    await _unitOfWork.SaveChangeAsync();
                    await _unitOfWork.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> AddMessageAdmin(MessageAdminRequest message)
        {
            try
            {
                Message mess = new Message
                {
                    Content = message.Content,
                    RoomId = message.RoomId,
                    ApplicationUserId = message.Sender,
                    SendTime = MonShop.BackEnd.Utility.Utils.Utility.GetInstance().GetCurrentDateTimeInTimeZone(),
                };
                await _messageRepository.Insert(mess);
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }

            return _result;
        }

        public async Task<AppActionResult> CreateRoom(RoomDTO room)
        {
            try
            {
                var roomRepository = Resolve<IRoomRepository>();
                await roomRepository.Insert(_mapper.Map<Room>(room));
                await _unitOfWork.SaveChangeAsync();

            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> DeleteRoom(int roomID)
        {
            try
            {
                var roomRepository = Resolve<IRoomRepository>();
                await roomRepository.DeleteById(roomID);
                await _unitOfWork.SaveChangeAsync();

            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetAllMessageByAccountID(string AccountID)
        {

            var Message = await _messageRepository.GetListByExpression(m => m.ApplicationUserId == AccountID);
            if (Message.Count() > 0)
            {
                Message mess = Message.ElementAt(0);
                if (mess != null)
                {
                    _result.Data = await _messageRepository.GetListByExpression(m => m.RoomId == mess.RoomId);

                }
            }

            return _result;
        }

        public async Task<AppActionResult> GetAllMessageByRoomID(int RoomID)
        {
            _result.Data = await _messageRepository.GetListByExpression(m => m.RoomId == RoomID);
            return _result;
        }

        public async Task<AppActionResult> GetAllRoom()
        {
            var roomRepository = Resolve<IRoomRepository>();
            _result.Data = await roomRepository.GetAll();
            return _result;
        }

        public async Task<AppActionResult> GetRoomByID(int roomID)
        {
            var roomRepository = Resolve<IRoomRepository>();
            _result.Data = await roomRepository.GetById(roomID);
            return _result;
        }

        public async Task<AppActionResult> UpdateRoom(Room room)
        {
            var roomRepository = Resolve<IRoomRepository>();
            await roomRepository.Update(room);
            return _result;
        }
    }
}
