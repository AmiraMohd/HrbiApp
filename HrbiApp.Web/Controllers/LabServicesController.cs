using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Areas.Common.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HrbiApp.Web.Controllers
{
    public class LabServicesController : WebController
    {
        CoreServices CS;
        Validators Validators;
        public LabServicesController(CoreServices cs,Validators validators)
        {
            CS = cs;
            Validators = validators;
        }
        public IActionResult Index()
        {
            var getServices=CS.GetLabServices();
            if (!getServices.Result)
            {
                Alert(Messages.ExceptionOccured,Consts.AdminNotificationType.error);
            }
            return View(getServices.Services);
        }
    }
}
