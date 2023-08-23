using DBContext;
using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Doctors
{
    public class DoctorDetailsModel
    {
        public int ID { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "FullName")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Phone")]
        public string Phone { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Email")]
        public string Email { get; set; }
        public string ApplicationUserID { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Specialization")]
        public string SpecializationNameAR { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Specialization")]
        public string SpecializationNameEN { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Position")]
        public string PositionNameAR { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Position")]
        public string PositionNameEN { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Address")]
        public string? Address { get; set; }
        public string? WorkHours { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Status")]
        public string Status { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Price")]
        public double? Price { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "AboutDoctor")]
        public string? AboutDoctor { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "OpenTime")]
        public DateTime? OpenTime { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "CloseTime")]
        public DateTime? CloseTime { get; set; }
        public IEnumerable<DoctorPayment>Payments { get; set; }=new List<DoctorPayment>();
    }
}
