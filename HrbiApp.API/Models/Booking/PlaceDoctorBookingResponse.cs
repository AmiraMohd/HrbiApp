namespace HrbiApp.API.Models.Doctor
{
    public class PlaceDoctorBookingResponse
    {
        public int BookingId { get; set; }
        public int? DoctorId { get; set; }
        public string PatientId { get; set; }
    }
}
