using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.LabBooking
{
    public class LabBookingListModel
    {
        public int ID { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "PatintName")]
        public string PatintName { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "VisitTime")]
        public DateTime VisitTime { get; set;}
        public int ServiceID { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "ServiceName")]
        public string ServiceNameAR { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "ServiceName")]
        public string ServiceNameEN { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "IsFromHome")]
        public bool IsFromHome { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Status")]
        public string Status { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Price")]
        public double Price { get; set; }
    }
}
