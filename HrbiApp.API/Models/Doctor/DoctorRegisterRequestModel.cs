namespace HrbiApp.API.Models.Doctor
{
    public class DoctorRegisterRequestModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int DoctorSpecializationId { get; set; }
        public int DoctorPositionId { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
    }
}
