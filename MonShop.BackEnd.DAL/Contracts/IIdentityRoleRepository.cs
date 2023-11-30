using Microsoft.AspNetCore.Identity;
using MonShop.BackEnd.DAL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.DAL.Contracts
{
    public interface IIdentityRoleRepository : IRepository<IdentityRole>
    {
    }
}
