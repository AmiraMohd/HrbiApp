namespace HrbiApp.API.Models.Booking
{
    public class PlaceDoctorBookigRequest
    {
        public string PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
