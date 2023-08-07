using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DBContext
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string UserID { get; set; }
        public string DataID { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }=DateTime.Now;
        public int Type { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
