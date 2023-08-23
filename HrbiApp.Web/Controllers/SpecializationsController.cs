using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Areas.Common.Controllers;
using HrbiApp.Web.Models.Doctors;
using HrbiApp.Web.Models.Specializations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrbiApp.Web.Controllers
{
    [Authorize]
    public class SpecializationsController : WebController
    {
        public SpecializationsController(CoreServices cs, Validators validators) : base(cs, validators)
        {
        }

        public IActionResult Index()
        {
            var getSpecializations=CS.GetSpecializations();
            if (!getSpecializations.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            return View(getSpecializations.Specializations);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateSpecializationModel model)
        {
            var isValid = Validators.IsValidSpecializationToCreate(model);
            if (!isValid.Result)
            {
                foreach (var error in isValid.Message.Split(","))
                    ModelState.AddModelError(Messages.BusinessError, error);
                return View(model);
            }
            var result = CS.CreateSpecialization(model);
            if (!result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return View(model);
            }
            Alert(Messages.Done, Consts.AdminNotificationType.success);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeActivate(int specializationID)
        {
            if (!Validators.IsValidSpecialization(specializationID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeSpecializationStatus(specializationID, Consts.NotActive);
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
        public IActionResult Activate(int specializationID)
        {
            if (!Validators.IsValidSpecialization(specializationID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeSpecializationStatus(specializationID, Consts.Active);
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
        public IActionResult Update(int specializationID)
        {
            if (!Validators.IsValidSpecialization(specializationID))
            {
                Alert(Messages.ExceptionOccured,Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var getSpecializationToUpdate = CS.GetSpecializationToUpdate(specializationID);
            if (!getSpecializationToUpdate.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
                        return View(getSpecializationToUpdate.Specialization);
        }
        [HttpPost]
        public IActionResult Update(UpdateSpecializationModel model)
        {
            var isValid = Validators.IsValidSpecializationToUpdate(model);
            if (!isValid.Result)
            {
                foreach (var error in isValid.Message.Split(","))
                    ModelState.AddModelError(Messages.BusinessError, error);
                return View(model);
            }

            var result = CS.UpdateSpecialization(model);
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
