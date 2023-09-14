
using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Ambulances
{
    public class CreateAmbulanceModel
    {
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Phone")]
        public string Phone { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Hospital")]
        public string Hospital { get; set; }
    }
}
