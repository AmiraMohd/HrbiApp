using DBContext;
using HrbiApp.API.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tzwed.API.Helpers
{
    public class NotificationSender
    {
        string _serverKey;
        ExceptionHandler _ex;
        ApplicationDBContext _db;
        IConfiguration _configuration;
        public NotificationSender(ApplicationDBContext db, IConfiguration configuration)
        {
            _db = db;
            _serverKey = Consts.NotificationServerKey;
            _ex = new ExceptionHandler(db);
            _configuration = configuration;

        }

        public async Task NotifyDoctorWithBooking(string orderID, string clientID)
        {
            try
            {
                var client = _db.Users.Find(clientID);
                var notification = new Notification()
                {
                    DataID = orderID,
                    UserID = clientID,
                    Body = Messages.NewDoctorBookingBody,
                    Title = Messages.NewDoctorBookingTitle,
                    Type = (int)Consts.NotificationTypes.DoctorBooking,
                };
                await SendNotification(notification);
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
            }
        }
        public string GetString(string name)
        {
            try
            {
                var setting = _db.Settings.FirstOrDefault(s => s.Name == name);
                if (setting == null)
                {
                    return "NotSet";
                }
                return setting.Value;
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return "NotSet";
            }
        }

        public async Task SendNotification(Notification notification)
        {
            try
            {
                var user = _db.Users.Find(notification.UserID);
                var reciever = user.InstanceID;

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _serverKey);
                    var model = new FireBaseNotificationModel()
                    {
                        to = reciever,
                        notification = new NotificationModel()
                        {
                            title = GetString(notification.Title+user.Language),
                            body = GetString(notification.Body + user.Language)
                        },
                        data = new
                        {
                            DataID = notification.DataID,
                            Title = GetString(notification.Title + user.Language),
                            Info = GetString(notification.Body + user.Language),
                            IsReaded = false,
                            Notification_type = notification.Type,
                            Date = notification.Time
                        }
                    };
                    var response = await client.PostAsJsonAsync("https://fcm.googleapis.com/fcm/send", model);
                    if (!response.IsSuccessStatusCode)
                    {
                        _ex.LogException(JsonSerializer.Serialize(response.Content), "NotificationSendError", MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);

                    }
                    else 
                    _ex.LogException(await response.Content.ReadAsStringAsync(), "NotificationSendSuccess", MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);

                    SaveNotification(notification);
                }
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
            }
        }
        public void SaveNotification(Notification notification)
        {
            try
            {
                _db.Notifications.Add(notification);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
            }
        }

     
        public class NotificationModel
        {
            public string body { get; set; }
            public string title { get; set; }
        }
        public class FireBaseNotificationModel
        {
            public string to { get; set; }
            public NotificationModel notification { get; set; }
            public object data { get; set; }
        }

    }
}
