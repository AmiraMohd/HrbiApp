using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Settings
{
    public class BankAccountListModel
    {
        public int ID { get; set; }
        [Display(Name = "BankName")]
        public string BankName { get; set; }
        [Display( Name = "AccountNumber")]
        public string AccountNumber { get; set; }
        [Display( Name = "BranchName")]
        public string BranchName {  get; set; } 
    }
}
