
using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Doctors
{
    public class DoctorsListModel
    {
        public int ID { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "FullName")]
        public string FullName { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Email")]
        public string Email { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Phone")]
        public string Phone { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Status")]
        public string Status { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Price")]
        public double Price { get; set; }
    }
}
