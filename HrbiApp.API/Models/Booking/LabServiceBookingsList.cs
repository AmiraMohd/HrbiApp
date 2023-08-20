using DBContext;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrbiApp.API.Models.Booking
{
    public class LabServiceBookingsList
    {
        public string PatientID { get; set; }
        public int LabServiceID { get; set; }
        public bool IsFromHome { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
        public DateTime VisitTime { get; set; }
    }
}
