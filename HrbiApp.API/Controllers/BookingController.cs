using HrbiApp.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DBContext;
using Microsoft.AspNetCore.Identity;

using HrbiApp.API.Models.Booking;

namespace HrbiApp.API.Controllers
{
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

            if (!_validator.IsValidDoctor(model.DoctorId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Messages.NotValidDoctor
                });
            }
            if (!_validator.IsValidPatient(model.PatientId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Messages.NotValidPatient
                });
            }
            var result = CS.PlaceDoctorBooking(model);

            if (result.Result == true)
            {
                return Ok(new BaseResponse()
                {
                    Status = true,
                    Message = Messages.SuccessfulBooking
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

            if (!_validator.IsValidNurseService(model.NurseServiceId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Messages.NotValidNurseService
                });
            }
            if (!_validator.IsValidPatient(model.PatientId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Messages.NotValidPatient
                });
            }
            var result = CS.PlaceNurseServiceBooking(model);

            if (result.Result == true)
            {
                return Ok(new BaseResponse()
                {
                    Status = true,
                    Message = Messages.SuccessfulBooking
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

            if (!_validator.IsValidLabService(model.LabServiceId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Messages.NotValidLabService
                });
            }
            if (!_validator.IsValidPatient(model.PatientId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Messages.NotValidPatient
                });
            }
            var result = CS.PlaceLabServiceBooking(model);

            if (result.Result == true)
            {
                return Ok(new BaseResponse()
                {
                    Status = true,
                    Message = Messages.SuccessfulBooking
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
        public async Task<IActionResult> AcceptBookingByDoctor(int bookingId,int doctorId)
        {
            if (_validator.IsBookingRejected(bookingId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.BookingAlreadyRejected
                });
            }
            if (!_validator.IsValidDoctorToAcceptRejectBooking(doctorId,bookingId)) {
                return Ok(new BaseResponse()
                {
                    Message = Messages.NotValidDoctor
                });

            }
            var result = CS.AcceptBooking(bookingId);
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
        public async Task<IActionResult> RejectBookingByDoctor(int bookingId, int doctorId)
        {
            if (_validator.IsBookingAccepted(bookingId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.BookingAlreadyAccepted
                });
            }
            if (!_validator.IsValidDoctorToAcceptRejectBooking(doctorId, bookingId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.NotValidDoctor
                });

            }
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
        public async Task<IActionResult> DelayBookingByDoctor(int bookingId, int doctorId)
        {
            if (_validator.IsBookingAccepted(bookingId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.BookingAlreadyAccepted
                });
            }
            if (_validator.IsBookingRejected(bookingId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.BookingAlreadyRejected
                });
            }
            if (!_validator.IsValidDoctorToAcceptRejectBooking(doctorId, bookingId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.NotValidDoctor
                });

            }
            var result = CS.DelayBooking(bookingId);
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
        [Route("GetDoctorBookingsByDoctorId")]
        public async Task<IActionResult> GetDoctorBookingsByDoctorId(int doctorId)
        {
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
        [Route("GetDoctorBookingsByPatientId")]
        public async Task<IActionResult> GetDoctorBookingsByPatientId(string patientId)
        {
            if (!_validator.IsValidPatient(patientId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.NotValidPatient
                });
            }
            var result = CS.GetDoctorBookingsByPatientId(patientId);
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
        [Route("GetLabServiceBookingsByPatientId")]
        public async Task<IActionResult> GetLabServiceBookingsByPatientId(string patientId)
        {
            if (!_validator.IsValidPatient(patientId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.NotValidPatient
                });
            }
            var result = CS.GetLabServiceBookingsByPatientId(patientId);
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
        [Route("GetNurseBookingsByPatientId")]
        public async Task<IActionResult> GetNurseBookingsByPatientId(string patientId)
        {
            if (!_validator.IsValidPatient(patientId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.NotValidPatient
                });
            }
            var result = CS.GetNurseBookingsByPatientId(patientId);
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
        public async Task<IActionResult> GetDoctorsBookingPayment(int bookingId)
        {

            if (!_validator.IsValidBooking(bookingId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Messages.NotValidSpecialization
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
