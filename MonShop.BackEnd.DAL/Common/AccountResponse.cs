using Microsoft.AspNetCore.Identity;
using MonShop.BackEnd.DAL.Models;

namespace MonShop.BackEnd.DAL.Common
{
    public class AccountResponse
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<IdentityRole> Role { get; set; } = new List<IdentityRole>();
    }
}
