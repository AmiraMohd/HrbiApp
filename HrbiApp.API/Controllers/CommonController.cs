﻿using DBContext;
using HrbiApp.API.Models;
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
        [Route("GetAllDoctorSpeicalizations")]
        public async Task<IActionResult> GetAllDoctorSpeicalizations()
        {

            var result = CS.GetAllSpecializations();
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
                    Data = result.Response
                });

            }
        }

        [HttpGet]
        [Route("GetAllDoctorPosotions")]
        public async Task<IActionResult> GetAllDoctorPosotions()
        {

            var result = CS.GetAllDoctorPositions();
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
                    Data = result.Response
                });

            }
        }
    }
}