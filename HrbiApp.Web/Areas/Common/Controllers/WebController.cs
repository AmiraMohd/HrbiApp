using DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrbiApp.Web.Areas.Common.Controllers
{
    [Authorize]
    public class WebController : Controller
    {
        protected CoreServices CS;
        protected Validators Validators;
        public WebController(CoreServices cs, Validators validators)
        {
            CS = cs;
            Validators = validators;
        }
        protected void Alert(string message, Consts.AdminNotificationType notificationType)
        {
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
