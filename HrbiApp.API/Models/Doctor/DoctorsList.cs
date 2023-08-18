namespace HrbiApp.API.Models.Doctor
{
    public class DoctorsList
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
        public string AboutDoctor { get; set; }
        public double Price { get; set; }
    }
}
