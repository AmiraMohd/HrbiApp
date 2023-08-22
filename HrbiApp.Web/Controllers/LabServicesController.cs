using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Areas.Common.Controllers;
using HrbiApp.Web.Models.LabServices;
using Microsoft.AspNetCore.Mvc;

namespace HrbiApp.Web.Controllers
{
    public class LabServicesController : WebController
    {
        
        public LabServicesController(CoreServices cs,Validators validators):base(cs,validators)
        {
            
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create (CreateLabServiceModel model)
        {
            var isValid=Validators.IsValidLabServiceToCreate(model);
            if(isValid.Result)
            {
                if (CS.CreateLabService(model) == true) {
                    Alert(Messages.Done, Consts.AdminNotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                    return View(model);
                }
            }
            else
            {
                foreach (var error in isValid.Message.Split(","))
                    ModelState.AddModelError(Messages.BusinessError, error);
                return View(model);
            }
        }
        public IActionResult Update(int serviceID)
        {
            if (!Validators.IsValidLabService(serviceID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var getService = CS.GetLabServiceToUpdate(serviceID);
            if (!getService.Result)
            {
                Alert(Messages.ExceptionOccured,Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            return View(getService.Service);
        }
        [HttpPost]
        public IActionResult Update(UpdateLabServiceModel model) {
            var result = CS.UpdateLabService(model);
            if (result)
            {
                Alert(Messages.Done, Consts.AdminNotificationType.success);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return View(model);
            }
        }
        public IActionResult DeActivateService(int serviceID) {
            if (!Validators.IsValidLabService(serviceID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeLabServiceStatus(serviceID, Consts.NotActive);
            if (!result) {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            else
            {
                Alert(Messages.Done, Consts.AdminNotificationType.success);
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ActivateService(int serviceID) {
            if (!Validators.IsValidLabService(serviceID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeLabServiceStatus(serviceID, Consts.Active);
            if (!result) {
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
