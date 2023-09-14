
using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.NurseServices
{
    public class CreateNurseModel
    {
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "FullName")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Status")]
        public string Status { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Email")]
        public string Email { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Phone")]
        public string Phone { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Experience")]
        public string Experience { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Address")]
        public string Address { get; set; }
    }
}
