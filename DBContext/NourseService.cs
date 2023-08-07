using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext
{
    public class NourseService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }

    }
}
