namespace HrbiApp.Web.Models.Reports
{
    public class BookingsList
    {
        public int BookingID { get; set; }
        public string VisitTime { get; set; }
        public double Price { get; set; }
        public double DoctorProfit { get; set; }
        public double SystemProfit { get; set; }
        public string PatientName { get; set; }
        public string PatientPhone { get; set; }
        public string Status { get; set;}
        public string PaymentStatus { get; set;}
    }
}
