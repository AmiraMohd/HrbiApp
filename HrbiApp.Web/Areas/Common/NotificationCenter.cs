using DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
//using Nancy.Json;
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

namespace HrbiApp.Web.Areas.Common
{
    public class NotificationCenter
    {
        string _serverKey;
        ExcptionHandler EXH;
        ApplicationDBContext _db;
        IConfiguration _configuration;
        public NotificationCenter(ApplicationDBContext db, IConfiguration configuration)
        {
            _db = db;
            _serverKey = Consts.NotificationServerKey;
            EXH = new ExcptionHandler(db);
            _configuration = configuration;

        }
        #region Lab Bookings
        public async Task NotifyPatientWithAcceptedLabBooking(int bookingID)
        {
            try
            {
                var booking = _db.LabServiceBookings.Find(bookingID);

                var patient = _db.Users.Find(booking.PatientID);
                var notification = new Notification()
                {
                    DataID = bookingID + "",
                    UserID = booking.PatientID,
                    Body = Messages.AcceptedLabBookingBody,
                    Title = Messages.AcceptedLabBookingTitle,
                    Type = (int)Consts.BookingsNotificationTypes.Accepted,

                };
                await SendNotification(notification);

            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
            }
        }
        public async Task NotifyPatientWithRejectedLabBooking(int bookingID)
        {
            try
            {
                var booking = _db.LabServiceBookings.Find(bookingID);
                var patient = _db.Users.Find(booking.PatientID);
                var notification = new Notification()
                {
                    DataID = bookingID + "",
                    UserID = booking.PatientID,
                    Body = Messages.RejectedLabBookingBody,
                    Title = Messages.RejectedLabBookingTitle,
                    Type = (int)Consts.BookingsNotificationTypes.Rejected,
                };
                await SendNotification(notification);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
            }
        }
        public async Task NotifyPatientWithDoneLabBooking(int bookingID)
        {
            try
            {
                var booking = _db.LabServiceBookings.Find(bookingID);
                var patient = _db.Users.Find(booking.PatientID);
                var notification = new Notification()
                {
                    DataID = bookingID + "",
                    UserID = booking.PatientID,
                    Body = Messages.DoneLabBookingBody,
                    Title = Messages.DoneLabBookingTitle,
                    Type = (int)Consts.BookingsNotificationTypes.Rejected,
                };
                await SendNotification(notification);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion
        #region Nurse Bookings
        public async Task NotifyPatientWithAcceptedNurseBooking(int bookingID)
        {
            try
            {
                var booking = _db.NurseBookings.Find(bookingID);

                var patient = _db.Users.Find(booking.PatientID);
                var notification = new Notification()
                {
                    DataID = bookingID + "",
                    UserID = booking.PatientID,
                    Body = Messages.AcceptedNurseBookingBody,
                    Title = Messages.AcceptedNurseBookingTitle,
                    Type = (int)Consts.BookingsNotificationTypes.Accepted,

                };
                await SendNotification(notification);

            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
            }
        }
        public async Task NotifyPatientWithRejectedNurseBooking(int bookingID)
        {
            try
            {
                var booking = _db.NurseBookings.Find(bookingID);
                var patient = _db.Users.Find(booking.PatientID);
                var notification = new Notification()
                {
                    DataID = bookingID + "",
                    UserID = booking.PatientID,
                    Body = Messages.RejectedNurseBookingBody,
                    Title = Messages.RejectedNurseBookingTitle,
                    Type = (int)Consts.BookingsNotificationTypes.Rejected,
                };
                await SendNotification(notification);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
            }
        }
        public async Task NotifyPatientWithDoneNurseBooking(int bookingID)
        {
            try
            {
                var booking = _db.NurseBookings.Find(bookingID);
                var patient = _db.Users.Find(booking.PatientID);
                var notification = new Notification()
                {
                    DataID = bookingID + "",
                    UserID = booking.PatientID,
                    Body = Messages.DoneNurseBookingBody,
                    Title = Messages.DoneNurseBookingTitle,
                    Type = (int)Consts.BookingsNotificationTypes.Rejected,
                };
                await SendNotification(notification);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion
        public async Task SendNotification(Notification notification)
        {
            try
            {
                var user = _db.Users.Find(notification.UserID);
                var reciever = user.InstanceID;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _serverKey);
                    notification.Title = GetSetting(notification.Title + user.Language);
                    notification.Body = GetSetting(notification.Body + user.Language);
                    var model = new FireBaseNotificationModel()
                    {
                        to = reciever,
                        notification = new NotificationModel()
                        {
                            title = notification.Title,
                            body = notification.Body,
                        },
                        data = new
                        {
                            DataID = notification.DataID,
                            Title = notification.Title,
                            Info = notification.Body,
                            IsReaded = false,
                            Notification_type = notification.Type,
                            Date = notification.Time
                        }
                    };
                    var response = await client.PostAsJsonAsync("https://fcm.googleapis.com/fcm/send", model);
                    if (!response.IsSuccessStatusCode)
                    {
                        EXH.LogException(JsonSerializer.Serialize(response.Content), "NotificationSendError", MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);

                    }
                    EXH.LogException(await response.Content.ReadAsStringAsync(), "NotificationSendSuccess", MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);

                    SaveNotification(notification);
                }
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
            }
        }
        public string GetSetting(string name)
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
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return "NotSet";
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
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
            }
        }

        public void SaveNotification(string recieverUserID, string title, string body, string reciver)
        {
            try
            {
                Notification notification = new Notification()
                {
                    UserID = recieverUserID,
                    Body = body,
                    Time = DateTime.Now,
                    Title = title,
                };
                _db.Notifications.Add(notification);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
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
