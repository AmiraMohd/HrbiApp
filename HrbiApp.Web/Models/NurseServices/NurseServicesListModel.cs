
using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.NurseServices
{
    public class NurseServicesListModel
    {
        public int ID { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "ServiceName")]
        public string ServiceNameAR { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "ServiceName")]
        public string ServiceNameEN { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Status")]
        public string Status { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Price")]
        public double Price { get; set; }
    }
}
