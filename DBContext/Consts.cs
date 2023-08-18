using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext
{
    public class Consts
    {
        #region GeneralSettings
        public const string ConfirmationPurose = "Confirmation";
        public const string ResetPurose = "Reset";
        public const string NotificationServerKey = "";

        #endregion
        #region Statuses


        #region GenaralStatuses
        public const string NotActive = "NotActive";
        public const string Active = "Active";
        public const string Deleted = "Deleted";
        public const string Pending = "Suspended";

        #endregion
        #endregion
        #region AccountTypes
        public const string DoctorAccountType = "Doctor";
        public const string PatientAccountType = "Patient";
        public const string LabAdminAccountType = "LabAdmin";
        public const string AdminAccountType = "Admin";
        public const string NourseAdminAccountType = "NourseAdmin";

        #endregion

        #region NotificationTypes
        public enum AdminNotificationType
        {
            error,
            success,
            warning,
            info
        }
        #endregion
    }
}
