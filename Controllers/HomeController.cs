using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UkrMemesWeb.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace UkrMemesWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _IConfiguraton;
        private static List<UserClass> UsersList = new List<UserClass>();

        public HomeController(ILogger<HomeController> logger, IConfiguration ICongig)
        {
            _logger = logger;
            _IConfiguraton = ICongig;
        }

        public IActionResult Index()
        {
            var someUser = new UserClass();
            return View(someUser);
        }

        public IActionResult LoginUser(UserClass someUser)
        {
            UsersList.Add(someUser);
            if (someUser._IsAdmin)
            {
                return RedirectToAction(nameof(Admin));
            }
            return RedirectToAction(nameof(User));
        }

        public IActionResult RegUser(UserClass someUser)
        {
            UsersList.Add(someUser);
            return RedirectToAction(nameof(User));
        }

        public IActionResult LogOut()
        {
            return RedirectToAction(nameof(Index));
        }

        [Route("/user")]
        public IActionResult User()
        {
            return View(UsersList);
        }

        [Route("/admin")]
        public IActionResult Admin()
        {
            return View(UsersList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
