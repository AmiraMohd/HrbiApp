using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Complaints
{
    public class ComplaintsListModel
    {
        public int ID { get; set; }
        [Display(Name ="ComplaintText")]
        public string Text { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "CreateDate")]
        public string CreateDate { get; set; }
        [Display(Name = "FullName")]
        public string UserName { get; set; }
        [Display(Name = "Phone")]
        public string UserPhone { get; set; }
        [Display(Name = "Email")]
        public string UserEmail { get; set; }
    }
}
