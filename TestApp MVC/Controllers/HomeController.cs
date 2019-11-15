using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestApp.MVC.Models;
using TestApp_MVC.Models;

namespace TestApp_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly TestAppContext _context;

        public HomeController(TestAppContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Users");
            }

            User user = _context.User.First(u => u.Username.Equals(HttpContext.Session.GetString("Username")));

            var modules = _context.LecturerAssignment.Where(la => la.Username.Equals(user.Username)).Select(la => la.Module).ToList();
            List<Test> tests = _context.Test.Where(t => t.Username.Equals(user.Username)).OrderBy(t => t.DueDate).OrderByDescending(t => t.DueDate).ThenBy(t => t.Title).Take(6).ToList();

            ViewBag.Tests = tests;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
