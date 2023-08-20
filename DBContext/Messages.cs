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
    }
}
