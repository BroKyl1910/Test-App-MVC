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

            List<Test> tests = _context.Test.Where(t => t.Username == user.Username).OrderByDescending(t => t.DueDate).ThenBy(t => t.Title).ToList();
            List<Module> modules = _context.LecturerAssignment.Where(la => la.Username == user.Username).Select(la => la.Module).ToList();

            ViewBag.Tests = tests;
            ViewBag.Modules = modules;
            return View();
        }
    }
}