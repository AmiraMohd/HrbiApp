
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.Ambulances
{
    public class AmbulancesListModel
    {
        public int ID { get; set; }
        [Display(ResourceType =typeof(Resource.ResourceAr),Name ="Phone")]
        public string Phone { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Hospital")]
        public string Hospital { get; set; }
        [Display(ResourceType = typeof(Resource.ResourceAr), Name = "Status")]
        public string Status { get; set; }
    }
}
