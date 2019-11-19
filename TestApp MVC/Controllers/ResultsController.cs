using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestApp.MVC.Models;

namespace TestApp_MVC.Controllers
{
    public class ResultsController : Controller
    {
        private readonly TestAppContext _context;
        public ResultsController(TestAppContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            User user = _context.User.First(u => u.Username == HttpContext.Session.GetString("Username"));
            if (user.UserType == 0) return View("Index", "Tests");

            bool hasTests = _context.Test.Any(t => t.Username == user.Username);
            List<Module> modules = _context.LecturerAssignment.Where(la => la.Username == user.Username).Select(la => la.Module).ToList();

            ViewBag.HasTests = hasTests;
            ViewBag.Modules = modules;
            return View();
        }
    }
}