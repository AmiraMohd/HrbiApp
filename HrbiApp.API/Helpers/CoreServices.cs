using DBContext;
using DBContext.Enums;
using DBContext.Migrations;
using HrbiApp.API.Models.Account;
using HrbiApp.API.Models.Booking;
using HrbiApp.API.Models.Common;
using HrbiApp.API.Models.Doctor;
using HrbiApp.API.Models.Patient;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace HrbiApp.API.Helpers
{
    public class CoreServices
    {
        ApplicationDBContext _db;
        IConfiguration _configuration;
        ExceptionHandler _ex;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        private IServiceScopeFactory _serviceScopeFactory;
        public SMSSender SMS;

        public CoreServices(ApplicationDBContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IServiceScopeFactory serviceProviderFactory)
        {
            _db = db;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _serviceScopeFactory = serviceProviderFactory;
            _ex = new ExceptionHandler(db);
            SMS = new SMSSender(db);
        }

        #region Doctor Services
        public async Task<(bool Result, string Message)> DoctorRegister(DoctorRegisterRequestModel model)
        {
            try
            {
                var user = new ApplicationUser()
                {
                    UserName = model.PhoneNumber,
                    FullName = model.FirstName + " " + model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    AccountType = AccountType.Doctor.ToString(),
                    Status = Consts.Active

                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return (false, string.Join(",", result.Errors.Select(e => e.Description)));
                }

                user = _db.Users.FirstOrDefault(u => u.UserName == model.PhoneNumber);
                var doctor = new Doctor()
                {
                    ApplicationUserID = user.Id,
                    SpecializationID = model.DoctorSpecializationId,
                    PositionID = model.DoctorPositionId,
                    Status = Consts.NotActive


                };
                _db.Doctors.Add(doctor);
                _db.SaveChanges();
                SMS.SendConfirmationSMS(user.UserName, user.Id);
                return (true, "");
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, Messages.ExceptionOccured);
            }
        }

        public async Task<bool> CheckRegisterationOtp(OTPConfirmationModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.PhoneNumber);
                if (user == null)
                {
                    return false;
                }

                else
                {
                    var otp = _db.OTPs.FirstOrDefault(o => o.UserID == user.Id && o.Purpose == Consts.ConfirmationPurpose
                    && o.Code == model.OTP);
                    if (otp == null)
                    {
                        return false;
                    }
                    user.PhoneNumberConfirmed = true;
                    _db.Update(user);
                    _db.SaveChanges();
                }

                return true;
            }

            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public async Task<(bool Result, DoctorLoginResponse Response)> DoctorLogin(DoctorLoginModel model)
        {
            try
            {

                var user = await _userManager.FindByNameAsync(model.PhoneNumber);
                if (user == null)
                {
                    return (false, new DoctorLoginResponse() { Message = Messages.UserNotExist });
                }

                var result = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!result)
                {
                    return (false, new DoctorLoginResponse() { Message = Messages.WrongeUserNameOrPassword });
                }
                var token = await GenerateJSONWebToken(user);
                if (token == "")
                {
                    return (false, new DoctorLoginResponse() { Message = Messages.ExceptionOccured });
                }
                var customer = _db.Doctors.FirstOrDefault(c => c.ApplicationUserID == user.Id);
                return (true, new DoctorLoginResponse()
                {
                    Token = token,
                    PhoneNumber = model.PhoneNumber,
                    Name = user.FullName
                });
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new DoctorLoginResponse() { Message = Messages.ExceptionOccured });
            }
        }

        public (bool Result, List<DoctorsList> Response) GetDoctorsList()
        {
            try
            {
                var doctors = _db.Doctors.Include(a => a.User).ToList();
                var result = doctors.Select(a => new DoctorsList
                {
                    DoctorId = a.ID,
                    CloseTime = a.OpenTime,
                    OpenTime = a.CloseTime,
                    DoctorName = a.User.FullName,
                    AboutDoctor = a.AboutDoctor,
                    Price = a.Price


                }).ToList();
                return (true, result);
            }
            catch (Exception)
            {
                return (false, new());
            }
        }

        public (bool Result, List<DoctorsList> Response) GetDoctorsByFilter(int specializationId, int positionId)
        {
            try
            {
                var doctors = _db.Doctors.ToList();
                if (specializationId != 0)
                {
                    doctors = _db.Doctors.Include(a => a.User).Include(a => a.Specialization).Where(a => a.SpecializationID == specializationId).ToList();
                }
                if (positionId != 0)
                {
                    doctors = _db.Doctors.Include(a => a.User).Include(a => a.Position).Where(a => a.PositionID == positionId).ToList();
                }

                var result = doctors.Select(a => new DoctorsList
                {
                    DoctorId = a.ID,
                    CloseTime = a.OpenTime,
                    OpenTime = a.CloseTime,
                    DoctorName = a.User.FullName,
                    AboutDoctor = a.AboutDoctor,
                    Price = a.Price


                }).ToList();
                return (true, result);
            }
            catch (Exception)
            {
                return (false, new());
            }
        }

        public async Task<(bool Result, DoctorBookingPaymentModel Response)> GetDoctorBookingPayment(int bookingId)
        {
            try
            {
                var payment = _db.DoctorBookingPayments.FirstOrDefault(a => a.BookingID == bookingId);
                var model = new DoctorBookingPaymentModel()
                {
                    ID = payment.ID,
                    AcceptDate = payment.AcceptDate,
                    TotalAmount = payment.TotalAmount,
                    BookingID = payment.BookingID,
                    CreateDate = payment.CreateDate,
                    Status = payment.Status,
                    SettledDate = payment.SettledDate
                };
                return (true, model);
            }
            catch (Exception)
            {
                return (false, new());
            }
        }

        public async Task<bool> UpdateDoctorDetails(DoctorProfileModel model)
        {
            try
            {
                var doctor = _db.Doctors.Find(model.Id);
                doctor.AboutDoctor = model.AboutDoctor;
                doctor.CloseTime = model.CloseTime;
                doctor.OpenTime = model.OpenTime;
                doctor.Price = model.TicketPrice;
                doctor.Address = model.Address;
                _db.Doctors.Update(doctor);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;

            }
        }


        #endregion

        #region Booking Services
        public async Task<(bool Result, PlaceDoctorBookingResponse Response)> PlaceDoctorBooking(PlaceDoctorBookigRequest model, string userId)
        {
            try
            {
                var booking = new DoctorBooking()
                {
                    PatientID = userId,
                    DoctorID = model.DoctorId,
                    Status = Consts.Pending,
                    CreateDate = DateTime.Now

                };
                _db.DoctorBookings.Add(booking);
                _db.SaveChanges();
                var doctor = _db.Doctors.Find(model.DoctorId);
                var percentage = Convert.ToDouble(_db.Settings.FirstOrDefault(s => s.Name == Consts.DoctorProfitSetting).Value);
                var payment = new DoctorBookingPayment()
                {
                    Status = Consts.New,
                    TotalAmount = doctor.Price.GetValueOrDefault(),
                    BookingID = booking.ID,
                    CreateDate = DateTime.Now,
                    ProfitPercentage = percentage,
                    SystemProfit = doctor.Price.GetValueOrDefault() * percentage,
                    DoctorProfit = doctor.Price.GetValueOrDefault() - (doctor.Price.GetValueOrDefault() * percentage),
                };
                _db.DoctorBookingPayments.Add(payment);
                _db.SaveChanges();
                return (true, new PlaceDoctorBookingResponse()
                {
                    BookingId = booking.ID,
                    DoctorId = booking.DoctorID,
                    PatientId = booking.PatientID
                });
            }
            catch (Exception)
            {
                return (false, new());
            }


        }

        public async Task<(bool Result, PlaceNurseBookingResponse Response)> PlaceNurseServiceBooking(PlaceNurseBookingRequest model, string userId)
        {
            try
            {
                var booking = new NurseBooking()
                {
                    ServiceID = model.NurseServiceId,
                    CreateDate = DateTime.Now,
                    PatientID = userId,
                    Status = Consts.Pending

                };
                _db.NurseBookings.Add(booking);
                _db.SaveChanges();
                return (true, new PlaceNurseBookingResponse
                {
                    ServiceId = booking.ServiceID,
                    BookingId = booking.ID,
                    PatientId = booking.PatientID
                });
            }
            catch (Exception)
            {
                return (false, new());
            }

        }

        public async Task<(bool Result, PlaceLabServiceBookingResponse Response)> PlaceLabServiceBooking(PlaceLabServiceBookingRequest model, string userId)
        {
            try
            {
                var labService = _db.LabServices.FirstOrDefault(a => a.ID == model.LabServiceId);
                var booking = new LabServiceBooking()
                {
                    LabServiceID = model.LabServiceId,
                    PatientID = userId,
                    Status = Consts.Pending,
                    IsFromHome = model.IsFromHome,
                    Price = labService.Price

                };
                _db.LabServiceBookings.Add(booking);
                _db.SaveChanges();

                return (true, new PlaceLabServiceBookingResponse
                {
                    ServiceId = booking.LabServiceID,
                    BookingId = booking.ID,
                    PatientId = booking.PatientID
                });
            }
            catch (Exception)
            {
                return (false, new());
            }
        }

        public (bool Result, List<DoctorBookingsList> Response) GetDoctorBookingsByDoctorId(int doctorId)
        {
            try
            {
                var bookings = _db.DoctorBookings.Where(a => a.DoctorID == doctorId);
                var result = bookings.Select(a => new DoctorBookingsList
                {
                    PatientId = a.PatientID,
                    PatientName = a.Patient.FullName,
                    PatientNumber = a.Patient.UserName,
                    Status = a.Status,
                    CreateDate = a.CreateDate,
                    VisiteDate = a.VisiteDate
                }).ToList();
                return (true, result);
            }
            catch (Exception)
            {
                return (false, new());
            }
        }

        public (bool Result, List<PatientBookingsList> Response) GetDoctorBookingsByPatientId(string patientId)
        {
            try
            {
                var bookings = _db.DoctorBookings.Where(a => a.PatientID == patientId);
                var result = bookings.Select(a => new PatientBookingsList
                {
                    DoctorId = a.DoctorID,
                    DoctorName = a.Doctor.User.FullName,
                    DoctorNumber = a.Doctor.User.PhoneNumber,
                    Status = a.Status,
                    CreateDate = a.CreateDate,
                    VisiteDate = a.VisiteDate
                }).ToList();
                return (true, result);
            }
            catch (Exception)
            {
                return (false, new());
            }
        }

        public async Task<bool> AcceptBooking(AcceptBookingModel model)
        {
            try
            {
                var booking = _db.DoctorBookings.FirstOrDefault(a => a.ID == model.BookingId);
                booking.VisiteDate = model.VisitTime;
                booking.Status = Consts.Accepted;
                _db.Update(booking);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> RejectBooking(int bookingId)
        {
            try
            {
                var booking = _db.DoctorBookings.FirstOrDefault(a => a.ID == bookingId);
                booking.Status = Consts.Rejected;
                _db.Update(booking);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DelayBooking(DelayBookingModel model)
        {
            try
            {
                var booking = _db.DoctorBookings.FirstOrDefault(a => a.ID == model.BookingId);
                booking.VisiteDate = model.VisitTime;
                booking.Status = Consts.Delayed;
                _db.Update(booking);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public (bool Result, List<NurseBookingsList> Response) GetNurseBookingsByPatientId(string patientId)
        {
            try
            {
                var bookings = _db.NurseBookings.Where(a => a.PatientID == patientId);
                var result = bookings.Select(a => new NurseBookingsList
                {
                    ServiceID = a.ServiceID,
                    PatientID = a.PatientID,
                    Status = a.Status,
                    CreateDate = a.CreateDate,
                    VisitTime = a.VisitTime
                }).ToList();
                return (true, result);
            }
            catch (Exception)
            {
                return (false, new());
            }
        }

        public (bool Result, List<LabServiceBookingsList> Response) GetLabServiceBookingsByPatientId(string patientId)
        {
            try
            {
                var bookings = _db.LabServiceBookings.Where(a => a.PatientID == patientId);
                var result = bookings.Select(a => new LabServiceBookingsList
                {
                    LabServiceID = a.LabServiceID,
                    PatientID = a.PatientID,
                    Status = a.Status,
                    IsFromHome = a.IsFromHome,
                    VisitTime = a.VisitTime,
                    Price = a.Price
                }).ToList();
                return (true, result);
            }
            catch (Exception)
            {
                return (false, new());
            }
        }
        #endregion

        #region Common Services
        public async Task<string> GenerateJSONWebToken(ApplicationUser user)
        {
            try
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var stringUserRoles = string.Join(',', userRoles);
                var key = _configuration["JWT:Secret"];
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var ceredintial = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Sub,user.PhoneNumber),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, stringUserRoles),
                    new Claim(Consts.UserIDClaimName,user.Id),
                    new Claim(Consts.PhoneNumberClaimName,user.PhoneNumber),


                };
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims,
                    expires: DateTime.UtcNow.AddYears(1),
                    signingCredentials: ceredintial);

                var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
                return encodedToken;
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return "";
            }
        }

        public async Task<bool> SendConfirmationOTP(string userName)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(u => u.UserName == userName);
                string OTP = new Random().Next(0, 999999).ToString("D6");
                var otp = _db.OTPs.FirstOrDefault(otp => otp.UserID == user.Id &&
                otp.Purpose == Consts.ConfirmationPurpose);
                if (otp == null)
                {
                    _db.OTPs.Add(new OTP()
                    {
                        Code = OTP,
                        Phone = user.PhoneNumber,
                        UserID = user.Id,
                        Purpose = Consts.ConfirmationPurpose,
                        Count = 1
                    });
                }
                else
                {
                    if (otp.Count > 50)
                    {
                        return false;
                    }
                    otp.Code = OTP;
                    otp.Count++;
                    _db.Entry(otp).State = EntityState.Modified;
                }
                _db.SaveChanges();
                string message = Messages.YourVerficationCodeIs + OTP;

                //var result = SMS.SendSMS(message, user.PhoneNumber);
                return true;
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public async Task<bool> SendResetOTP(string userName)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(u => u.UserName == userName);
                string OTP = new Random().Next(0, 999999).ToString("D6");
                var otp = _db.OTPs.FirstOrDefault(otp => otp.UserID == user.Id &&
                otp.Purpose == Consts.ResetPurpose);
                if (otp == null)
                {
                    _db.OTPs.Add(new OTP()
                    {
                        Code = OTP,
                        Phone = user.PhoneNumber,
                        UserID = user.Id,
                        Purpose = Consts.ResetPurpose,
                        Count = 1
                    });
                }
                else
                {
                    if (otp.Count > 50)
                    {
                        return false;
                    }
                    otp.Code = OTP;
                    otp.Count++;
                    _db.Entry(otp).State = EntityState.Modified;
                }
                _db.SaveChanges();
                string message = Messages.YourResetPasswordCodeIs + OTP;

                var result = SMS.SendSMS(message, user.PhoneNumber);
                return result;
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public bool SendLoginOTP(string phone)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(u => u.UserName == phone);

                return SMS.SendConfirmationSMS(phone, user.Id);
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public async Task<bool> ForgetPasswordAsync(string phone)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(u => u.PhoneNumber == phone);
                return SMS.SendForgetPasswordSMS(phone, user.Id);
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public async Task<bool> ResetPassword(ResetPassword model)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(u => u.UserName == model.Phone);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public (bool Result, List<UserNotificationModel> Notifications) GetUserNotifications(string userID)
        {
            try
            {
                var notifications = _db.Notifications.Where(n => n.UserID == userID).Select(n => new UserNotificationModel()
                {
                    Time = n.Time.ToString("dd-MM-yyyy HH:mm"),
                    Title = n.Title,
                    Body = n.Body,
                    Type = n.Type,
                    DataID = n.DataID,
                }).ToList();
                return (true, notifications);
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new());
            }
        }
        public bool SaveInstanceID(string instanceID, string userID)
        {
            try
            {
                var user = _db.Users.Find(userID);
                user.InstanceID = instanceID;
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public (bool Result, List<SpecializationModel> Response) GetAllSpecializations()
        {
            try
            {

                var specializations = _db.Specializations.Select(a => new SpecializationModel
                {
                    Id = a.ID,
                    NameAR = a.NameAR,
                    NameEN = a.NameEN
                }).ToList();

                return (true, specializations);

            }
            catch (Exception)
            {

                return (false, new());
            }
        }

        public (bool Result, List<DoctorPositionsModel> Response) GetAllDoctorPositions()
        {
            try
            {

                var positions = _db.DoctorPositions.Select(a => new DoctorPositionsModel
                {
                    ID = a.ID,
                    NameAR = a.NameAR,
                    NameEN = a.NameEN
                }).ToList();

                return (true, positions);

            }
            catch (Exception)
            {

                return (false, new());
            }
        }

        public (bool Result, List<AmbulanceModel> Response) GetAllAmbulances()
        {
            try
            {
                var ambulances = _db.Ambulances.Select(a => new AmbulanceModel
                {
                    ID = a.ID,
                    Phone = a.Phone,
                    Hospital = a.Phone
                }).ToList();
                return (true, ambulances);
            }
            catch (Exception) { return (false, new()); }
        }

        public (bool Result, AmbulanceModel Response) GetAmbulanceById(int id)
        {
            try
            {
                var ambulance = _db.Ambulances.FirstOrDefault(a => a.ID == id);
                var model = new AmbulanceModel()
                {
                    ID = ambulance.ID,
                    Phone = ambulance.Phone,
                    Hospital = ambulance.Phone
                };
                return (true, model);
            }
            catch (Exception) { return (false, new()); }
        }
        public (bool Result, List<ServicesModel> Response) GetAllServices()
        {
            try
            {

                var services = _db.Services.Select(a => new ServicesModel
                {
                    Id = a.ID,
                    NameAR = a.NameAR,
                    NameEN = a.NameEN
                }).ToList();

                return (true, services);

            }
            catch (Exception ex)
            {

                return (false, new());
            }
        }

        public (bool Result, List<LabServicesModel> Response) GetAllLabServices()
        {
            try
            {

                var services = _db.LabServices.Where(a => a.Status == Consts.Active).Select(a => new LabServicesModel
                {
                    Id = a.ID,
                    NameAR = a.NameAR,
                    NameEN = a.NameEN,
                    Price = a.Price,
                    IsAvailableFromHome = a.IsAvailableFromHome
                }).ToList();

                return (true, services);

            }
            catch (Exception)
            {

                return (false, new());
            }
        }

        public (bool Result, List<NurseServicesModel> Response) GetAllNurseServices()
        {
            try
            {

                var services = _db.NurseServices.Where(a => a.Status == Consts.Active).Select(a => new NurseServicesModel
                {
                    Id = a.ID,
                    NameAR = a.NameAR,
                    NameEN = a.NameEN,
                    Price = a.Price
                }).ToList();

                return (true, services);

            }
            catch (Exception)
            {

                return (false, new());
            }
        }

        #endregion

        #region Patient Services
        public async Task<(bool Result, string Message)> PatientRegister(PatientRegisterRequestModel model)
        {
            try
            {
                var user = new ApplicationUser()
                {
                    UserName = model.PhoneNumber,
                    PhoneNumber = model.PhoneNumber,
                    AccountType = AccountType.Patient.ToString(),
                    Status = Consts.Active,
                    FullName = model.FirstName + " " + model.LastName
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return (false, string.Join(",", result.Errors.Select(e => e.Description)));
                }

                SMS.SendConfirmationSMS(user.UserName, user.Id);
                return (true, "");
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, Messages.ExceptionOccured);
            }
        }

        public async Task<(bool Result, PatientLoginResponse Response)> PatientLogin(PatientLoginModel model)
        {
            try
            {

                var user = await _userManager.FindByNameAsync(model.PhoneNumber);
                if (user == null)
                {
                    return (false, new PatientLoginResponse() { Message = Messages.UserNotExist });
                }

                else
                {
                    var otp = _db.OTPs.FirstOrDefault(o => o.UserID == user.Id && o.Purpose == Consts.ConfirmationPurpose
                    && o.Code == model.OTP);
                    if (otp == null)
                    {
                        return (false, new PatientLoginResponse() { Message = Messages.NotValidOTP });
                    }
                }
                var token = await GenerateJSONWebToken(user);
                if (token == "")
                {
                    return (false, new PatientLoginResponse() { Message = Messages.ExceptionOccured });
                }

                return (true, new PatientLoginResponse()
                {
                    Message = token,
                    PhoneNumber = model.PhoneNumber,
                    Name = user.FullName
                });
            }
            catch (Exception ex)
            {
                _ex.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return (false, new PatientLoginResponse() { Message = Messages.ExceptionOccured });
            }
        }

        //public async Task<(bool Result, List<PatientBillsModel> Response)> GetDoctorBookingBill(string bookingId)
        //{
        //    try
        //    {
        //        var bills = _db.DoctorBookingPayments.Include(a => a.DoctorBooking)
        //            .Where(a => a.DoctorBooking.PatientID == patientId).Select(a => new PatientBillsModel()
        //            {
        //                ID= a.ID,
        //                AcceptDate = a.AcceptDate,
        //                TotalAmount = a.TotalAmount,
        //                BookingID = a.BookingID,
        //                CreateDate= a.CreateDate,
        //                Status =a.Status,
        //                SettledDate = a.SettledDate
        //            }).ToList();
        //        return (true, bills);
        //    }
        //    catch (Exception)
        //    {
        //        return (false, new());
        //    }
        //}
        #endregion

    }
}
