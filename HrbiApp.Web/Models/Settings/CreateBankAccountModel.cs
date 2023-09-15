
using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Settings
{
    public class CreateBankAccountModel
    {
        [Display( Name = "BankName")]
        public string BankName { get; set; }
        [Display( Name = "AccountNumber")]
        [RegularExpression (pattern: @"^\d*$", ErrorMessage= "OnlyNumbersAllowed")]
        [Required (ErrorMessage= "OnlyNumbersAllowed")]
        public string AccountNumber { get; set; }
        [Display( Name = "BranchName")]
        public string BranchName { get; set; }
    }
}
