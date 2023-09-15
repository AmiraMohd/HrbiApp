using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Areas.Common.Controllers;
using HrbiApp.Web.Models.Ambulances;
using HrbiApp.Web.Models.Specializations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace HrbiApp.Web.Controllers
{
    [Authorize]
    public class AmbulancesController : WebController
    {
        public AmbulancesController(CoreServices cs, Validators validators, IStringLocalizer<SharedResource> sharedLocalizer) : base(cs, validators, sharedLocalizer)
        {
        }

        public IActionResult Index()
        {
            var getAmbulances=CS.GetAmbulances();
            if (!getAmbulances.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            return View(getAmbulances.Ambulances);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateAmbulanceModel model) {
            var isValid = Validators.IaValidAmbulanceToCreate(model);
            if (!isValid.Result)
            {
                foreach (var error in isValid.Message.Split(","))
                    ModelState.AddModelError(Messages.BusinessError, error);
                return View(model);
            }
            var result = CS.CreateAmbulance(model);
            if (!result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return View(model);
            }
            Alert(Messages.Done, Consts.AdminNotificationType.success);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeActivate(int ambulanceID)
        {
            if (!Validators.IsValidAmbulance(ambulanceID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeAmbulanceStatus(ambulanceID, Consts.NotActive);
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
        public IActionResult Activate(int ambulanceID)
        {
            if (!Validators.IsValidAmbulance(ambulanceID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeAmbulanceStatus(ambulanceID, Consts.Active);
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
        public IActionResult Update(int ambulanceID)
        {
            if (!Validators.IsValidAmbulance(ambulanceID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var getAmbulanceToUpdate = CS.GetAmbulanceToUpdate(ambulanceID);
            if (!getAmbulanceToUpdate.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            return View(getAmbulanceToUpdate.Ambulance);
        }
        [HttpPost]
        public IActionResult Update(UpdateAmbulanceModel model)
        {
            var isValid = Validators.IaValidAmbulanceToUpdate(model);
            if (!isValid.Result)
            {
                foreach (var error in isValid.Message.Split(","))
                    ModelState.AddModelError(Messages.BusinessError, error);
                return View(model);
            }

            var result = CS.UpdateAmbulance(model);
            if (!result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return View(model);
            }
            Alert(Messages.Done, Consts.AdminNotificationType.success);
            return RedirectToAction(nameof(Index));

        }
    }
}
