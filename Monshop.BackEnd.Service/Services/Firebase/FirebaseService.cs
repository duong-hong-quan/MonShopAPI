﻿using System.Net;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MonShop.BackEnd.Common.Dto.Request;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Monshop.BackEnd.Service.Services.Firebase;

public class FirebaseService : IFirebaseService
{
    private readonly IConfiguration _configuration;
    private readonly AppActionResult _result;

    public FirebaseService(IConfiguration configuration)
    {
        _configuration = configuration;
        _result = new AppActionResult();
    }

    public async Task<AppActionResult> UploadImageToFirebase(IFormFile file, string pathFileName)
    {
        var isValid = true;
        if (file == null || file.Length == 0)
        {
            isValid = false;
            _result.Messages.Add("The file is empty");
        }

        if (isValid)
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var stream = new MemoryStream(memoryStream.ToArray());
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_configuration["Firebase:ApiKey"]));

                var account = await auth.SignInWithEmailAndPasswordAsync(_configuration["Firebase:AuthEmail"],
                    _configuration["Firebase:AuthPassword"]);
                var cancellation = new CancellationTokenSource();

                var destinationPath = $"{pathFileName}";


                var task = new FirebaseStorage(
                        _configuration["Firebase:Bucket"],
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(account.FirebaseToken),
                            ThrowOnCancel = true
                        })
                    .Child(destinationPath)
                    .PutAsync(stream, cancellation.Token);
                if (task != null)
                {
                    _result.Data = await GetUrlImageFromFirebase(pathFileName);
                }
                else
                {
                    _result.IsSuccess = false;
                    _result.Messages.Add("Upload failed");
                }
            }

        return _result;
    }

    public async Task<string> GetUrlImageFromFirebase(string pathFileName)
    {
        var a = pathFileName.Split("/");
        pathFileName = $"{a[0]}%2F{a[1]}";
        var api = $"https://firebasestorage.googleapis.com/v0/b/yogacenter-44949.appspot.com/o?name={pathFileName}";
        if (string.IsNullOrEmpty(pathFileName))
        {
            return string.Empty;
        }

        var client = new RestClient();
        var request = new RestRequest(api);
        var response = client.Execute(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var jmessage = JObject.Parse(response.Content);
            var downloadToken = jmessage.GetValue("downloadTokens").ToString();
            return
                $"https://firebasestorage.googleapis.com/v0/b/{_configuration["Firebase:Bucket"]}/o/{pathFileName}?alt=media&token={downloadToken}";
        }

        return string.Empty;
    }

    public async Task<AppActionResult> DeleteImageFromFirebase(string pathFileName)
    {
        try
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(_configuration["Firebase:ApiKey"]));

            var account = await auth.SignInWithEmailAndPasswordAsync(_configuration["Firebase:AuthEmail"],
                _configuration["Firebase:AuthPassword"]);
            var cancellation = new CancellationTokenSource();

            var storage = new FirebaseStorage(
                _configuration["Firebase:Bucket"],
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(account.FirebaseToken),
                    ThrowOnCancel = true
                });
            await storage
                .Child(pathFileName)
                .DeleteAsync();
            _result.Messages.Add("Delete image successful");
        }
        catch (FirebaseStorageException ex)
        {
            _result.Messages.Add($"Error deleting image: {ex.Message}");
        }

        return _result;
    }
}