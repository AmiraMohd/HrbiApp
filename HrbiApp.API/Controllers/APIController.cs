using DBContext;
using HrbiApp.API.Helpers;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HrbiApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        protected CoreServices CS;
        protected UserManager<ApplicationUser> _userManager;
        protected SignInManager<ApplicationUser> _signManager;
        protected ExceptionHandler _ex;
        protected ApplicationDBContext _db;
        protected APIValidators _validator;
        protected IConfiguration _configuration;

        public APIController(ApplicationDBContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IServiceScopeFactory serviceScopeFactory)
        {
            _db = db;
            _configuration = configuration;
            CS = new CoreServices(db, configuration, userManager, signInManager, serviceScopeFactory);
            _ex = new ExceptionHandler(db);
            _validator = new APIValidators(db, configuration);

        }
        public APIController(ApplicationDBContext db, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _db = db;
            _configuration = configuration;
            //CS = new CoreServices(db, configuration, serviceScopeFactory);
            _ex = new ExceptionHandler(db);
            _validator = new APIValidators(db, configuration);


        }
    }
}
