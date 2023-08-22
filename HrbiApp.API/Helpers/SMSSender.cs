using DBContext;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System.Net;
using System.Reflection;

namespace HrbiApp.API.Helpers
{
    public class SMSSender
    {
        ExceptionHandler EXH;
        ApplicationDBContext _db;
        public SMSSender(ApplicationDBContext db)
        {
            _db = db;
            EXH = new ExceptionHandler(db);
        }

        public bool SendConfirmationSMS(string phone, string userID)
        {
            try
            {
                string OTP = new Random().Next(0, 999999).ToString("D6");
                var otp = _db.OTPs.FirstOrDefault(otp => otp.UserID == userID &&
                otp.Purpose == Consts.ConfirmationPurpose);
                if (otp == null)
                {
                    _db.OTPs.Add(new OTP()
                    {
                        Code = OTP,
                        Phone = phone,
                        UserID = userID,
                        Purpose = Consts.ConfirmationPurpose
                    });
                }
                else
                {
                    if (otp.Count > 50)
                    {
                        return false;
                    }
                    otp.Code = OTP;
                    otp.Count++;
                    _db.Entry(otp).State = EntityState.Modified;
                }
                _db.SaveChanges();
                string message = Messages.YourVerficationCodeIs + OTP;

                var result = SendSMS(message, phone);
                return result;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }

        }
        public bool SendForgetPasswordSMS(string phone, string userID)
        {
            try
            {
                string OTP = new Random().Next(0, 999999).ToString("D6");
                var otp = _db.OTPs.FirstOrDefault(otp => otp.UserID == userID &&
                otp.Purpose == Consts.ResetPurose);
                if (otp == null)
                {
                    _db.OTPs.Add(new OTP()
                    {
                        Code = OTP,
                        Phone = phone,
                        UserID = userID,
                        Purpose = Consts.ResetPurose,
                        Count = 1
                    });
                }
                else
                {
                    if (otp.Count > 50)
                    {
                        return false;
                    }
                    otp.Code = OTP;
                    otp.Count++;
                    _db.Entry(otp).State = EntityState.Modified;
                }
                _db.SaveChanges();
                string message = Messages.YourResetPasswordCodeIs + OTP;

                var result = SendSMS(message, phone);
                return result;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }

        }

        public bool SendSMS(string message, string phone)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var password = GetSettingValue(Consts.SMSPasswordSetting);
                var username = GetSettingValue(Consts.SMSUsernameSetting);
                var sender = GetSettingValue(Consts.SMSSenderSetting);

                var client = new RestClient(string.Format("http://www.airtel.sd/bulksms/webacc.aspx?user={3}&pwd={4}&nums={0}&smstext={1}&sender={2}",
                    "249" + phone.Substring(1),
                    message,
                    sender,
                    username,
                    password));
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                IRestResponse response = client.Execute(request);
                var responseString = response.Content.ToString();

                if (response.IsSuccessful)
                {
                    if (responseString == "OK")
                    {
                        return true;
                    }
                }
                EXH.LogException(responseString, "AirTelError", response.StatusCode.ToString(), response.ErrorMessage);
                return false;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }


        }

        public string GetSettingValue(string name)
        {
            try
            {
                var setting = _db.Settings.FirstOrDefault(s => s.Name == name);
                return setting.Value;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return "";
            }
        }
    }
}
