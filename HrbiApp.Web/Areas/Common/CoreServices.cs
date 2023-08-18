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
        public (bool Result,LabServicesListModel Services) GetLabServices()
        {
            try
            {
                var services=_dbContext.la
            }
            catch (Exception ex)
            {
                EXH.LogException(ex);
                return (false,new LabServicesListModel());
            }
        }
        #endregion
    }
}
