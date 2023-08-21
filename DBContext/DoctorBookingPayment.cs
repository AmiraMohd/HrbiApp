using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext
{
    public class DoctorBookingPayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int BookingID { get; set; }
        [ForeignKey(nameof(BookingID))]
        public DoctorBooking DoctorBooking { get; set; }
        public double TotalAmount { get; set; }
        public double DoctorProfit { get; set; }
        public double ProfitPercentage { get; set; }
        public double SystemProfit { get; set; }
        public DateTime CreateDate { get; set; }
        public string Status { get; set; }
        public DateTime? AcceptDate { get; set; }
        public DateTime? SettledDate { get; set; }
    }
}
