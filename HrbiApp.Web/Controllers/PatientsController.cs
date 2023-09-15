using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Areas.Common.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace HrbiApp.Web.Controllers
{
    public class PatientsController : WebController
    {
        public PatientsController(CoreServices cs, Validators validators, IStringLocalizer<SharedResource> sharedLocalizer) : base(cs, validators, sharedLocalizer)
        {
        }

        public IActionResult Index()
        {
            var getPatients = CS.GetPatients();
            if (!getPatients.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            return View(getPatients.Patients);
        }
        public IActionResult DeActivate(string patientID)
        {
            if (!Validators.IsValidUser(patientID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeUserStatus(patientID, Consts.NotActive);
            if (!result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            else
            {
                Alert(Messages.Done, Consts.AdminNotificationType.success);
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Activate(string patientID)
        {
            if (!Validators.IsValidUser(patientID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeUserStatus(patientID, Consts.Active);
            if (!result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            else
            {
                Alert(Messages.Done, Consts.AdminNotificationType.success);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
