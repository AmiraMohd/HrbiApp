using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext
{
    public class Responses
    {
        public const string ExceptionOccured = "ExceptionOccured";

        #region Account Messages

        public const string NotValidPhone = "NotValidPhone";
        public const string NotValidEmail = "NotValidEmail";
        public const string NotActiveAccount = "NotActiveAccount";
        public const string NotValidOTP = "NotValidOTP";
        public const string PhoneAlreadyExist = "AlreadyExist";
        public const string UserNotExist = "UserNotExist";
        public const string YourVerficationCodeIs = "Your Tazweed code : ";
        public const string YourResetPasswordCodeIs = "رمز إعادة تعيين كلمة المرور هو : ";
        public const string NotValidDoctor = "NotValidDoctor";
        public const string NotValidPatient = "NotValidPatient";
        public const string NotValidSpecialization = "NotValidSpecialization";
        public const string NotValidNurseService = "NotValidNurseService";
        public const string NotValidLabService = "NotValidLabService";
        #endregion

        #region Booking Messages
        public const string SuccessfulBooking = "CreatedSuccessfully";
        public const string BookingFailed = "BookingFailed";
        public const string BookingAlreadyRejected = "BookingAlreadyRejected";
        public const string BookingAlreadyAccepted = "BookingAlreadyAccepted";
        #endregion

        #region General

        public const string Failed = "Failed";
        #endregion

    }
}
