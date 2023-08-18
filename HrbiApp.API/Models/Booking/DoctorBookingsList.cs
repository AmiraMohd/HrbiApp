namespace HrbiApp.API.Models.Booking
{
    public class DoctorBookingsList
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientNumber { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime VisiteDate { get; set; }
    }
}
