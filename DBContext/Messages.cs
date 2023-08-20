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

        public const string ThereIsUserWithSameName = "ThereIsDoctorWithSameName";
        public const string ThereIsUserWithSameEmail = "ThereIsDoctorWithSameEmail";
        public const string ThereIsUserWithSamePhone = "ThereIsDoctorWithSamePhone";

        public const string ThereIsSpecializationWithSameARName = "ThereIsSpecializationWithSameArabicName";
        public const string ThereIsSpecializationWithSameENName = "ThereIsSpecializationWithSameEnglishName";

        public const string ThereIsAnAmbulanceWithSamePhone = "ThereIsAnAmbulanceWithSamePhone";

        #endregion

        #region Notifications
        #region Lab Booking
        public const string AcceptedLabBookingTitle = "AcceptedLabBookingTitle";
        public const string AcceptedLabBookingBody = "AcceptedLabBookingBody";

        public const string RejectedLabBookingTitle = "RejectedLabBookingTitle";
        public const string RejectedLabBookingBody = "RejectedLabBookingBody";

        public const string DoneLabBookingTitle = "DoneLabBookingTitle";
        public const string DoneLabBookingBody = "DoneLabBookingBody";
        #endregion
         #region Nurse Booking
        public const string AcceptedNurseBookingTitle = "AcceptedNurseBookingTitle";
        public const string AcceptedNurseBookingBody = "AcceptedNurseBookingBody";

        public const string RejectedNurseBookingTitle = "RejectedNurseBookingTitle";
        public const string RejectedNurseBookingBody = "RejectedNurseBookingBody";

        public const string DoneNurseBookingTitle = "DoneNurseBookingTitle";
        public const string DoneNurseBookingBody = "DoneNurseBookingBody";
        #endregion
        #endregion
    }
}
