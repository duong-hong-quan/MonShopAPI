using Microsoft.AspNetCore.Http;
using MonShop.BackEnd.Common.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Contracts
{
    public interface IFileService
    {
        public Task<AppActionResult> UploadImageToFirebase(IFormFile file, string pathFileName);
        public Task<string> GetUrlImageFromFirebase(string pathFileName);
        public Task<AppActionResult> DeleteImageFromFirebase(string pathFileName);
    }
}
