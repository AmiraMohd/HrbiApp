using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.NurseServices
{
    public class CreateNurseServiceModel
    {
        [MinLength(3)]
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "NameAR")]
        public string ServiceNameAR { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "NameEN")]
        [MinLength(3)]
        public string ServiceNameEN { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "ServicePrice")]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
    }
}
