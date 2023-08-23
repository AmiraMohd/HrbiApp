using DBContext;
using HrbiApp.API.Models.Doctor;
using HrbiApp.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HrbiApp.API.Models.Patient;
using HrbiApp.API.Models.Account;

namespace HrbiApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : APIController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public PatientController(
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
        public async Task<IActionResult> Register([FromBody] PatientRegisterRequestModel model)
        {
            try
            {
                if (_validator.IsValidPhone(model.PhoneNumber))
                {
                    return Ok(new BaseResponse
                    {
                        Message = Messages.NotValidPhone
                    });
                }
                var register = await CS.PatientRegister(model);
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
        public async Task<IActionResult> Login([FromBody] PatientLoginModel model)
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
            var login = await CS.PatientLogin(model);
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
    }
}
