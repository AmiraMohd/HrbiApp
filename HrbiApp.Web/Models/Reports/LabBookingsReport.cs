

using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Reports
{
    public class LabBookingsReport
    {
        public int BookingID { get; set; }
        [Display(ResourceType =typeof(Resource.ResourceAr),Name = "PatientName")]
        public string PatientName { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr),Name = "PatientPhone")]
        public string PatientPhone { get; set;}
        [Display(ResourceType = typeof(Resource.ResourceAr),Name = "TotalAmount")]
        public double TotalAmount { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr),Name = "VisitTime")]
        public string VisitTime { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr),Name = "ServiceNameAR")]
        public string ServiceNameAR { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr),Name = "ServiceNameEN")]
        public string ServiceNameEN { get; set; }
    }
}
