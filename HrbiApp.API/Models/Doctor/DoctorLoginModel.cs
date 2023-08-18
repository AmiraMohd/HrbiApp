using System.ComponentModel.DataAnnotations;

namespace HrbiApp.API.Models.Doctor
{
    public class DoctorLoginModel
    {
        public string PhoneNumber { get; set; }

        public string OTP { get; set; }
    }
}
