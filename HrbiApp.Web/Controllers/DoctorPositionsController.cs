using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Areas.Common.Controllers;
using HrbiApp.Web.Models.DoctorPositions;
using Microsoft.AspNetCore.Mvc;

namespace HrbiApp.Web.Controllers
{
    public class DoctorPositionsController : WebController
    {
        public DoctorPositionsController(CoreServices cs, Validators validators) : base(cs, validators)
        {
        }

        public IActionResult Index()
        {
            var getServices = CS.GetDoctorPositions();
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
        public IActionResult Create(CreateDoctorPositionModel model)
        {
            var isValid = Validators.IsValidDoctorPositionToCreate(model);
            if (isValid.Result)
            {
                if (CS.CreateDoctorPosition(model) == true)
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
        public IActionResult Update(int positionId)
        {
            if (!Validators.IsValidDoctorPosition(positionId))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var getService = CS.GetDoctorPositionToUpdate(positionId);
            if (!getService.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            return View(getService.Service);
        }
        [HttpPost]
        public IActionResult Update(UpdateDoctorPositionModel model)
        {
            var result = CS.UpdateDoctorPosition(model);
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
        public IActionResult DeActivate(int positionId)
        {
            if (!Validators.IsValidDoctorPosition(positionId))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeDoctorPositionStatus(positionId, Consts.NotActive);
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
        public IActionResult Activate(int positionId)
        {
            if (!Validators.IsValidDoctorPosition(positionId))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeDoctorPositionStatus(positionId, Consts.Active);
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
