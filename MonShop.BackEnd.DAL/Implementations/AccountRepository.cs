using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Data;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.DAL.Data;
using MonShop.BackEnd.DAL.IRepository;


namespace MonShop.BackEnd.DAL.Repository
{
    public class AccountRepository : Repository<ApplicationUser>, IAccountRepository
    {
        public AccountRepository(MonShopContext context) : base(context)
        {
        }
    }
}
