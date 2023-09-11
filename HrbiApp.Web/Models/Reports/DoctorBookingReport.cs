namespace HrbiApp.Web.Models.Reports
{
    public class DoctorBookingReport
    {
        public string DoctorName { get; set; }
        public List<BookingsList> Bookings { get; set; }=new List<BookingsList>();
    }
}
