

using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Settings
{
    public class CreateBankAccountModel
    {
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "BankName")]
        public string BankName { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "AccountNumber")]
        [RegularExpression(pattern: @"^[0-9]")]
        public string AccountNumber { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "BranchName")]
        public string BranchName { get; set; }
    }
}
