using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.DoctorPositions
{
    public class DoctorPositionsListModel
    {
        public int ID { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "NameAR")]
        public string NameAR { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "NameEN")]
        public string NameEN { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Status")]
        public string Status { get; set; }
    }
}
