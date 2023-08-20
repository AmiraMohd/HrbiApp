using DBContext;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrbiApp.API.Models.Booking
{
    public class NurseBookingsList
    {
        public string PatientID { get; set; }
        public int ServiceID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime VisitTime { get; set; }
        public string Status { get; set; }
    }
}
