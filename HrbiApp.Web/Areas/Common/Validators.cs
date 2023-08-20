using DBContext;
using HrbiApp.Web.Models.Ambulances;
using HrbiApp.Web.Models.Doctors;
using HrbiApp.Web.Models.LabServices;
using HrbiApp.Web.Models.NurseServices;
using HrbiApp.Web.Models.Specializations;
using System.Reflection;
using System.Text;

namespace HrbiApp.Web.Areas.Common
{
    public class Validators
    {
        ExcptionHandler EXH;
        ApplicationDBContext _dbContext;
        public Validators(ExcptionHandler exh, ApplicationDBContext db)
        {
            EXH = exh;
            _dbContext = db;
        }
        #region LabServices

        
        public bool IsValidLabService(int serviceID)
        {
            try
            {
                var service =_dbContext.LabServices.FirstOrDefault(s=>s.ID==serviceID);
                return service != null;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public (bool Result,string Message)IsValidLabServiceToCreate(CreateLabServiceModel model)
        {
            try
            {
                List<string> messages = new List<string>();
                var sameARName = _dbContext.LabServices.FirstOrDefault(s => s.NameAR == model.ServiceNameAR);
                if(sameARName != null) messages.Add(Messages.ThereIsServiceWithSameARName);

                var sameENName= _dbContext.LabServices.FirstOrDefault(s=>s.NameEN==model.ServiceNameEN);
                if (sameENName != null) messages.Add(Messages.ThereIsServiceWithSameENName);

                return (messages.Count==0 , string.Join(",",messages));
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false,Messages.ExceptionOccured);
            }
        }

        public (bool Result, string Message) IsValidLabServiceToUpdate(UpdateLabServiceModel model)
        {
            try
            {
                List<string> messages = new ();
                var sameARName = _dbContext.LabServices.FirstOrDefault(s => s.NameAR == model.ServiceNameAR&&s.ID!=model.ID);
                if (sameARName != null) messages.Add(Messages.ThereIsServiceWithSameARName);

                var sameENName = _dbContext.LabServices.FirstOrDefault(s => s.NameEN == model.ServiceNameEN && s.ID != model.ID);
                if (sameENName != null) messages.Add(Messages.ThereIsServiceWithSameENName);

                return (messages.Count == 0, string.Join(",", messages));
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, Messages.ExceptionOccured);
            }
        }

        #endregion

        #region LabServiceBookings
        public bool IsValidLabServiceBooking(int bookingID)
        {
            try
            {
                var booking = _dbContext.LabServiceBookings.Find(bookingID);
                return booking != null;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        #endregion

        #region NurseServices


        public bool IsValidNurseService(int serviceID)
        {
            try
            {
                var service = _dbContext.NurseServices.FirstOrDefault(s => s.ID == serviceID);
                return service != null;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public (bool Result, string Message) IsValidNurseServiceToCreate(CreateNurseServiceModel model)
        {
            try
            {
                List<string> messages = new List<string>();
                var sameARName = _dbContext.NurseServices.FirstOrDefault(s => s.NameAR == model.ServiceNameAR);
                if (sameARName != null) messages.Add(Messages.ThereIsServiceWithSameARName);

                var sameENName = _dbContext.NurseServices.FirstOrDefault(s => s.NameEN == model.ServiceNameEN);
                if (sameENName != null) messages.Add(Messages.ThereIsServiceWithSameENName);

                return (messages.Count == 0, string.Join(",", messages));
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, Messages.ExceptionOccured);
            }
        }

        public (bool Result, string Message) IsValidNurseServiceToUpdate(UpdateNurseServiceModel model)
        {
            try
            {
                List<string> messages = new();
                var sameARName = _dbContext.NurseServices.FirstOrDefault(s => s.NameAR == model.ServiceNameAR && s.ID != model.ID);
                if (sameARName != null) messages.Add(Messages.ThereIsServiceWithSameARName);

                var sameENName = _dbContext.NurseServices.FirstOrDefault(s => s.NameEN == model.ServiceNameEN && s.ID != model.ID);
                if (sameENName != null) messages.Add(Messages.ThereIsServiceWithSameENName);

                return (messages.Count == 0, string.Join(",", messages));
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, Messages.ExceptionOccured);
            }
        }

        #endregion

        #region NurseServiceBookings
        public bool IsValidNurseServiceBooking(int bookingID)
        {
            try
            {
                var booking = _dbContext.NurseBookings.Find(bookingID);
                return booking != null;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        #endregion

        #region Doctors
        public (bool Result,string Message) IsValidDoctorToCreate(CreateDoctorModel model)
        {
            try
            {
                List<string> messages = new List<string>();
                var sameName = _dbContext.Users.FirstOrDefault(s => s.FullName == model.FullName);
                if (sameName != null) messages.Add(Messages.ThereIsUserWithSameName);

                var sameEmail = _dbContext.Users.FirstOrDefault(s => s.Email == model.Email);
                if (sameEmail != null) messages.Add(Messages.ThereIsUserWithSameEmail);

                var samePhone = _dbContext.Users.FirstOrDefault(s => s.PhoneNumber == model.Phone);
                if (samePhone != null) messages.Add(Messages.ThereIsUserWithSamePhone);

                return (messages.Count == 0, string.Join(",", messages));
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false,Messages.ExceptionOccured);
            }
        }
        public (bool Result, string Message) IsValidDoctorToUpdate(UpdateDoctorModel model)
        {
            try
            {
                var doctor=_dbContext.Doctors.Find(model.ID);
                var user=_dbContext.Users.Find(doctor.ApplicationUserID);

                List<string> messages = new List<string>();
                var sameName = _dbContext.Users.FirstOrDefault(s => s.FullName == model.FullName&&s.Id!=user.Id);
                if (sameName != null) messages.Add(Messages.ThereIsUserWithSameName);

                var sameEmail = _dbContext.Users.FirstOrDefault(s => s.Email == model.Email && s.Id != user.Id);
                if (sameEmail != null) messages.Add(Messages.ThereIsUserWithSameEmail);

                var samePhone = _dbContext.Users.FirstOrDefault(s => s.PhoneNumber == model.Phone && s.Id != user.Id);
                if (samePhone != null) messages.Add(Messages.ThereIsUserWithSamePhone);

                return (messages.Count == 0, string.Join(",", messages));
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, Messages.ExceptionOccured);
            }
        }

        public bool IsValidDoctor(int doctorID)
        {
            try
            {
                var doctor = _dbContext.Doctors.Find(doctorID);
                return doctor != null;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        #endregion

        #region Specializations
        public bool IsValidSpecialization(int specializationID)
        {
            try
            {
                var specialization = _dbContext.Specializations.Find(specializationID);
                return specialization != null;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public (bool Result, string Message) IsValidSpecializationToCreate(CreateSpecializationModel model)
        {
            try
            {
                List<string> messages = new List<string>();
                var sameARName = _dbContext.Specializations.FirstOrDefault(s => s.NameAR == model.NameAR);
                if (sameARName != null) messages.Add(Messages.ThereIsSpecializationWithSameARName);

                var sameENName = _dbContext.Specializations.FirstOrDefault(s => s.NameEN == model.NameEN);
                if (sameENName != null) messages.Add(Messages.ThereIsSpecializationWithSameENName);

                return (messages.Count == 0, string.Join(",", messages));
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, Messages.ExceptionOccured);
            }
        }

        public (bool Result, string Message) IsValidSpecializationToUpdate(UpdateSpecializationModel model)
        {
            try
            {
                List<string> messages = new List<string>();
                var sameARName = _dbContext.Specializations.FirstOrDefault(s => s.NameAR == model.NameAR && s.ID != model.ID);
                if (sameARName != null) messages.Add(Messages.ThereIsSpecializationWithSameARName);

                var sameENName = _dbContext.Specializations.FirstOrDefault(s => s.NameEN == model.NameEN && s.ID != model.ID);
                if (sameENName != null) messages.Add(Messages.ThereIsSpecializationWithSameENName);

                return (messages.Count == 0, string.Join(",", messages));
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, Messages.ExceptionOccured);
            }
        }

        #endregion

        #region Ambulance
        public bool IsValidAmbulance(int ambulanceID)
        {
            try
            {
                var ambulance = _dbContext.Ambulances.Find(ambulanceID);
                return ambulance != null;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public (bool Result,string Message) IaValidAmbulanceToCreate(CreateAmbulanceModel model)
        {
            try
            {
                List<string> messages = new List<string>();

                var samePhone = _dbContext.Ambulances.FirstOrDefault(a => a.Phone == model.Phone);
                if(samePhone!=null)messages.Add(Messages.ThereIsAnAmbulanceWithSamePhone);
                return (messages.Count == 0, string.Join(",", messages));
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false,Messages.ExceptionOccured);
            }
        }
        public (bool Result, string Message) IaValidAmbulanceToUpdate(UpdateAmbulanceModel model)
        {
            try
            {
                List<string> messages = new List<string>();

                var samePhone = _dbContext.Ambulances.FirstOrDefault(a => a.Phone == model.Phone&&a.ID!=model.ID);
                if (samePhone != null) messages.Add(Messages.ThereIsAnAmbulanceWithSamePhone);
                return (messages.Count == 0, string.Join(",", messages));
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false,Messages.ExceptionOccured);
            }
        }
        #endregion
    }
}
