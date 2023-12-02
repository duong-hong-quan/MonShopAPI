using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Contracts
{
    public interface IEmailService
    {
        public void SendEmail(string recipient, string subject, string body);

    }
}
