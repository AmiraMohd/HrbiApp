﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext
{
    public class Consts
    {
        #region GeneralSettings
        public const string ConfirmationPurpose = "Confirmation";
        public const string ResetPurose = "Reset";
        public const string NotificationServerKey = "";

        public const string ContactUsPhoneSetting = "ContactUsPhone";
        public const string ContactUsEmailSetting = "ContactUsE,Email";
        public const string ContactUsLatSetting = "ContactUsLat";
        public const string ContactUsLonSetting = "ContactUsLon";
        #endregion
        #region Statuses


        #region GenaralStatuses
        public const string NotActive = "NotActive";
        public const string Active = "Active";
        public const string Deleted = "Deleted";
        public const string Pending = "Suspended";
        public const string Accepted = "Accepted";
        public const string Rejected = "Rejected";
        public const string Delayed = "Delayed";

        #endregion

        #region booking Statuses
        public const string New = "New";
        public const string OnProgress = "OnProgress";
        public const string Done = "Done";
        #endregion
        #region Payment Statuses
        public const string Settled = "Settled";

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
        public enum BookingsNotificationTypes
        {
            New=1,
            Accepted,
            Rejected,
            Done,
        }
        #endregion

        #region ClaimsName
        public const string UserIDClaimName = "UserID";
        public const string PhoneNumberClaimName = "PhoneNumber";
        #endregion
    }
}
