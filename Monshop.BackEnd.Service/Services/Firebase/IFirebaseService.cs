using Microsoft.AspNetCore.Http;
using MonShop.BackEnd.Common.Dto.Request;

namespace Monshop.BackEnd.Service.Services.Firebase;

public interface IFirebaseService
{
    public Task<AppActionResult> UploadImageToFirebase(IFormFile file, string pathFileName);
    public Task<string> GetUrlImageFromFirebase(string pathFileName);
    public Task<AppActionResult> DeleteImageFromFirebase(string pathFileName);
}