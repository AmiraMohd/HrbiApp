using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Areas.Common.Controllers;

using Microsoft.AspNetCore.Mvc;

namespace HrbiApp.Web.Controllers
{
    public class ServicesController : WebController
    {
        public ServicesController(CoreServices cs, Validators validators) : base(cs, validators)
        {
        }

        public IActionResult Index()
        {
            var getServices = CS.GetServices();
            if (!getServices.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            return View(getServices.Services);
        }
        public IActionResult DeActivate(int serviceID)
        {
            if (!Validators.IsValidService(serviceID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeServiceStatus(serviceID, Consts.NotActive);
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
        public IActionResult Activate(int serviceID)
        {
            if (!Validators.IsValidService(serviceID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeServiceStatus(serviceID, Consts.Active);
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
