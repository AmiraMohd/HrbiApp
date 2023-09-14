using DBContext;
using HrbiApp.Web.Models.Ambulances;
using HrbiApp.Web.Models.Doctors;
using HrbiApp.Web.Models.LabBooking;
using HrbiApp.Web.Models.LabServices;
using HrbiApp.Web.Models.NurseBookings;
using HrbiApp.Web.Models.NurseServices;
using HrbiApp.Web.Models.DoctorPositions;
using HrbiApp.Web.Models.Specializations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Reflection;
using HrbiApp.Web.Models.Payments;
using HrbiApp.Web.Models.Services;
using HrbiApp.Web.Models.Patients;
using HrbiApp.Web.Models.Reports;
using HrbiApp.Web.Models.Settings;

namespace HrbiApp.Web.Areas.Common
{
    public class CoreServices
    {
        ApplicationDBContext _dbContext;
        ExcptionHandler EXH;
        NotificationCenter NC;
        UserManager<ApplicationUser> _userManager;
        public CoreServices(ApplicationDBContext dbContext, ExcptionHandler exh, NotificationCenter nc, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            EXH = exh;
            NC = nc;
            _userManager = userManager;

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
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
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
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
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
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
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
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
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
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
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
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }

        public async Task<bool> AcceptLabServiceBooking(int bookingID, DateTime vistTime)
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
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
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
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
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
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        #endregion

        #region Nurse Services
        public (bool Result, List<NurseServicesListModel> Services) GetNurseServices()
        {
            try
            {
                var services = _dbContext.NurseServices.Select(s => new NurseServicesListModel()
                {
                    ID = s.ID,
                    Price = s.Price,
                    ServiceNameAR = s.NameAR,
                    ServiceNameEN = s.NameEN,
                    Status = s.Status,
                }).ToList();
                return (true, services);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new List<NurseServicesListModel>());
            }
        }

        public bool CreateNurseService(CreateNurseServiceModel model)
        {
            try
            {
                var service = new NurseService();
                service.NameAR = model.ServiceNameAR;
                service.NameEN = model.ServiceNameEN;
                service.Price = model.Price;
                service.Status = Consts.NotActive;
                _dbContext.NurseServices.Add(service);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public bool ChangeNurseServiceStatus(int serviceID, string status)
        {
            try
            {
                var service = _dbContext.NurseServices.Find(serviceID);
                service.Status = status;
                _dbContext.NurseServices.Update(service);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public (bool Result, UpdateNurseServiceModel Service) GetNurseServiceToUpdate(int serviceID)
        {
            try
            {
                var service = _dbContext.NurseServices.Find(serviceID);
                var editModel = new UpdateNurseServiceModel()
                {
                    ID = serviceID,
                    ServiceNameAR = service.NameAR,
                    ServiceNameEN = service.NameEN,
                    Price = service.Price,
                };
                return (true, editModel);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new UpdateNurseServiceModel());
            }
        }
        public bool UpdateNurseService(UpdateNurseServiceModel model)
        {
            try
            {
                var service = _dbContext.NurseServices.FirstOrDefault(s => s.ID == model.ID);
                service.NameAR = model.ServiceNameAR;
                service.NameEN = model.ServiceNameEN;
                service.Price = model.Price;
                _dbContext.NurseServices.Update(service);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public (bool Result, List<NurseListModel> Nurses) GetNurses()
        {
            try
            {
                var nurses = _dbContext.Nurses.Include(n => n.User)
                    .Select(n => new NurseListModel()
                    {
                        Address = n.Address,
                        Status = n.Status,
                        Email = n.User.Email,
                        Experiance = n.Experiance,
                        ID = n.ID,
                        Name = n.User.FullName,
                        Phone = n.User.PhoneNumber
                    }).ToList();
                return (true, nurses);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }
        public async Task<bool> CreateNurse(CreateNurseModel model)
        {
            try
            {
                var user = new ApplicationUser()
                {
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    FullName = model.Name,
                    UserName = model.Phone,
                    AccountType = Consts.NurseAccountType,
                    Status = Consts.NotActive
                };
                var reuslt = await _userManager.CreateAsync(user, "123456");
                if (reuslt.Succeeded)
                {
                    Nurse nurse = new Nurse()
                    {
                        Status = Consts.NotActive,
                        Address = model.Address,
                        ApplicationUserID = user.Id,
                        Experiance = model.Experience,
                    };
                    _dbContext.Nurses.Add(nurse);
                    _dbContext.SaveChanges();
                    return true;
                };
                return false;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public bool UpdateNurse(UpdateNurseModel model)
        {
            try
            {
                var nurse = _dbContext.Nurses.Find(model.ID);
                var user = _dbContext.Users.Find(nurse.ApplicationUserID);
                user.PhoneNumber = model.Phone;
                user.Email = model.Email;
                user.FullName = model.Name;
                nurse.Experiance = model.Experience;
                nurse.Address = model.Address;
                _dbContext.Users.Update(user);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public (bool Result, UpdateNurseModel Nurse) GetNurseToUpdate(int nurseID)
        {
            try
            {
                var nurse = _dbContext.Nurses.Find(nurseID);
                var user = _dbContext.Users.Find(nurse.ApplicationUserID);
                var updateModel = new UpdateNurseModel()
                {
                    ID = nurseID,
                    Email = user.Email,
                    Status = nurse.Status,
                    Name = user.FullName,
                    Phone = user.PhoneNumber,
                    Experience = nurse.Experiance,
                    Address = nurse.Address,
                };
                return (true, updateModel);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new UpdateNurseModel() { ID = nurseID });
            }
        }
        public bool ChangeStatus(int nurseID, string status)
        {
            try
            {
                var nurse = _dbContext.Nurses.Find(nurseID);
                nurse.Status = status;
                _dbContext.Nurses.Update(nurse);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        #endregion

        #region Nurse Bookings
        public (bool Result, List<NurseBookingListModel> Bookings) GetNurseServiceBookings()
        {
            try
            {
                var bookings = _dbContext.NurseBookings.Where(b => b.Status != Consts.Deleted)
                    .Include(b => b.NurseService)
                    .Include(b => b.Patient)
                    .Include(b => b.Nurse).ThenInclude(n => n.User)
                    .Select(b => new NurseBookingListModel()
                    {
                        Status = b.Status,
                        ServiceID = b.ServiceID,
                        ID = b.ID,
                        ServiceNameAR = b.NurseService.NameAR,
                        ServiceNameEN = b.NurseService.NameEN,
                        PatintName = b.Patient.FullName,
                        Price = b.Price,
                        VisitTime = b.VisitTime,
                        NurseName = b.Nurse == null ? "" : b.Nurse.User.FullName
                    }
                ).ToList();
                return (true, bookings);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }
        public (bool Result, List<SelectListItem> Nurses) GetNursesToSelect()
        {
            try
            {
                var nurses = _dbContext.Nurses.Where(n => n.Status == Consts.Active).Include(n => n.User).Select(n => new SelectListItem()
                {
                    Text = n.User.FullName,
                    Value = n.ID + "",
                }).ToList();
                return (true, nurses);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }
        public async Task<bool> AcceptNurseServiceBooking(int bookingID, DateTime vistTime, int nurseID)
        {
            try
            {
                var booking = _dbContext.NurseBookings.Find(bookingID);
                booking.Status = Consts.Accepted;
                booking.VisitTime = vistTime;
                booking.NurseID = nurseID;
                _dbContext.NurseBookings.Update(booking);
                _dbContext.SaveChanges();
                await NC.NotifyPatientWithAcceptedNurseBooking(bookingID);
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public async Task<bool> RejectNurseServiceBooking(int bookingID)
        {
            try
            {
                var booking = _dbContext.NurseBookings.Find(bookingID);
                booking.Status = Consts.Rejected;
                _dbContext.NurseBookings.Update(booking);
                _dbContext.SaveChanges();
                await NC.NotifyPatientWithRejectedNurseBooking(bookingID);
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public async Task<bool> MakeNurseServiceBookingDone(int bookingID)
        {
            try
            {
                var booking = _dbContext.NurseBookings.Find(bookingID);
                booking.Status = Consts.Done;
                _dbContext.NurseBookings.Update(booking);
                _dbContext.SaveChanges();
                await NC.NotifyPatientWithDoneNurseBooking(bookingID);
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        #endregion

        #region Doctors
        public (bool Result, DoctorDetailsModel Doctor) GetDoctorDetails(int doctorId)
        {
            try
            {
                var doctor = _dbContext.Doctors.Find(doctorId);
                var position = _dbContext.DoctorPositions.Find(doctor.PositionID);
                var specilazation = _dbContext.Specializations.Find(doctor.SpecializationID);
                var user = _dbContext.Users.Find(doctor.ApplicationUserID);

                var detailsModel = new DoctorDetailsModel()
                {
                    Name = user.FullName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    AboutDoctor = doctor.AboutDoctor,
                    ApplicationUserID = doctor.ApplicationUserID,
                    SpecializationNameAR = specilazation.NameAR,
                    SpecializationNameEN = specilazation.NameEN,
                    Status = doctor.Status,
                    Address = doctor.Address,
                    CloseTime = doctor.CloseTime,
                    ID = doctor.ID,
                    Lat = doctor.Lat,
                    Lon = doctor.Lon,
                    OpenTime = doctor.OpenTime,
                    Price = doctor.Price,
                    WorkHours = doctor.WorkHours,
                    PositionNameAR = position.NameAR,
                    PositionNameEN = position.NameEN,

                };
                detailsModel.Payments = _dbContext.DoctorBookingPayments.Include(p => p.DoctorBooking)
                   .Where(p => p.DoctorBooking.DoctorID == doctorId)
                   .Select(p => new DoctorPayment()
                   {
                       ID = p.ID,
                       Status = p.Status,
                       SystemProfit = p.SystemProfit,
                       SettledDate = p.SettledDate.GetValueOrDefault().ToString("dd-MM-yyyy HH:mm"),
                       AcceptDate = p.AcceptDate.GetValueOrDefault().ToString("dd-MM-yyyy HH:mm"),
                       CreateDate = p.CreateDate.ToString("dd-MM-yyyy HH:mm"),
                       DoctorProfit = p.DoctorProfit,
                       ProfitPercentage = p.ProfitPercentage,
                       TotalAmount = p.TotalAmount,
                   }).ToList();
                return (true, detailsModel);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }
        public (bool Result, List<DoctorsListModel> Doctors) GetDoctors()
        {
            try
            {
                var doctors = _dbContext.Doctors.Include(d => d.User).Select(d => new DoctorsListModel()
                {
                    ID = d.ID,
                    Email = d.User.Email,
                    FullName = d.User.FullName,
                    Status = d.Status,
                    Phone = d.User.PhoneNumber,
                    Price = d.Price.GetValueOrDefault(),
                }).ToList();
                return (true, doctors);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }
        public (bool Result, List<SelectListItem> Doctors) GetDoctorsToSelect()
        {
            try
            {
                var doctors = _dbContext.Doctors.Include(d => d.User).Select(d => new SelectListItem()
                {
                    Value = d.ID + "",
                    Text = d.User.FullName,

                }).ToList();
                return (true, doctors);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }

        public async Task<bool> CreateDoctor(CreateDoctorModel model)
        {
            try
            {
                var user = new ApplicationUser()
                {
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    FullName = model.FullName,
                    UserName = model.Phone,
                    AccountType = Consts.DoctorAccountType,
                };
                var reuslt = await _userManager.CreateAsync(user, "123456");
                if (!reuslt.Succeeded)
                {
                    return false;
                }
                var doctor = new Doctor()
                {
                    SpecializationID = model.SpecializationID,
                    Status = Consts.NotActive,
                    ApplicationUserID = user.Id,
                };
                _dbContext.Doctors.Add(doctor);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public bool ChangeDoctorStatus(int doctorID, string status)
        {
            try
            {
                var doctor = _dbContext.Doctors.Find(doctorID);
                doctor.Status = status;
                _dbContext.Doctors.Update(doctor);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public (bool Result, UpdateDoctorModel Doctor) GetDoctorToUpdate(int doctorID)
        {
            try
            {
                var doctor = _dbContext.Doctors.Find(doctorID);
                var user = _dbContext.Users.Find(doctor.ApplicationUserID);
                var updateModel = new UpdateDoctorModel()
                {
                    SpecializationID = doctor.SpecializationID,
                    Email = user.Email,
                    FullName = user.FullName,
                    ID = doctor.ID,
                    Phone = user.PhoneNumber
                };
                return (true, updateModel);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }
        public bool UpdateDoctor(UpdateDoctorModel model)
        {
            try
            {
                var doctor = _dbContext.Doctors.Find(model.ID);
                var user = _dbContext.Users.Find(doctor.ApplicationUserID);

                doctor.SpecializationID = model.SpecializationID;
                user.Email = model.Email;
                user.FullName = model.FullName;
                user.PhoneNumber = model.Phone;

                _dbContext.Doctors.Update(doctor);
                _dbContext.Users.Update(user);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        #endregion

        #region Specializations
        public (bool Result, List<SpecializationsListModel> Specializations) GetSpecializations()
        {
            try
            {
                var specilizations = _dbContext.Specializations.Select(s => new SpecializationsListModel()
                {
                    ID = s.ID,
                    NameAR = s.NameAR,
                    NameEN = s.NameEN,
                    Status = s.Status,
                }).ToList();
                return (true, specilizations);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }
        public (bool Result, List<SelectListItem> Specializations) GetSpecializationsToSelect()
        {
            try
            {
                var list = _dbContext.Specializations.Where(s => s.Status == Consts.Active).Select(s => new SelectListItem(s.NameAR, s.ID + "")).ToList();
                return (true, list);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }
        public (bool Result, UpdateSpecializationModel Specialization) GetSpecializationToUpdate(int specializationID)
        {
            try
            {
                var specilization = _dbContext.Specializations.Find(specializationID);
                var model = new UpdateSpecializationModel()
                {
                    ID = specializationID,
                    NameAR = specilization.NameAR,
                    NameEN = specilization.NameEN,
                };
                return (true, model);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }
        public bool UpdateSpecialization(UpdateSpecializationModel model)
        {
            try
            {
                var specilization = _dbContext.Specializations.Find(model.ID);
                specilization.NameEN = model.NameEN;
                specilization.NameAR = model.NameAR;

                _dbContext.Specializations.Update(specilization);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public bool CreateSpecialization(CreateSpecializationModel model)
        {
            try
            {
                var spec = new Specialization()
                {
                    NameEN = model.NameEN,
                    NameAR = model.NameAR,
                    Status = Consts.NotActive
                };
                _dbContext.Specializations.Add(spec);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public bool ChangeSpecializationStatus(int specializationID, string status)
        {
            try
            {
                var specialization = _dbContext.Specializations.Find(specializationID);
                specialization.Status = status;
                _dbContext.Specializations.Update(specialization);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        #endregion

        #region Ambulance
        public (bool Result, List<AmbulancesListModel> Ambulances) GetAmbulances()
        {
            try
            {
                var ambulances = _dbContext.Ambulances.Select(a => new AmbulancesListModel()
                {
                    Hospital = a.Hospital,
                    ID = a.ID,
                    Phone = a.Phone,
                    Status = a.Status,
                }).ToList();
                return (true, ambulances);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }
        public bool CreateAmbulance(CreateAmbulanceModel model)
        {
            try
            {
                var ambulance = new Ambulance()
                {
                    Hospital = model.Hospital,
                    Status = Consts.NotActive,
                    Phone = model.Phone,
                };
                _dbContext.Ambulances.Add(ambulance);
                _dbContext.SaveChanges();
                return (true);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public bool UpdateAmbulance(UpdateAmbulanceModel model)
        {
            try
            {
                var ambulance = _dbContext.Ambulances.Find(model.ID);
                ambulance.Phone = model.Phone;
                ambulance.Hospital = model.Hospital;
                _dbContext.Ambulances.Update(ambulance);
                _dbContext.SaveChanges();
                return (true);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public bool ChangeAmbulanceStatus(int ambulanceID, string status)
        {
            try
            {
                var ambulance = _dbContext.Ambulances.Find(ambulanceID);
                ambulance.Status = status;
                _dbContext.Ambulances.Update(ambulance);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public (bool Result, UpdateAmbulanceModel Ambulance) GetAmbulanceToUpdate(int ambulanceID)
        {
            try
            {
                var ambulance = _dbContext.Ambulances.Find(ambulanceID);
                var model = new UpdateAmbulanceModel()
                {
                    ID = ambulanceID,
                    Hospital = ambulance.Hospital,
                    Phone = ambulance.Phone,
                };
                return (true, model);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }


        #endregion

        #region Positions
        public (bool Result, List<DoctorPositionsListModel> Services) GetDoctorPositions()
        {
            try
            {
                var positions = _dbContext.DoctorPositions.Select(s => new DoctorPositionsListModel()
                {
                    ID = s.ID,
                    NameAR = s.NameAR,
                    NameEN = s.NameEN,
                    Status = s.Status,
                }).ToList();
                return (true, positions);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new List<DoctorPositionsListModel>());
            }
        }

        public bool CreateDoctorPosition(CreateDoctorPositionModel model)
        {
            try
            {
                var position = new DoctorPosition();
                position.NameAR = model.NameAR;
                position.NameEN = model.NameEN;
                position.Status = Consts.NotActive;
                _dbContext.DoctorPositions.Add(position);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public bool ChangeDoctorPositionStatus(int positionID, string status)
        {
            try
            {
                var position = _dbContext.DoctorPositions.Find(positionID);
                position.Status = status;
                _dbContext.DoctorPositions.Update(position);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public (bool Result, UpdateDoctorPositionModel Service) GetDoctorPositionToUpdate(int positionID)
        {
            try
            {
                var position = _dbContext.DoctorPositions.Find(positionID);
                var editModel = new UpdateDoctorPositionModel()
                {
                    ID = positionID,
                    NameAR = position.NameAR,
                    NameEN = position.NameEN,
                };
                return (true, editModel);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new UpdateDoctorPositionModel());
            }
        }
        public bool UpdateDoctorPosition(UpdateDoctorPositionModel model)
        {
            try
            {
                var position = _dbContext.DoctorPositions.FirstOrDefault(s => s.ID == model.ID);
                position.NameAR = model.NameAR;
                position.NameEN = model.NameEN;
                _dbContext.DoctorPositions.Update(position);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        #endregion

        #region Payments
        public (bool Result, List<AllDoctorPaymentsListModel> Payments) GetAllDoctorPayments()
        {
            try
            {
                var payments = _dbContext.DoctorBookingPayments.Include(p => p.DoctorBooking)
                    .ThenInclude(b => b.Doctor).ThenInclude(d => d.User).Select(p => new AllDoctorPaymentsListModel()
                    {
                        ID = p.ID,
                        Status = p.Status,
                        SystemProfit = p.SystemProfit,
                        SettledDate = p.SettledDate.GetValueOrDefault().ToString("dd-MM-yyyy HH:mm"),
                        AcceptDate = p.AcceptDate.GetValueOrDefault().ToString("dd-MM-yyyy HH:mm"),
                        CreateDate = p.CreateDate.ToString("dd-MM-yyyy HH:mm"),
                        DoctorName = p.DoctorBooking.Doctor.User.FullName,
                        DoctorPhone = p.DoctorBooking.Doctor.User.PhoneNumber,
                        DoctorProfit = p.DoctorProfit,
                        ProfitPercentage = p.ProfitPercentage,
                        TotalAmount = p.TotalAmount,
                    }).ToList();
                return (true, payments);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }
        public bool AcceptDoctorBookingPayment(int paymentID, string status)
        {
            try
            {
                var payment = _dbContext.DoctorBookingPayments.Find(paymentID);
                payment.Status = status;
                payment.AcceptDate = DateTime.Now;
                _dbContext.DoctorBookingPayments.Update(payment);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public bool SettleDoctorBookingPayment(int paymentID, string status)
        {
            try
            {
                var payment = _dbContext.DoctorBookingPayments.Find(paymentID);
                payment.Status = status;
                payment.SettledDate = DateTime.Now;
                _dbContext.DoctorBookingPayments.Update(payment);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        #endregion

        #region Services
        public (bool Result, List<ServicesListModel> Services) GetServices()
        {
            try
            {
                var services = _dbContext.Services.Select(s => new ServicesListModel()
                {
                    ID = s.ID,
                    NameAR = s.NameAR,
                    NameEN = s.NameEN,
                    Status = s.Status,
                }).ToList();
                return (true, services);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new List<ServicesListModel>());
            }
        }

        public bool ChangeServiceStatus(int serviceID, string status)
        {
            try
            {
                var service = _dbContext.Services.Find(serviceID);
                service.Status = status;
                _dbContext.Services.Update(service);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        #endregion
        #region Patients
        public (bool Result, List<PatientsListModel> Patients) GetPatients()
        {
            try
            {
                var users = _dbContext.Users.Where(u => u.AccountType == Consts.PatientAccountType).Select(s => new PatientsListModel()
                {
                    ID = s.Id,
                    Name = s.FullName,
                    Email = s.Email,
                    Phone = s.PhoneNumber,
                    Status = s.Status,
                }).ToList();
                return (true, users);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new List<PatientsListModel>());
            }
        }

        public bool ChangeUserStatus(string UserID, string status)
        {
            try
            {
                var user = _dbContext.Users.Find(UserID);
                user.Status = status;
                _dbContext.Users.Update(user);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        #endregion
        #region Admins
        public bool SaveAdminInstanceID(string adminID, string instanceID)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Id == adminID);
                user.InstanceID = instanceID;
                _dbContext.Users.Update(user);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        #endregion

        #region Reports
        public (bool Result, DoctorBookingReport Report) GetDoctorReport(int doctorID)
        {
            try
            {
                var doctor = _dbContext.Doctors.Find(doctorID);
                var user = _dbContext.Users.Find(doctor.ApplicationUserID);
                var report = new DoctorBookingReport()
                {
                    DoctorName = user.FullName,
                    Bookings = _dbContext.DoctorBookings.Where(b => b.DoctorID == doctorID)
                    .Include(b => b.Patient)
                    .Select(b => new BookingsList()
                    {
                        Status = b.Status,
                        BookingID = b.ID,
                        PatientName = b.Patient.FullName,
                        VisitTime = b.VisiteDate.ToString("dd-MM-yyyy HH:mm"),
                        PatientPhone = b.Patient.PhoneNumber,
                        Price = _dbContext.DoctorBookingPayments.FirstOrDefault(p => p.BookingID == b.ID).TotalAmount,
                        DoctorProfit = _dbContext.DoctorBookingPayments.FirstOrDefault(p => p.BookingID == b.ID).DoctorProfit,
                        SystemProfit = _dbContext.DoctorBookingPayments.FirstOrDefault(p => p.BookingID == b.ID).SystemProfit,
                        PaymentStatus = _dbContext.DoctorBookingPayments.FirstOrDefault(p => p.BookingID == b.ID).Status
                    }).ToList()
                };
                return (true, report);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new DoctorBookingReport());
            }
        }

        public (bool Result, List<LabBookingsReport> Report) GetLabBookingReport()
        {
            try
            {
                var report = _dbContext.LabServiceBookings
                    .Include(b => b.Patient)
                    .Include(b => b.LabService)
                    .Select(b => new LabBookingsReport()
                    {
                        ServiceNameAR = b.LabService.NameAR,
                        ServiceNameEN = b.LabService.NameEN,
                        BookingID = b.ID,
                        PatientName = b.Patient.FullName,
                        PatientPhone = b.Patient.PhoneNumber,
                        TotalAmount = b.Price,
                        VisitTime = b.VisitTime.ToString("dd-MM-yyyy HH:mm"),

                    }).ToList();
                return (true, report);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }

        public (bool Result, NurseReport Report) GetNurseReport(int nurseID)
        {
            try
            {
                var nurse = _dbContext.Nurses.Find(nurseID);
                var user = _dbContext.Users.Find(nurse.ApplicationUserID);
                var report = new NurseReport()
                {
                    NurseName = user.FullName,
                    NurseBookings = _dbContext.NurseBookings.Where(b=>b.NurseID==nurseID)
                    .Include(b => b.Patient)
                    .Include(b => b.NurseService).Select(b => new NurseBookings
                    {
                        ServiceNameAR = b.NurseService.NameAR,
                        ServiceNameEN = b.NurseService.NameEN,
                        PatientName = b.Patient.FullName,
                        PatientPhone = b.Patient.PhoneNumber,
                        Price = b.Price,
                        VisitTime = b.VisitTime.ToString("dd-MM-yyyy HH:mm")
                    }).ToList()
                };
                return (true,report);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }
        #endregion

        #region Settings
        public bool CreateBankAccount(CreateBankAccountModel model)
        {
            try
            {
                var account = new BankAccount()
                {
                    Bank = model.BankName,
                    Branch = model.BranchName,
                    Number = model.AccountNumber,
                };
                _dbContext.BankAccounts.Add(account);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public bool UpdateBankAccount(UpdateBankAccountModel model)
        {
            try
            {
                var account = _dbContext.BankAccounts.Find(model.ID);
                account.Bank = model.BankName;
                account.Branch= model.BranchName;
                account.Number = model.AccountNumber;
                _dbContext.BankAccounts.Update(account);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public (bool Result,List<BankAccountListModel>Accounts) GetBankAccounts()
        {
            try
            {
                var accounts = _dbContext.BankAccounts.Select(a => new BankAccountListModel()
                {
                    AccountNumber = a.Number,
                    BankName = a.Bank,
                    BranchName = a.Branch
                }).ToList();
                return (true, accounts);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }
        public (bool Result,UpdateBankAccountModel Account) GetBankAccountToUpdate(int accountID)
        {
            try
            {
                var account = _dbContext.BankAccounts.Find(accountID);
                var model = new UpdateBankAccountModel()
                {
                    ID = account.ID,
                    AccountNumber = account.Number,
                    BankName = account.Bank,
                    BranchName = account.Branch
                };
                return (true, model);
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false,new UpdateBankAccountModel() { ID=accountID});
            }
        }
        #endregion
    }
}
