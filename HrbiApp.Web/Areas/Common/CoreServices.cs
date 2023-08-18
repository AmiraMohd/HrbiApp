using DBContext;

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
        public void GetLabServices()
        {
            try
            {

            }
            catch (Exception ex)
            {
                EXH.LogException(ex);
            }
        }
        #endregion
    }
}
