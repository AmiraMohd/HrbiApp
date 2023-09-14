
using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Doctors
{
    public class CreateDoctorModel
    {
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "[FullName")]
        public string FullName { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Phone")]
        public string Phone { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Email")]
        public string Email { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Specialization")]
        public int SpecializationID { get; set; }    
    }
}
