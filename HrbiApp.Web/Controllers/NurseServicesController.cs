using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Areas.Common.Controllers;
using HrbiApp.Web.Models.NurseServices;
using Microsoft.AspNetCore.Mvc;

namespace HrbiApp.Web.Controllers
{
    public class NurseServicesController : WebController
    {
        public NurseServicesController(CoreServices cs, Validators validators) : base(cs, validators)
        {
        }

        public IActionResult Index()
        {
            var getServices = CS.GetNurseServices();
            if (!getServices.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            return View(getServices.Services);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateNurseServiceModel model)
        {
            var isValid = Validators.IsValidNurseServiceToCreate(model);
            if (isValid.Result)
            {
                if (CS.CreateNurseService(model) == true)
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
            else
            {
                foreach (var error in isValid.Message.Split(","))
                    ModelState.AddModelError(Messages.BusinessError, error);
                return View(model);
            }
        }
        public IActionResult Update(int serviceID)
        {
            if (!Validators.IsValidNurseService(serviceID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var getService = CS.GetNurseServiceToUpdate(serviceID);
            if (!getService.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            return View(getService.Service);
        }
        [HttpPost]
        public IActionResult Update(UpdateNurseServiceModel model)
        {
            var result = CS.UpdateNurseService(model);
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
        public IActionResult DeActivateService(int serviceID)
        {
            if (!Validators.IsValidNurseService(serviceID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeNurseServiceStatus(serviceID, Consts.NotActive);
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
        public IActionResult ActivateService(int serviceID)
        {
            if (!Validators.IsValidNurseService(serviceID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeNurseServiceStatus(serviceID, Consts.Active);
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
