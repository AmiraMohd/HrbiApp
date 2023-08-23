using HrbiApp.Web.Models.Doctors;
using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Payments
{
    public class AllDoctorPaymentsListModel:DoctorPayment
    {
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "DoctorName")]
        public string DoctorName { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "DoctorPhone")]
        public string DoctorPhone { get; set; }
        
    }
}
