using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Areas.Common.Controllers;
using HrbiApp.Web.Models.Doctors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HrbiApp.Web.Controllers
{
    public class DoctorsController : WebController
    {
        public DoctorsController(CoreServices cs, Validators validators) : base(cs, validators)
        {
        }

        public IActionResult Index()
        {
            var getDoctors = CS.GetDoctors();
            if (!getDoctors.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            return View(getDoctors.Doctors);
        }

        public IActionResult Details(int doctorID)
        {
            if (!Validators.IsValidDoctor(doctorID))
            {
Alert(Messages.ExceptionOccured,Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var getDetails = CS.GetDoctorDetails(doctorID);
            if (!getDetails.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            return View(getDetails.Doctor);
        }
        public IActionResult Create()
        {
            var getSpecialization = CS.GetSpecializationsToSelect();
            if (!getSpecialization.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            ViewData["Specializations"] = getSpecialization.Specializations;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDoctorModel model)
        {
            var isValid = Validators.IsValidDoctorToCreate(model);
            if (!isValid.Result)
            {
                foreach (var error in isValid.Message.Split(","))
                    ModelState.AddModelError(Messages.BusinessError, error);

                var getSpecialization = CS.GetSpecializationsToSelect();
                if (!getSpecialization.Result)
                {
                    Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                }
                ViewData["Specializations"] = getSpecialization.Specializations;
                return View(model);
            }
            var result = await CS.CreateDoctor(model);
            if (!result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                var getSpecialization = CS.GetSpecializationsToSelect();
                if (!getSpecialization.Result)
                {
                    Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                }
                ViewData["Specializations"] = getSpecialization.Specializations;
                return View(model);
            }
            Alert(Messages.Done, Consts.AdminNotificationType.success);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeActivate(int doctorID)
        {
            if (!Validators.IsValidDoctor(doctorID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeDoctorStatus(doctorID, Consts.NotActive);
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
        public IActionResult Activate(int doctorID)
        {
            if (!Validators.IsValidDoctor(doctorID))
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var result = CS.ChangeDoctorStatus(doctorID, Consts.Active);
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
        public IActionResult Update(int doctorID)
        {
            var getDoctorToUpdate = CS.GetDoctorToUpdate(doctorID);
            if (!getDoctorToUpdate.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            var getSpecializations = CS.GetSpecializationsToSelect();
            if (!getSpecializations.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            ViewData["Specializations"] = getSpecializations.Specializations;
            return View(getDoctorToUpdate.Doctor);
        }
        [HttpPost]
        public IActionResult Update(UpdateDoctorModel model)
        {
            var isValid = Validators.IsValidDoctorToUpdate(model);
            if (!isValid.Result)
            {
                foreach (var error in isValid.Message.Split(","))
                    ModelState.AddModelError(Messages.BusinessError, error);
                var getSpecialization = CS.GetSpecializationsToSelect();
                if (!getSpecialization.Result)
                {
                    Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                }
                ViewData["Specializations"] = getSpecialization;

                return View(model);
            }

            var result = CS.UpdateDoctor(model);
            if (!result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                var getSpecialization = CS.GetSpecializationsToSelect();
                if (!getSpecialization.Result)
                {
                    Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                }
                ViewData["Specializations"] = getSpecialization;
                return View(model);
            }
            Alert(Messages.Done, Consts.AdminNotificationType.success);
            return RedirectToAction(nameof(Index));

        }
    }
}
