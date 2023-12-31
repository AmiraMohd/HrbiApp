﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext
{
    public class Doctor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ApplicationUserID { get; set; }
        [ForeignKey(nameof(ApplicationUserID))]
        public ApplicationUser User { get; set; }
        [ForeignKey(nameof(SpecializationID))]
        public Specialization Specialization { get; set; }
        public int SpecializationID { get; set; }

        [ForeignKey(nameof(PositionID))]
        public DoctorPosition? Position { get; set; }
        public int? PositionID { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public string? Address { get; set; }
        public string? WorkHours { get; set; }
        public string Status { get; set; } = Consts.NotActive;
        public double? Price { get; set; }
        public string? AboutDoctor { get; set; }
        public DateTime? OpenTime { get; set; }
        public DateTime? CloseTime { get; set; }
        public string Instructions { get; set; } = "";
        public string? Image { get; set; }
    }
}
