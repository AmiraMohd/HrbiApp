using DBContext;

using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Doctors
{
    public class DoctorDetailsModel
    {
        public int ID { get; set; }
        [Display( Name = "FullName")]
        public string Name { get; set; }
        [Display( Name = "Phone")]
        public string Phone { get; set; }
        [Display( Name = "Email")]
        public string Email { get; set; }
        public string ApplicationUserID { get; set; }
        [Display( Name = "Specialization")]
        public string SpecializationNameAR { get; set; }
        [Display( Name = "Specialization")]
        public string SpecializationNameEN { get; set; }
        [Display( Name = "Position")]
        public string PositionNameAR { get; set; }
        [Display( Name = "Position")]
        public string PositionNameEN { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        [Display( Name = "Address")]
        public string? Address { get; set; }
        public string? WorkHours { get; set; }
        [Display( Name = "Status")]
        public string Status { get; set; }
        [Display( Name = "Price")]
        public double? Price { get; set; }
        [Display( Name = "AboutDoctor")]
        public string? AboutDoctor { get; set; }
        [Display( Name = "OpenTime")]
        public DateTime? OpenTime { get; set; }
        [Display( Name = "CloseTime")]
        public DateTime? CloseTime { get; set; }
        [Display( Name = "CloseTime")]
        public string Instructions { get; set; }
        [Display( Name = "Image")]
        public string Image { get; set; }
        public IEnumerable<DoctorPayment>Payments { get; set; }=new List<DoctorPayment>();
    }
}
