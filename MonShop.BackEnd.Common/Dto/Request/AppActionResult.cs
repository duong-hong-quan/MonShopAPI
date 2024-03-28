using MonShop.BackEnd.Common.Dto.Response;

namespace MonShop.BackEnd.Common.Dto.Request;

public class AppActionResult
{
    public object Data { get; set; } = new();

    public bool IsSuccess { get; set; } = true;
    public List<string?> Messages { get; set; } = new();
}