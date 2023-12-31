﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext
{
    public class DoctorBooking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [ForeignKey(nameof(PatientID))]
        public ApplicationUser Patient { get; set; }
        public string PatientID { get; set; }
        [ForeignKey(nameof(DoctorID))]
        public Doctor? Doctor { get; set; }
        public int? DoctorID { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime VisiteDate { get; set; }

    }
}
