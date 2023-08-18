namespace HrbiApp.API.Models.Booking
{
    public class PlaceNurseBookingRequest
    {
        public string PatientId { get; set; }
        public int NurseServiceId { get; set; }
    }
}
