namespace HrbiApp.API.Models.Doctor
{
    public class DoctorProfileModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string AboutDoctor { get; set; }
        public DateTime? OpenTime { get; set; }
        public DateTime? CloseTime { get; set; }
        public double? TicketPrice { get; set; }
    }
}
