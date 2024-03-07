
using Microsoft.Extensions.Options;
using static Monshop.BackEnd.Service.Services.GoogleNotification;
using System.Net.Http.Headers;
using System.Runtime;
using CorePush.Google;

namespace Monshop.BackEnd.Service.Services.Firebase
{

    public class CloudMessagingService : ICloudMessagingService
    {
        private readonly FcmNotificationSetting _fcmNotificationSetting;
        public CloudMessagingService(IOptions<FcmNotificationSetting> settings)
        {
            _fcmNotificationSetting = settings.Value;
        }

        public async Task<ResponseModel> SendNotification(NotificationModel notificationModel)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                if (string.IsNullOrEmpty(notificationModel.DeviceId))
                {
                    response.IsSuccess = false;
                    response.Message = "Device token is empty or null.";
                    return response;
                }
                using (HttpClient httpClient = new HttpClient())
                {
                    string authorizationKey = $"key={_fcmNotificationSetting.ServerKey}";
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload dataPayload = new DataPayload
                    {
                        Title = notificationModel.Title,
                        Body = notificationModel.Body
                    };

                    GoogleNotification notification = new GoogleNotification
                    {
                        Data = dataPayload,
                        Notification = dataPayload
                    };

                    var fcmSender = new FcmSender(new FcmSettings
                    {
                        SenderId = _fcmNotificationSetting.SenderId,
                        ServerKey = _fcmNotificationSetting.ServerKey
                    }, httpClient);

                    var registrationToken = notificationModel.DeviceId;
                    var fcmSendResponse = await fcmSender.SendAsync(registrationToken, notification);

                    if (fcmSendResponse.IsSuccess())
                    {
                        response.IsSuccess = true;
                        response.Message = "Notification sent successfully";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = $"Firebase notification error: {fcmSendResponse.Results[0].Error}";
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Something went wrong: {ex.Message}";
            }
            return response;
        }
    }
}
