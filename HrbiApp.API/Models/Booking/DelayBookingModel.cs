namespace HrbiApp.API.Models.Booking
{
    public class DelayBookingModel
    {
        public int BookingId { get; set; }
        public int DoctorId { get; set; }
        public DateTime VisitTime { get; set; }
    }
}
