using DBContext;
using HrbiApp.Web.Models.LabBooking;
using HrbiApp.Web.Models.LabServices;
using Microsoft.EntityFrameworkCore;

namespace HrbiApp.Web.Areas.Common
{
    public class CoreServices
    {
        ApplicationDBContext _dbContext;
        ExcptionHandler EXH;
        NotificationCenter NC;
        public CoreServices(ApplicationDBContext dbContext, ExcptionHandler exh,NotificationCenter nc)
        {
            _dbContext = dbContext;
            EXH = exh;
            NC = nc;
        }

        #region Lab Services
        public (bool Result, List<LabServicesListModel> Services) GetLabServices()
        {
            try
            {
                var services = _dbContext.LabServices.Select(s => new LabServicesListModel()
                {
                    ID = s.ID,
                    Price = s.Price,
                    ServiceNameAR = s.NameAR,
                    ServiceNameEN = s.NameEN,
                    Status = s.Status,
                    IsAvilableFromHome = s.IsAvailableFromHome
                }).ToList();
                return (true, services);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex);
                return (false, new List<LabServicesListModel>());
            }
        }

        public bool CreateLabService(CreateLabServiceModel model)
        {
            try
            {
                var service = new LabService();
                service.NameAR = model.ServiceNameAR;
                service.NameEN = model.ServiceNameEN;
                service.Price = model.Price;
                service.Status = Consts.NotActive;
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
        public bool ChangeLabServiceStatus(int serviceID, string status)
        {
            try
            {
                var service = _dbContext.LabServices.Find(serviceID);
                service.Status = status;
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

        public (bool Result, UpdateLabServiceModel Service) GetLabServiceToUpdate(int serviceID)
        {
            try
            {
                var service = _dbContext.LabServices.Find(serviceID);
                var editModel = new UpdateLabServiceModel()
                {
                    ID = serviceID,
                    ServiceNameAR = service.NameAR,
                    ServiceNameEN = service.NameEN,
                    IsAvilableFromHome = service.IsAvailableFromHome,
                    Price = service.Price,
                };
                return (true, editModel);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex);
                return (false, new UpdateLabServiceModel());
            }
        }
        public bool UpdateLabService(UpdateLabServiceModel model)
        {
            try
            {
                var service = _dbContext.LabServices.FirstOrDefault(s => s.ID == model.ID);
                service.NameAR = model.ServiceNameAR;
                service.NameEN = model.ServiceNameEN;
                service.Price = model.Price;
                service.IsAvailableFromHome = model.IsAvilableFromHome;
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

        #region Lab Bookings
        public (bool Result, List<LabBookingListModel> Bookings) GetLabServiceBookings()
        {
            try
            {
                var bookings = _dbContext.LabServiceBookings.Where(b => b.Status != Consts.Deleted)
                    .Include(b => b.LabService)
                    .Include(b => b.Patient)
                    .Select(b => new LabBookingListModel()
                    {
                        Status = b.Status,
                        ServiceID = b.LabServiceID,
                        ID = b.ID,
                        ServiceNameAR = b.LabService.NameAR,
                        ServiceNameEN = b.LabService.NameEN,
                        IsFromHome = b.IsFromHome,
                        PatintName = b.Patient.FullName,
                        Price = b.Price,
                        VisitTime = b.VisitTime,
                    }
                ).ToList();
                return (true, bookings);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex);
                return (false, new());
            }
        }

        public async Task<bool> AcceptLabServiceBooking(int bookingID,DateTime vistTime)
        {
            try
            {
                var booking = _dbContext.LabServiceBookings.Find(bookingID);
                booking.Status = Consts.Accepted;
                booking.VisitTime = vistTime;
                _dbContext.LabServiceBookings.Update(booking);
                _dbContext.SaveChanges();
                await NC.NotifyPatientWithAcceptedLabBooking(bookingID);
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex);
                return false;
            }
        }
        public async Task<bool> RejectLabServiceBooking(int bookingID)
        {
            try
            {
                var booking = _dbContext.LabServiceBookings.Find(bookingID);
                booking.Status = Consts.Rejected;
                _dbContext.LabServiceBookings.Update(booking);
                _dbContext.SaveChanges();
                await NC.NotifyPatientWithRejectedLabBooking(bookingID);
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex);
                return false;
            }
        }
        public async Task<bool> MakeLabServiceBookingDone(int bookingID)
        {
            try
            {
                var booking = _dbContext.LabServiceBookings.Find(bookingID);
                booking.Status = Consts.Done;
                _dbContext.LabServiceBookings.Update(booking);
                _dbContext.SaveChanges();
                await NC.NotifyPatientWithDoneLabBooking(bookingID);
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
