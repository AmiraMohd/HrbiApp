using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Services
{
    public class ServicesListModel
    {
        public int ID { get; set; }
        [Display(Name = "NameAR",ResourceType =typeof(Resource.ResourceAr))]
        public string NameAR { get; set; }
        [Display(Name = "NameEN", ResourceType = typeof(Resource.ResourceAr))]
        public string NameEN { get; set; }
        [Display(Name = "Status", ResourceType = typeof(Resource.ResourceAr))]
        public string Status { get; set; }
    }
}
