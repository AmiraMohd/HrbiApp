using DBContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrbiApp.API.Models.Account
{
    public class ResetPassword
    {
        [Required]
        [RegularExpression(@"(^[0][1]\d{8}$)|(^[0][9]\d{8}$)", ErrorMessage = Messages.NotValidPhone)]
        public string Phone { get; set; }
        [Required]
        public string OTP { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
