namespace HrbiApp.Web.Models.Reports
{
    public class NurseReport
    {
        public string NurseName { get; set; }
        public List<NurseBookings> NurseBookings { get; set; } = new List<NurseBookings>();
         
    }
    public class NurseBookings
    {
        public string PatientName { get; set; }
        public string PatientPhone { get; set; }
        public string VisitTime { get; set; }
        public string ServiceNameAR { get; set; }
        public string ServiceNameEN { get; set; }
        public double Price { get; set; }
    }
}
