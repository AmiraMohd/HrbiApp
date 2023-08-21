using DBContext;
namespace HrbiApp.Web.Models.Doctors
{
    public class DoctorDetailsModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ApplicationUserID { get; set; }
        public string SpecializationNameAR { get; set; }
        public string SpecializationNameEN { get; set; }
        public string PositionNameAR { get; set; }
        public string PositionNameEN { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public string? Address { get; set; }
        public string? WorkHours { get; set; }
        public string Status { get; set; }
        public double? Price { get; set; }
        public string? AboutDoctor { get; set; }
        public DateTime? OpenTime { get; set; }
        public DateTime? CloseTime { get; set; }
        public IEnumerable<DoctorPayment>Payments { get; set; }=new List<DoctorPayment>();
    }
}
