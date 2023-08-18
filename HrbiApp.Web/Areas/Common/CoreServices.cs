using DBContext;
using HrbiApp.Web.Models.LabServices;

namespace HrbiApp.Web.Areas.Common
{
    public class CoreServices
    {
        ApplicationDBContext _dbContext;
        ExcptionHandler EXH;
        public CoreServices(ApplicationDBContext dbContext,ExcptionHandler exh) {
        _dbContext = dbContext;
            EXH = exh;
        }

        #region Lab Services
        public (bool Result,List<LabServicesListModel> Services) GetLabServices()
        {
            try
            {
                var services = _dbContext.LabServices.Select(s => new LabServicesListModel()
                {
                    Price = s.Price,
                    ServiceNameAR = s.NameAR,
                    ServiceNameEN = s.NameEN,
                    Status = s.Status,
                    IsAvilableFromHome=s.IsAvailableFromHome
                }).ToList() ;
                return (true, services);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex);
                return (false,new List<LabServicesListModel>());
            }
        }

        public bool CreateLabService(CreateLabServiceModel model)
        {
            try
            {
                var service= new LabService();
                service.NameAR = model.ServiceNameAR;
                service.NameEN = model.ServiceNameEN;
                service.Price = model.Price;
                service.IsAvailableFromHome = model.IsAvilableFromHome;
                _dbContext.LabServices.Add(service);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex);
                return false;
            }
        }
        public bool ChangeLabServiceStatus(int serviceID,string status)
        {
            try
            {
                var service = _dbContext.LabServices.Find(serviceID);
                service.Status=status;
                _dbContext.LabServices.Update(service);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex);
                return false;
            }
        }
        public bool UpdateLabService(UpdateLabServiceModel model)
        {
            try
            {
                var service=_dbContext.LabServices.FirstOrDefault(s=>s.ID==model.ID);
                service.NameAR=model.ServiceNameAR;
                service.NameEN=model.ServiceNameEN;
                service.Price=model.Price;
                _dbContext.LabServices.Update(service);
                _dbContext.SaveChanges();
                return true;
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
