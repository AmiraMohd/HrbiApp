namespace HrbiApp.API.Models.Common
{
    public class LabServicesModel
    {
        public int Id { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public double Price { get; set; }
        public bool IsAvailableFromHome { get; set; }
    }
}
