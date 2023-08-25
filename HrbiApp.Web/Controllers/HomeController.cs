using DBContext;
using HrbiApp.Web.Areas.Common;
using HrbiApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace HrbiApp.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        CoreServices CS;
        public HomeController(ILogger<HomeController> logger,CoreServices cs)
        {
            _logger = logger;
            CS = cs;
        }

        public IActionResult Index()
        {
            return RedirectToAction( "AllDoctorPayments", "Payments");
            return View();
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public JsonResult SaveAdminInstanceID(string instanceID)
        {
            var userIDClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return Json( CS.SaveAdminInstanceID(userIDClaim.Value, instanceID));
        }
    }
}