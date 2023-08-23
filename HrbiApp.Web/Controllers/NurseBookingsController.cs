using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Areas.Common.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrbiApp.Web.Controllers
{
    [Authorize]
    public class NurseBookingsController : WebController
    {
        public NurseBookingsController(CoreServices cs, Validators validators) : base(cs, validators)
        {
        }

        public IActionResult Index()
        {
            var getBooking = CS.GetNurseServiceBookings();
            if (!getBooking.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            return View(getBooking.Bookings);
        }
        public IActionResult Accept(int bookingID, DateTime visitTime)
        {
            if (!Validators.IsValidNurseServiceBooking(bookingID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var accept = CS.AcceptNurseServiceBooking(bookingID, visitTime);
            if (!accept.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            else
            {
                Alert(Messages.Done, Consts.AdminNotificationType.success);
            }
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Reject(int bookingID)
        {
            if (!Validators.IsValidNurseServiceBooking(bookingID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var accept = CS.RejectNurseServiceBooking(bookingID);
            if (!accept.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            else
            {
                Alert(Messages.Done, Consts.AdminNotificationType.success);
            }
            return RedirectToAction(nameof(Index));

        }
        public IActionResult MakeDone(int bookingID)
        {
            if (!Validators.IsValidNurseServiceBooking(bookingID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var accept = CS.MakeNurseServiceBookingDone(bookingID);
            if (!accept.Result)
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
