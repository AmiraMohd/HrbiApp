using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Doctors
{
    public class DoctorPayment
    {
        public int ID { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "TotalAmount")]
        public double TotalAmount { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "DoctorProfit")]
        public double DoctorProfit { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "SystemProfit")]
        public double SystemProfit { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "ProfitPercentage")]
        public double ProfitPercentage { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Status")]
        public string Status { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "CreateDate")]
        public string CreateDate { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "AcceptDate")]
        public string AcceptDate { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "SettledDate")]
        public string SettledDate { get; set; }
    }
}
