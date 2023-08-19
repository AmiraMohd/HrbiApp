using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrbiApp.API.Models
{
    public class BaseResponse
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; } = "";
        public object Data { get; set; } = new object();
    }
}
