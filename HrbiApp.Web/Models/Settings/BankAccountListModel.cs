

using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Settings
{
    public class BankAccountListModel
    {
        public int ID { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr),Name = "BankName")]
        public string BankName { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "AccountNumber")]
        public string AccountNumber { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "BranchName")]
        public string BranchName {  get; set; } 
    }
}
