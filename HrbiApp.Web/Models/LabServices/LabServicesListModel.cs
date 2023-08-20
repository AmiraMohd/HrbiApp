namespace HrbiApp.Web.Models.LabServices
{
    public class LabServicesListModel
    {
        public int ID { get; set; }
        public string ServiceNameAR { get; set; }
        public string ServiceNameEN { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
        public bool IsAvilableFromHome { get; set; }
    }
}
