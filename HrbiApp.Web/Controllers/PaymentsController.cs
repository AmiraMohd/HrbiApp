using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Areas.Common.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrbiApp.Web.Controllers
{
    [Authorize]
    public class PaymentsController : WebController
    {
        public PaymentsController(CoreServices cs, Validators validators) : base(cs, validators)
        {
        }

        public IActionResult AllDoctorPayments()
        {
            var getPayments=CS.GetAllDoctorPayments();
            if (!getPayments.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            return View(getPayments.Payments);
        }
        public IActionResult AcceptDoctorBookingPayment(int paymentID,string returnURL="")
        {
            if(!Validators.IsValidPaymentToAccept(paymentID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                if(returnURL =="")
                return RedirectToAction(nameof(AllDoctorPayments));
                else
                {
                    return Redirect(returnURL);
                }
            }
            var result=CS.AcceptDoctorBookingPayment(paymentID, Consts.Accepted);
            if (!result)
            {
                if (returnURL == "")
                    return RedirectToAction(nameof(AllDoctorPayments));
                else
                {
                    return Redirect(returnURL);
                }
            }
            Alert(Messages.Done, Consts.AdminNotificationType.success);
            if (returnURL == "")
                return RedirectToAction(nameof(AllDoctorPayments));
            else
            {
                return Redirect(returnURL);
            }
        }

        public IActionResult SettleDoctorBookingPayment(int paymentID, string returnURL = "")
        {
            if (!Validators.IsValidPaymentToSettle(paymentID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                if (returnURL == "")
                    return RedirectToAction(nameof(AllDoctorPayments));
                else
                {
                    return Redirect(returnURL);
                }
            }
            var result = CS.SettleDoctorBookingPayment(paymentID, Consts.Settled);
            if (!result)
            {
                if (returnURL == "")
                    return RedirectToAction(nameof(AllDoctorPayments));
                else
                {
                    return Redirect(returnURL);
                }
            }
            Alert(Messages.Done, Consts.AdminNotificationType.success);
            if (returnURL == "")
                return RedirectToAction(nameof(AllDoctorPayments));
            else
            {
                return Redirect(returnURL);
            }
        }

    }
}
