namespace HrbiApp.API.Models.Booking
{
    public class PatientBookingsList
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorNumber { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime VisiteDate { get; set; }
    }
}
