using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext
{
    public class ApplicationVersion
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int VersionCode { get; set; }
        public string VersionName { get; set; }
        public bool IsManditory { get; set; }
        public bool IsClientApp { get; set; }
    }
}
