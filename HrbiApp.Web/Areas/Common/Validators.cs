using DBContext;
using HrbiApp.Web.Models.LabServices;
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
                EXH.LogException(ex);
                return false;
            }
        }

        public (bool Result,string Message)IsValidLabServiceToCreate(CreateLabServiceModel model)
        {
            try
            {
                List<string> messages = new List<string>();
                var sameARName = _dbContext.LabServices.FirstOrDefault(s => s.NameAR == model.ServiceNameAR);
                if(sameARName != null) messages.Append(Messages.ThereIsServiceWithSameARName);

                var sameENName= _dbContext.LabServices.FirstOrDefault(s=>s.NameEN==model.ServiceNameEN);
                if (sameENName != null) messages.Append(Messages.ThereIsServiceWithSameENName);

                return (messages.Count==0 , string.Join(",",messages));
            }
            catch (Exception ex)
            {
                EXH.LogException(ex);
                return (false,Messages.ExceptionOccured);
            }
        }

        public (bool Result, string Message) IsValidLabServiceToUpdate(UpdateLabServiceModel model)
        {
            try
            {
                StringBuilder message = new StringBuilder();
                var sameARName = _dbContext.LabServices.FirstOrDefault(s => s.NameAR == model.ServiceNameAR&&s.ID!=model.ID);
                if (sameARName != null) message.Append(Messages.ThereIsServiceWithSameARName);

                var sameENName = _dbContext.LabServices.FirstOrDefault(s => s.NameEN == model.ServiceNameEN && s.ID != model.ID);
                if (sameENName != null) message.Append(Messages.ThereIsServiceWithSameENName);

                return (message == new StringBuilder(), message.ToString());
            }
            catch (Exception ex)
            {
                EXH.LogException(ex);
                return (false, Messages.ExceptionOccured);
            }
        }

        #endregion
        #region LabServiceBookings
        public bool IsValidLabServiceBooking(int bookingID)
        {
            try
            {
                var service = _dbContext.LabServiceBookings.Find(bookingID);
                return service != null;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex);
                return false;
            }
        }

        #endregion
    }
}
