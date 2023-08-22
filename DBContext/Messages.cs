using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext
{
    public class Messages
    {
        public const string ExceptionOccured = "ExceptionOccured";
        public const string Done = "Done";
        #region Validation Messages
        public const string BusinessError = "BusinessError";
        public const string ThereIsServiceWithSameARName = "ThereIsServiceWithSameArabicName";
        public const string ThereIsServiceWithSameENName = "ThereIsServiceWithSameEnglishName";
        #endregion

        #region Notifications
        public const string AcceptedLabBookingTitle = "AcceptedLabBookingTitle";
        public const string AcceptedLabBookingBody = "AcceptedLabBookingBody";

        public const string RejectedLabBookingTitle = "RejectedLabBookingTitle";
        public const string RejectedLabBookingBody = "RejectedLabBookingBody";

        public const string DoneLabBookingTitle = "DoneLabBookingTitle";
        public const string DoneLabBookingBody = "DoneLabBookingBody";
        #endregion

        #region Account Messages

        public const string NotValidPhone = "NotValidPhone";
        public const string NotValidPhoneAndOTP = "NotValidPhoneAndOTP";
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
        public const string NotActiveDoctorSpecialization = "NotActiveDoctorSpecialization";
        public const string NotActiveDoctorPosition = "NotActiveDoctorPosition";
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
