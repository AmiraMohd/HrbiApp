using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DBContext
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; }
        public string AccountType { get; set; }
        public string Status { get;set; }
        public string Lanuage { get; set; }
        public string InstanceID { get; set; }
    }
}
