using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext
{
    public class Nurse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Experiance { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        [ForeignKey(nameof(ApplicationUserID))]
        public ApplicationUser User { get; set; }
        public string ApplicationUserID { get; set; }
    }
}
