using Microsoft.AspNetCore.Mvc;
using Monshop.BackEnd.Service.Services;
using Monshop.BackEnd.Service.Services.Firebase;

namespace MonShop.BackEnd.API.Controller;

[Route("notification")]
[ApiController]
public class CloudMessagingController : ControllerBase
{
    private readonly ICloudMessagingService _notificationService;

    public CloudMessagingController(ICloudMessagingService notificationService)
    {
        _notificationService = notificationService;
    }

    [Route("send")]
    [HttpPost]
    public async Task<IActionResult> SendNotification(NotificationModel notificationModel)
    {
        var result = await _notificationService.SendNotification(notificationModel);
        return Ok(result);
    }
}