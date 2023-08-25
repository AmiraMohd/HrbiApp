using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext
{
    public class LabServiceBooking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [ForeignKey(nameof(PatientID))]
        public ApplicationUser Patient { get; set; }
        public string PatientID { get; set; }
        public string? Code { get; set;}
        [ForeignKey(nameof(LabServiceID))]
        public LabService LabService { get; set;}
        public int LabServiceID { get; set; }
        public bool IsFromHome { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
        public DateTime VisitTime { get; set; }

    }
}
