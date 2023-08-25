using DBContext;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrbiApp.API.Models.Doctor
{
    public class DoctorBookingPaymentModel
    {
        public int ID { get; set; }
        public int BookingID { get; set; }
        public double TotalAmount { get; set; }
        public DateTime CreateDate { get; set; }
        public string Status { get; set; }
        public DateTime? AcceptDate { get; set; }
        public DateTime? SettledDate { get; set; }
    }
}
