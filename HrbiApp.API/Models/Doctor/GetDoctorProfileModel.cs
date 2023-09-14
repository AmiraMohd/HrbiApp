namespace HrbiApp.API.Models.Doctor
{
    public class GetDoctorProfileModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string AboutDoctor { get; set; }
        public string Instructions { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public DateTime? OpenTime { get; set; }
        public DateTime? CloseTime { get; set; }
        public double? TicketPrice { get; set; }
    }
}
