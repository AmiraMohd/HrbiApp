
using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Specializations
{
    public class CreateSpecializationModel
    {
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "NameAR")]
        public string NameAR { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "NameEN")]
        public string NameEN { get; set; }
    }
}
