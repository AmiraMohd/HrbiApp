using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Areas.Common.Controllers;
using HrbiApp.Web.Models.Settings;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace HrbiApp.Web.Controllers
{
    public class SettingsController : WebController
    {

        public SettingsController(CoreServices cs, Validators validators, IStringLocalizer<SharedResource> sharedLocalizer) : base(cs, validators,sharedLocalizer)
        {
        }

        public IActionResult GetBankAccounts()
        {
            var getAccounts = CS.GetBankAccounts();
            if (!getAccounts.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            return View(getAccounts.Accounts);
        }

        public IActionResult CreateBankAccount()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateBankAccount(CreateBankAccountModel model)
        {
            var isValid = Validators.IsValidBankAccountToCreate(model);
            if (!isValid.Result)
            {
                Alert((isValid.Message), Consts.AdminNotificationType.error);
                return View(model);
            }
            var reuslt = CS.CreateBankAccount(model);
            if (!reuslt)
            {
                Alert((Messages.ExceptionOccured), Consts.AdminNotificationType.error);
                return View(model);
            }
            Alert(Messages.Done, Consts.AdminNotificationType.success);
            return RedirectToAction(nameof(GetBankAccounts));

        }
        public IActionResult UpdateBankAccount(int accountID)
        {
            if (!Validators.IsValidBankAccount(accountID))
            {
                Alert((Messages.ExceptionOccured), Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(GetBankAccounts));
            }
            var getAccount = CS.GetBankAccountToUpdate(accountID);
            if (!getAccount.Result)
            {
                Alert((Messages.ExceptionOccured), Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(GetBankAccounts));
            }
            return View(getAccount.Account);
        }
        [HttpPost]
        public IActionResult UpdateBankAccount(UpdateBankAccountModel model)
        {
            var isValid = Validators.IsValidBankAccountToUpdate(model);
            if (!isValid.Result)
            {
                Alert((Messages.ExceptionOccured), Consts.AdminNotificationType.error);
                ModelState.AddModelError(Messages.BusinessError, (isValid.Message));
                return View(model);
            }
            var result = CS.UpdateBankAccount(model);
            if (!result)
            {
                Alert((Messages.ExceptionOccured), Consts.AdminNotificationType.error);
                return View(model);
            }
            Alert((Messages.Done), Consts.AdminNotificationType.success);
            return RedirectToAction(nameof(GetBankAccounts));
        }
        public IActionResult DeleteBanckAccount(int accountID)
        {
            if (!Validators.IsValidBankAccount(accountID))
            {
                Alert((Messages.ExceptionOccured), Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(GetBankAccounts));
            }
            var result = CS.DeteleBankAccount(accountID);
            if (!result)
            {
                Alert((Messages.ExceptionOccured), Consts.AdminNotificationType.error);
            }
            else
            {
                Alert((Messages.Done), Consts.AdminNotificationType.success);
            }
            return RedirectToAction(nameof(GetBankAccounts));
        }
    }
}
