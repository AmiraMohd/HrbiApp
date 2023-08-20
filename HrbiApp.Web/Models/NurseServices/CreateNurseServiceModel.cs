using System.ComponentModel.DataAnnotations;

namespace HrbiApp.Web.Models.NurseServices
{
    public class CreateNurseServiceModel
    {
        [MinLength(3)]
        public string ServiceNameAR { get; set; }
        [MinLength(3)]
        public string ServiceNameEN { get; set; }
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
    }
}
