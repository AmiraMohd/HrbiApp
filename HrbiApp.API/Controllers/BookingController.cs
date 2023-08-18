using HrbiApp.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DBContext;
using Microsoft.AspNetCore.Identity;
using Azure;
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

                    Message = Responses.NotValidDoctor
                });
            }
            if (!_validator.IsValidPatient(model.PatientId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Responses.NotValidPatient
                });
            }
            var result = CS.PlaceDoctorBooking(model);

            if (result.Result == true)
            {
                return Ok(new BaseResponse()
                {
                    Status = true,
                    Message = Responses.SuccessfulBooking
                });
            }
            else
            {
                return Ok(new BaseResponse()
                {
                    Message = Responses.BookingFailed
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

                    Message = Responses.NotValidNurseService
                });
            }
            if (!_validator.IsValidPatient(model.PatientId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Responses.NotValidPatient
                });
            }
            var result = CS.PlaceNurseServiceBooking(model);

            if (result.Result == true)
            {
                return Ok(new BaseResponse()
                {
                    Status = true,
                    Message = Responses.SuccessfulBooking
                });
            }
            else
            {
                return Ok(new BaseResponse()
                {
                    Message = Responses.BookingFailed
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

                    Message = Responses.NotValidLabService
                });
            }
            if (!_validator.IsValidPatient(model.PatientId))
            {
                return Ok(new BaseResponse()
                {

                    Message = Responses.NotValidPatient
                });
            }
            var result = CS.PlaceLabServiceBooking(model);

            if (result.Result == true)
            {
                return Ok(new BaseResponse()
                {
                    Status = true,
                    Message = Responses.SuccessfulBooking
                });
            }
            else
            {
                return Ok(new BaseResponse()
                {
                    Message = Responses.BookingFailed
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
                    Message = Responses.BookingAlreadyRejected
                });
            }
            if (!_validator.IsValidDoctorToAcceptRejectBooking(doctorId,bookingId)) {
                return Ok(new BaseResponse()
                {
                    Message = Responses.NotValidDoctor
                });

            }
            var result = CS.AcceptBooking(bookingId);
            if(!result.Result)
            {
                return Ok(new BaseResponse()
                {
                    Message = Responses.Failed
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
                    Message = Responses.BookingAlreadyAccepted
                });
            }
            if (!_validator.IsValidDoctorToAcceptRejectBooking(doctorId, bookingId))
            {
                return Ok(new BaseResponse()
                {
                    Message = Responses.NotValidDoctor
                });

            }
            var result = CS.RejectBooking(bookingId);
            if (!result.Result)
            {
                return Ok(new BaseResponse()
                {
                    Message = Responses.Failed
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
    }
}
