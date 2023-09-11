using HrbiApp.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DBContext;
using Microsoft.AspNetCore.Identity;

using HrbiApp.API.Models.Booking;
using Microsoft.AspNetCore.Authorization;

namespace HrbiApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : APIController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public BookingController(
            IConfiguration configuration, ApplicationDBContext db,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,
            IServiceScopeFactory serviceScopeFactory) : base(db, configuration, userManager, signInManager, serviceScopeFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
      
        }

        [HttpPost]
        [Route("PlaceDoctorBooking")]
        public async Task<IActionResult> PlaceDoctorBooking([FromBody] PlaceDoctorBookigRequest model)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == Consts.UserIDClaimName).Value;
            if (!_validator.IsValidDoctor(model.DoctorId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Messages.NotValidDoctor
                });
            }
            if (!_validator.IsValidPatient(userId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Messages.NotValidPatient
                });
            }
            var result = await CS.PlaceDoctorBooking(model,userId);

            if (result.Result == true)
            {
                return Ok(new BaseResponse()
                {
                    Status = true,
                    Message = Messages.SuccessfulBooking,
                    Data = result.Response
                });
            }
            else
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.BookingFailed
                });
            }
        }

        [HttpPost]
        [Route("PlaceNurseBooking")]
        public async Task<IActionResult> PlaceNurseBooking([FromBody] PlaceNurseBookingRequest model)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == Consts.UserIDClaimName).Value;
            if (!_validator.IsValidNurseService(model.NurseServiceId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Messages.NotValidNurseService
                });
            }
            if (!_validator.IsValidPatient(userId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Messages.NotValidPatient
                });
            }
            var result = await CS.PlaceNurseServiceBooking(model,userId);

            if (result.Result == true)
            {
                return Ok(new BaseResponse()
                {
                    Status = true,
                    Message = Messages.SuccessfulBooking,
                    Data = result.Response
                });
            }
            else
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.BookingFailed
                });
            }
        }

        [HttpPost]
        [Route("PlaceLabServiceBooking")]
        public async Task<IActionResult> PlaceLabServiceBooking([FromBody] PlaceLabServiceBookingRequest model)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == Consts.UserIDClaimName).Value;
            if (!_validator.IsValidLabService(model.LabServiceId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Messages.NotValidLabService
                });
            }
            if (!_validator.IsValidPatient(userId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Messages.NotValidPatient
                });
            }
            var result = await CS.PlaceLabServiceBooking(model,userId);

            if (result.Result == true)
            {
                return Ok(new BaseResponse()
                {
                    Status = true,
                    Message = Messages.SuccessfulBooking,
                    Data = result.Response

                });
            }
            else
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.BookingFailed
                });
            }
        }

        [HttpPut]
        [Route("AcceptBookingByDoctor")]
        public async Task<IActionResult> AcceptBookingByDoctor(AcceptBookingModel model)
        {


            if (!_validator.IsValidDoctorBooking(model.BookingId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.NotValidBooking
                });
            }
            if (_validator.IsBookingRejected(model.BookingId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.BookingAlreadyRejected
                });
            }
           
            //if (!_validator.IsValidDoctorToAcceptRejectBooking(model.DoctorId,model.BookingId)) {
            //    return Ok(new BaseResponse()
            //    {
            //        Message = Messages.NotValidDoctor
            //    });

            //}
            var result = CS.AcceptBooking(model);
            if(!result.Result)
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.Failed
                });
            }
            else
            {
                return Ok(new BaseResponse()
                {
                    Status = true
               
                }); ;
            }
        }

        [HttpPut]
        [Route("RejectBookingByDoctor")]
        public async Task<IActionResult> RejectBookingByDoctor(int bookingId)
        {
            if (!_validator.IsValidDoctorBooking(bookingId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Messages.NotValidBooking
                });
            }
            if (_validator.IsBookingAccepted(bookingId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.BookingAlreadyAccepted
                });
            }
            //if (!_validator.IsValidDoctorToAcceptRejectBooking(doctorId, bookingId))
            //{
            //    return Ok(new BaseResponse()
            //    {
            //        Message = Messages.NotValidDoctor
            //    });

            //}
            var result = CS.RejectBooking(bookingId);
            if (!result.Result)
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.Failed
                });
            }
            else
            {
                return Ok(new BaseResponse()
                {
                    Status = true

                }); ;
            }
        }

        [HttpPut]
        [Route("DelayBookingByDoctor")]
        public async Task<IActionResult> DelayBookingByDoctor(DelayBookingModel model)
        {
            if (!_validator.IsValidDoctorBooking(model.BookingId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Messages.NotValidBooking
                });
            }
            if (_validator.IsBookingAccepted(model.BookingId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.BookingAlreadyAccepted
                });
            }
            if (_validator.IsBookingRejected(model.BookingId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.BookingAlreadyRejected
                });
            }
            //if (!_validator.IsValidDoctorToAcceptRejectBooking(doctorId, bookingId))
            //{
            //    return Ok(new BaseResponse()
            //    {
            //        Message = Messages.NotValidDoctor
            //    });

            //}
            var result = CS.DelayBooking(model);
            if (!result.Result)
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.Failed
                });
            }
            else
            {
                return Ok(new BaseResponse()
                {
                    Status = true

                }); ;
            }
        }

        [HttpGet]
        [Route("GetDoctorBookingsByDoctor")]
        public async Task<IActionResult> GetDoctorBookingsByDoctor()
        {
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == Consts.UserIDClaimName).Value;
            var doctorId = _db.Doctors.FirstOrDefault(a => a.ApplicationUserID == userId).ID;
          
            if (!_validator.IsValidDoctor(doctorId)) 
            {
                return Ok(new BaseResponse() {
                    Message = Messages.NotValidDoctor
                });
            }
                var result = CS.GetDoctorBookingsByDoctorId(doctorId);
            if (!result.Result)
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.Failed
                });
            }

            return Ok(new BaseResponse()
            {
                Status = true,
                Data = result.Response
            });

        }

        [HttpGet]
        [Route("GetDoctorBookingsByPatient")]
        public async Task<IActionResult> GetDoctorBookingsByPatient()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == Consts.UserIDClaimName).Value;

            if (!_validator.IsValidPatient(userId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.NotValidPatient
                });
            }
            var result = CS.GetDoctorBookingsByPatientId(userId);
            if (!result.Result)
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.Failed
                });
            }

            return Ok(new BaseResponse()
            {
                Status = true,
                Data = result.Response
            });

        }

        [HttpGet]
        [Route("GetLabServiceBookingsByPatient")]
        public async Task<IActionResult> GetLabServiceBookingsByPatient()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == Consts.UserIDClaimName).Value;

            if (!_validator.IsValidPatient(userId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.NotValidPatient
                });
            }
            var result = CS.GetLabServiceBookingsByPatientId(userId);
            if (!result.Result)
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.Failed
                });
            }

            return Ok(new BaseResponse()
            {
                Status = true,
                Data = result.Response
            });

        }

        [HttpGet]
        [Route("GetNurseBookingsByPatient")]
        public async Task<IActionResult> GetNurseBookingsByPatient()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == Consts.UserIDClaimName).Value;
            if (!_validator.IsValidPatient(userId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.NotValidPatient
                });
            }
            var result = CS.GetNurseBookingsByPatientId(userId);
            if (!result.Result)
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.Failed
                });
            }

            return Ok(new BaseResponse()
            {
                Status = true,
                Data = result.Response
            });

        }

        [HttpGet]
        [Route("GetDoctorBookingPayment/{bookingId}")]
        public async Task<IActionResult> GetDoctorBookingPayment(int bookingId)
        {

            if (!_validator.IsValidDoctorBooking(bookingId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Messages.NotValidBooking
                });
            }
            var result =await CS.GetDoctorBookingPayment(bookingId);
            if (result.Result == true)
            {
                return Ok(new BaseResponse()
                {
                    Status = true,
                    Data = result.Response
                });

            }
            else
            {
                return Ok(new BaseResponse()
                {
                    Status = false,

                });
            }

        }
    }
}
