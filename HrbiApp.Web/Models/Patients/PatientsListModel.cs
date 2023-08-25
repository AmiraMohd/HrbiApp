using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Patients
{
    public class PatientsListModel
    {
        public string ID { get; set; }
        [Display(Name = "FullName",ResourceType =typeof(Resource.ResourceAr))]
        public string Name { get; set; }
        [Display(Name = "Phone", ResourceType = typeof(Resource.ResourceAr))]
        public string Phone { get; set; }
        [Display(Name = "Email", ResourceType = typeof(Resource.ResourceAr))]
        public string Email { get; set; }
        [Display(Name = "Status", ResourceType = typeof(Resource.ResourceAr))]
        public string Status { get; set; }
    }
}
