using Newtonsoft.Json;

namespace Monshop.BackEnd.Service.Services;

public class ResponseModel
{
    [JsonProperty("isSuccess")] public bool IsSuccess { get; set; }

    [JsonProperty("message")] public string Message { get; set; }
}