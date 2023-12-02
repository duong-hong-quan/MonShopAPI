using Microsoft.EntityFrameworkCore;
using MonShop.BackEnd.DAL.Contracts;
using MonShop.BackEnd.DAL.Data;
using MonShop.BackEnd.DAL.Models;

namespace MonShop.BackEnd.DAL.Implementations
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(MonShopContext context) : base(context)
        {
        }
    }
}
