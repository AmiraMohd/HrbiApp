using DBContext;
using HrbiApp.API.Models;
using HrbiApp.API.Models.Account;
using HrbiApp.API.Models.Doctor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HrbiApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class DoctorController : APIController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public DoctorController(
            IConfiguration configuration, ApplicationDBContext db,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,
            IServiceScopeFactory serviceScopeFactory) : base(db, configuration, userManager, signInManager, serviceScopeFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] DoctorRegisterRequestModel model)
        {
            try
            {
                if (_validator.IsValidPhone(model.PhoneNumber))
                {
                    return Ok(new BaseResponse
                    {
                        Message = Messages.NotValidPhone
                    }) ;
                }
                var register = await CS.DoctorRegister(model);
                if (!register.Result)
                {
                    return Ok(new BaseResponse() { Message = register.Message });
                }
                return Ok(new BaseResponse() { Status = true });

            }
            catch (Exception)
            {
                return Ok();
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] DoctorLoginModel model)
        {

            if (!_validator.IsActiveAccount(model.PhoneNumber))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.NotActiveAccount
                });
            }
            if (!_validator.IsValidPhone(model.PhoneNumber))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.NotValidPhone
                });
            }
            var login = await CS.DoctorLogin(model);
            if (!login.Result)
            {
                return Ok(new BaseResponse()
                {
                    Message = login.Response.Message
                });
            }
            return Ok(new BaseResponse()
            {
                Status = true,
                Data = login.Response

            });
        }

        [HttpGet("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string phone)
        {
            if (!_validator.IsActiveAccount(phone))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.NotActiveAccount
                });
            }
            if (!_validator.IsValidPhone(phone))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.NotValidPhone
                });
            }
            var result = await CS.ForgetPasswordAsync(phone);
            if (!result)
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.ExceptionOccured
                });
            }
            return Ok(new BaseResponse()
            {
                Status = true
            });
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword model)
        {
            if (!_validator.IsValidPhoneAndOTP(model.Phone, model.OTP, Consts.ResetPurpose))
            {
                return Ok(new BaseResponse()
                {
                    Status = false,
                    Message = Messages.NotValidPhoneAndOTP
                });
            }
            var result = await CS.ResetPassword(model);
            if (!result)
            {
                return Ok(new BaseResponse()
                {
                    Status = false,
                });
            }
            return Ok(new BaseResponse()
            {
                Status = true,
            });
        }

        [HttpGet("GetLoginOTP/{phone}")]
        public async Task<IActionResult> GetLoginOTP(string phone)
        {
            if (!_validator.IsActiveAccount(phone))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.NotActiveAccount
                });
            }
            if (!_validator.IsValidPhone(phone))
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.NotValidPhone
                });
            }
            
            var sendLoginOTP = CS.SendLoginOTP(phone);
            if (!sendLoginOTP)
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.ExceptionOccured
                });
            }
            return Ok(new BaseResponse()
            {
                Status = true,
            });

        }

        [HttpGet]
        [Route("GetDoctors")]
        public async Task<IActionResult> DoctorsList()
        {
            var result = CS.GetDoctorsList();
            if(result.Result == true)
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

        [HttpGet]
        [Route("GetDoctorsBySpecialization/{specializationId}")]
        public async Task<IActionResult> GetDoctorsBySpecialization(int specializationId)
        {

            if (!_validator.IsValidSpecialization(specializationId))
            {
                return Ok(new BaseResponse()
                {
             
                    Message = Messages.NotValidSpecialization
                });
            }
            var result = CS.GetDoctorsBySpecialization(specializationId);
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
