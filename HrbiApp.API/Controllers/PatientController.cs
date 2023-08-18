using DBContext;
using HrbiApp.API.Models.Doctor;
using HrbiApp.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HrbiApp.API.Models.Patient;

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
                        Message = Responses.NotValidPhone
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
                    Message = Responses.NotActiveAccount
                });
            }
            if (!_validator.IsValidPhone(model.PhoneNumber))
            {
                return Ok(new BaseResponse()
                {
                    Message = Responses.NotValidPhone
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
    }
}
