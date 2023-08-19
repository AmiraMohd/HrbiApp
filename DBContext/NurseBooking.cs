using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext
{
    public class NurseBooking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string PatientID { get; set; }
        [ForeignKey(nameof(PatientID))]
        public ApplicationUser Patient { get; set; }
        public int ServiceID { get; set; }
        [ForeignKey(nameof(ServiceID))]
        public NurseService NurseService { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime VisitTime { get; set; }
        public string Status { get; set; }
    }
}
