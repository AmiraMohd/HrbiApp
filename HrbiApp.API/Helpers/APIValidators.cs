using DBContext;
using DBContext.Enums;
using System.Reflection;

namespace HrbiApp.API.Helpers
{
    public class APIValidators
    {
        ApplicationDBContext _db;
        IConfiguration _configuration;
        ExceptionHandler _ex;

        public APIValidators(ApplicationDBContext db, IConfiguration configuration )
        {
            _db = db;
            _configuration = configuration;
           _ex = new ExceptionHandler(db);
        }



        public bool IsValidPhone(string phone)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(u => u.UserName == phone);
                if (user == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public bool IsActiveAccount(string phone)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(u => u.UserName == phone);
                if (user != null && user.Status == Consts.NotActive)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }



        #region Doctor Validators

        public bool IsValidDoctor(int id)
        {
            try
            {
                var user = _db.Doctors.FirstOrDefault(u => u.ID == id);
                if (user != null && user.Status == Consts.Active)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public bool IsValidSpecialization(int id)
        {
            try
            {
                var sp = _db.Specializations.FirstOrDefault(u => u.ID == id);
                if (sp != null && sp.Status == Consts.Active)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        #endregion

        #region Patient Validators

            public bool IsValidPatient(string id)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == id && u.AccountType == AccountType.Patient.ToString());
                if (user != null && user.Status == Consts.Active)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        #endregion

        public bool IsValidNurseService(int id)
        {
            try
            {
                var ns = _db.NurseServices.FirstOrDefault(u => u.ID == id);
                if (ns != null && ns.Status == Consts.Active)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public bool IsValidLabService(int id)
        {
            try
            {
                var ls = _db.LabServices.FirstOrDefault(u => u.ID == id);
                if (ls != null && ls.Status == Consts.Active)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
    }
}
