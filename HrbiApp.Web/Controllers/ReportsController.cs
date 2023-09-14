using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Areas.Common.Controllers;
using HrbiApp.Web.Models.Reports;

using Microsoft.AspNetCore.Mvc;

namespace HrbiApp.Web.Controllers
{
    public class ReportsController : WebController
    {
        public ReportsController(CoreServices cs, Validators validators) : base(cs, validators)
        {
        }

        public IActionResult DoctorReport()
        {
            var getDoctors = CS.GetDoctorsToSelect();
            if (!getDoctors.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            @ViewData["Doctors"] = getDoctors.Doctors;
            return View();
        }
        [HttpPost]
        public IActionResult DoctorReport(int doctorID)
        {
            if (!Validators.IsValidDoctor(doctorID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return View();
            }
            var getReport = CS.GetDoctorReport(doctorID);
            if (!getReport.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            var getDoctors = CS.GetDoctorsToSelect();
            if (!getDoctors.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            @ViewData["Doctors"] = getDoctors.Doctors;
            return View(getReport.Report);
        }
        public IActionResult LabServiceReport()
        {
            var getReport = CS.GetLabBookingReport();
            if (!getReport.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            return View(getReport.Report);
        }
        public IActionResult NurseReport()
        {
            var getNurses = CS.GetNursesToSelect();
            if (!getNurses.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            ViewData["Nurses"] = getNurses.Nurses;
            return View();
        }
        [HttpPost]
        public IActionResult NurseReport(int nurseID)
        {
            if (!Validators.IsValidNurse(nurseID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(NurseReport));
            }
            var getReport = CS.GetNurseReport(nurseID);
            if (!getReport.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            var getNurses = CS.GetNursesToSelect();
            if (!getNurses.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            ViewData["Nurses"] = getNurses.Nurses;
            return View(getReport.Report);
        }

    }
}
