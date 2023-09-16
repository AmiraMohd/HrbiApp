using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Areas.Common.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace HrbiApp.Web.Controllers
{
    public class ComplaintsController : WebController
    {
        public ComplaintsController(CoreServices cs, Validators validators, IStringLocalizer<SharedResource> sharedLocalizer) : base(cs, validators, sharedLocalizer)
        {
        }

        public IActionResult Index()
        {
            var getComplaints=CS.GetComplaints();
            if (!getComplaints.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            return View(getComplaints.Complaints);
        }
    }
}
