
using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.NurseServices
{
    public class NurseListModel
    {
        public int ID { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "FullName")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Status")]
        public string Status { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Email")]
        public string Email { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Phone")]
        public string Phone { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Experience")]
        public string Experiance { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Address")]
        public string Address { get; set; }
    }
}
