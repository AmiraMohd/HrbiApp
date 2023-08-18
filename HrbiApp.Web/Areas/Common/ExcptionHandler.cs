
using DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrbiApp.Web.Areas.Common
{
    public class ExcptionHandler
    {
        ApplicationDBContext _db;
        //UnitOfWork _unitOfWork;
        public ExcptionHandler(ApplicationDBContext db)
        {
            _db = db;
            //_unitOfWork = new UnitOfWork(db);

        }
        public void LogException(Exception exception, string className = "", string methodName = "")
        {
            try
            {
                ExceptionLog exceptionLog = new ExceptionLog
                {
                    ClassName = className,
                    MethodName = methodName,
                    Message = exception?.Message,
                    DateTime = DateTime.Now,
                    InnerException = (exception?.InnerException?.Message) == null ? "" : exception?.InnerException?.Message,
                    StackTrace = exception?.StackTrace
                };
                //_unitOfWork.ExceptionLogs.Add(new Data.Entities.ExceptionLog() { });
                //_unitOfWork.Complete();
                _db.Add<ExceptionLog>(exceptionLog);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
        public void LogException(string body, string title, string className = "", string methodName = "")
        {
            try
            {
                ExceptionLog exceptionLog = new ExceptionLog
                {
                    ClassName = className,
                    MethodName = methodName,
                    Message = body,
                    DateTime = DateTime.Now,
                    InnerException = title,
                    StackTrace = ""
                };
                //_unitOfWork.ExceptionLogs.Add(new Data.Entities.ExceptionLog() { });
                //_unitOfWork.Complete();
                _db.ExceptionLogs.Add(exceptionLog);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }


    }

}
