using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Areas.Common.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HrbiApp.Web.Controllers
{
    public class LabServiceBookingsController : WebController
    {
        public LabServiceBookingsController(CoreServices cs, Validators validators) : base(cs, validators)
        {
        }

        public IActionResult Index()
        {
            var getBooking = CS.GetLabServiceBookings();
            if (!getBooking.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            return View(getBooking.Bookings);
        }
        public IActionResult Accept(int bookingID, DateTime visitTime)
        {
            if (!Validators.IsValidLabServiceBooking(bookingID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var accept = CS.AcceptLabServiceBooking(bookingID, visitTime);
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
            if (!Validators.IsValidLabServiceBooking(bookingID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var accept = CS.RejectLabServiceBooking(bookingID);
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
            if (!Validators.IsValidLabServiceBooking(bookingID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var accept = CS.MakeLabServiceBookingDone(bookingID);
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
