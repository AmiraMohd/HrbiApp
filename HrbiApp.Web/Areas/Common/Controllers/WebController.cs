using DBContext;


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Resources;

namespace HrbiApp.Web.Areas.Common.Controllers
{
    [Authorize]
    public class WebController : Controller
    {
        protected CoreServices CS;
        protected Validators Validators;
        //protected LanguageService _localization;
        protected readonly IStringLocalizer<SharedResource> _sharedLocalizer;


        public WebController(CoreServices cs, Validators validators, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            CS = cs;
            Validators = validators;
            _sharedLocalizer = sharedLocalizer;

        }
        protected void Alert(string message, Consts.AdminNotificationType notificationType)
        {
            message = _sharedLocalizer[message];
            string msg = "";
            switch (notificationType)
            {
                case Consts.AdminNotificationType.success:
                    //msg = " title='تم';message='" + message + "';type='" + notificationType.ToString().ToLower() + "'; swal({title: title,text:message,type: type});";
                    msg = " title='تم';message='" + message + "';type='" + notificationType.ToString().ToLower() + "'; sweetAlert( title,message,type);";
                    break;
                case Consts.AdminNotificationType.error:
                    //msg = "swal({title: " + "خطأ" + ",text:" + message + ",type: " + notificationType.ToString().ToLower() + "});";
                    //msg = " title='خطأ';message='" + message + "';type='" + notificationType.ToString().ToLower() + "'; swal({title: title,text:message,type: type});";
                    msg = " title='خطأ';message='" + message + "';type='" + notificationType.ToString().ToLower() + "'; sweetAlert( title,message,type);";

                    break;
                case Consts.AdminNotificationType.info:
                    //msg = " swal({title: " + "تنبيه" + ",text:" + message + ",type: " + notificationType.ToString().ToLower() + "});";
                    //msg = " title='تنبيه';message='" + message + "';type='" + notificationType.ToString().ToLower() + "'; swal({title: title,text:message,type: type});";
                    msg = " title='تنبيه';message='" + message + "';type='" + notificationType.ToString().ToLower() + "'; swal({title: title,text:message,type: type});";

                    break;
                case Consts.AdminNotificationType.warning:
                    //msg = "swal({title: " + "تحذير" + ",text:" + message + ",type: " + notificationType.ToString().ToLower() + "});";
                    //msg = " title='تحذير';message='" + message + "';type='" + notificationType.ToString().ToLower() + "'; swal({title: title,text:message,type: type});";
                    msg = " title='تحذير';message='" + message + "';type='" + notificationType.ToString().ToLower() + "'; swal({title: title,text:message,type: type});";

                    break;
            }
            //var msg = "sweetAlert('" + notificationType.ToString().ToUpper() + "', '" + message + "','" + notificationType + "')" + "";
            TempData["notification"] = msg;
        }

    }
}
