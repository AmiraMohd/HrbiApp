using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Areas.Common.Controllers;
using HrbiApp.Web.Models.NurseServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrbiApp.Web.Controllers
{
    [Authorize]
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
        public IActionResult NurseIndex()
        {
            var getNurses = CS.GetNurses();
            if (!getNurses.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            return View(getNurses.Nurses);
        }
        public IActionResult CreateNurse()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateNurse(CreateNurseModel model)
        {
            var isValid = Validators.IsValidNurseToCreate(model);
            if (!isValid.Result)
            {
                foreach (var error in isValid.Message.Split(","))
                    ModelState.AddModelError(Messages.BusinessError, error);
                return View(model);
            }
            var result = await CS.CreateNurse(model);
            if (!result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return View(model);
            }
            Alert(Messages.Done,Consts.AdminNotificationType.success);
            return RedirectToAction(nameof(NurseIndex));
        }
        public IActionResult UpdateNurse(int nurseID)
        {
            var getNurse = CS.GetNurseToUpdate(nurseID);
            if (!getNurse.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            return View(getNurse.Nurse);
        }
        [HttpPost]
        public IActionResult UpdateNurse(UpdateNurseModel model)
        {
            var isValid=Validators.IsValidNurseToUpdate(model);
            if (!isValid.Result)
            {
                ModelState.AddModelError(Messages.BusinessError, isValid.Message);
                return View(model);
            }
            var result=CS.UpdateNurse(model);
            if (!result)
            {
                Alert(Messages.ExceptionOccured,Consts.AdminNotificationType.error);
                return View(model);
            }
            Alert(Messages.Done,Consts.AdminNotificationType.success);
            return RedirectToAction(nameof(NurseIndex));
        }
        public IActionResult Activate(int nurseID)
        {
            if (!Validators.IsValidNurse(nurseID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeStatus(nurseID, Consts.Active);
            return RedirectToAction(nameof(NurseIndex));
        }
        public IActionResult DeActivate(int nurseID)
        {
            if (!Validators.IsValidNurse(nurseID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeStatus(nurseID, Consts.NotActive);
            return RedirectToAction(nameof(NurseIndex));
        }

    }
}
