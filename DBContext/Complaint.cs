using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext
{
    public class Complaint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID {  get; set; }
        public string Text { get; set; }
        [MaxLength(15)]
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string ApplicationUserID { get; set; }
        [ForeignKey(nameof(ApplicationUserID))]
        public ApplicationUser User { get; set; }
    }
}
