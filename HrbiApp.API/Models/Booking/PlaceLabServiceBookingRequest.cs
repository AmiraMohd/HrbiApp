namespace HrbiApp.API.Models.Booking
{
    public class PlaceLabServiceBookingRequest
    {
        public string PatientId { get; set; }
        public int LabServiceId { get; set; }
        //public double Price { get; set; }
        public bool IsFromHome { get; set; }
    }
}
