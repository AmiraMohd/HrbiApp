namespace HrbiApp.Web.Models.Payments
{
    public class AllDoctorPaymentsListModel
    {
        public int ID { get; set; }
        public double TotalAmount { get; set; }
        public double DoctorProfit { get; set; }
        public double SystemProfit { get; set; }
        public double ProfitPercentage { get; set; }
        public string Status { get;set; }
        public string DoctorName { get; set; }
        public string DoctorPhone { get; set; }
        public string CreateDate { get; set; }
        public string AcceptDate { get; set; }
        public string SettledDate { get; set; }
    }
}
