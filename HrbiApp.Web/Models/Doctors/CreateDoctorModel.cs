namespace HrbiApp.Web.Models.Doctors
{
    public class CreateDoctorModel
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int SpecializationID { get; set; }    
    }
}
