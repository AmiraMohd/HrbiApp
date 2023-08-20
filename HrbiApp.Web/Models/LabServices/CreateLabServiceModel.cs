using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.LabServices
{
    public class CreateLabServiceModel
    {
        [MinLength(3)]
        public string ServiceNameAR { get; set; }
        [MinLength(3)]
        public string ServiceNameEN { get; set; }
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        public bool IsAvilableFromHome { get; set; }
    }
}
