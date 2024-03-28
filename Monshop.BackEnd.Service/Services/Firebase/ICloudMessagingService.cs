namespace Monshop.BackEnd.Service.Services.Firebase;

public interface ICloudMessagingService
{
    Task<ResponseModel> SendNotification(NotificationModel notificationModel);
}