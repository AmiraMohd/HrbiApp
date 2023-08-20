namespace HrbiApp.Web.Models.NurseBookings
{
    public class NurseBookingListModel
    {
        public int ID { get; set; }
        public string PatintName { get; set; }
        public DateTime VisitTime { get; set; }
        public int ServiceID { get; set; }
        public string ServiceNameAR { get; set; }
        public string ServiceNameEN { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
    }
}
