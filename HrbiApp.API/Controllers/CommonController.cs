using DBContext;
using HrbiApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HrbiApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : APIController
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public CommonController(
            IConfiguration configuration, ApplicationDBContext db,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,
            IServiceScopeFactory serviceScopeFactory) : base(db, configuration, userManager, signInManager, serviceScopeFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }


        [HttpGet]
        [Route("GetAllServices")]
        public async Task<IActionResult> GetAllServices()
        {

            var result = CS.GetAllServices();
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
                    Status = true,
                    Data = result.Response
                }); ;

            }
        }

        [HttpGet]
        [Route("GetAllLabServices")]
        public async Task<IActionResult> GetAllLabServices()
        {

            var result = CS.GetAllLabServices();
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
                    Status = true,
                    Data = result.Response
                }); ;

            }
        }

        [HttpGet]
        [Route("GetAllNurseServices")]
        public async Task<IActionResult> GetAllNurseServices()
        {

            var result = CS.GetAllNurseServices();
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
                    Status = true,
                    Data = result.Response
                }); ;

            }
        }

        [HttpGet]
        [Route("GetAllDoctorSpeicalizations")]
        public async Task<IActionResult> GetAllDoctorSpeicalizations()
        {

            var result = CS.GetAllSpecializations();
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
                    Status = true,
                    Data = result.Response
                });

            }
        }

        [HttpGet]
        [Route("GetAllDoctorPostions")]
        public async Task<IActionResult> GetAllDoctorPostions()
        {

            var result = CS.GetAllDoctorPositions();
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
                    Status = true,
                    Data = result.Response
                });

            }
        }

        [Authorize]
        [HttpGet("SaveInstanceID")]
        public async Task<IActionResult> SaveInstanceID(string instanceID)
        {
            var userID = User.Claims.FirstOrDefault(x => x.Type.Equals(Consts.UserIDClaimName, StringComparison.InvariantCultureIgnoreCase));
            var result = CS.SaveInstanceID(instanceID, userID.Value);
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
        
        [Authorize]
        [HttpGet("GetNotifications")]
        public IActionResult GetNotifications()
        {
            var userID = User.Claims.FirstOrDefault(x => x.Type.Equals(Consts.UserIDClaimName, StringComparison.InvariantCultureIgnoreCase));

            var getNotifications = CS.GetUserNotifications(userID.Value);
            if (!getNotifications.Result)
            {
                return Ok(new BaseResponse()
                {
                    Message = Messages.ExceptionOccured
                });
            }
            return Ok(new BaseResponse() { Data = getNotifications.Notifications, Status = true });
        }

        [HttpGet("GetOTP")]
        public IActionResult GetOTP(string phoneNumber)
        {
            var otp = _db.OTPs.OrderBy(a=>a.ID).LastOrDefault(a => a.Phone == phoneNumber && a.Purpose == Consts.ConfirmationPurpose).Code;
            if (otp != null)
                return Ok(new BaseResponse()
                {
                    Status = true,
                    Message = otp
                });
            else
                return Ok(new BaseResponse()
                {
                    Status = false
                });

        }
    }
}
