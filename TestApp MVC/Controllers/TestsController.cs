using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestApp.MVC.Models;

namespace TestApp_MVC.Controllers
{
    public class TestsController : Controller
    {
        private readonly TestAppContext _context;

        public TestsController(TestAppContext context)
        {
            _context = context;
        }

        // GET: Tests
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Users");
            }

            User user = _context.User.First(u => u.Username.Equals(HttpContext.Session.GetString("Username")));
            //if (user.UserType == 0)
            //{
            //    //Student
            //}
            ////Lecturer
            var modules = _context.LecturerAssignment.Where(la => la.Username.Equals(user.Username)).Select(la => la.Module).ToList();
            List<Test> tests = _context.Test.Where(t => t.Username.Equals(user.Username)).OrderBy(t => t.DueDate).ThenBy(t => t.Title).Take(6).ToList();

            ViewBag.Modules = modules;
            ViewBag.Tests = tests;

            return View();
        }

        public async Task<IActionResult> Filter(string moduleID)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Users");
            }

            User user = _context.User.First(u => u.Username.Equals(HttpContext.Session.GetString("Username")));
            //if (user.UserType == 0)
            //{
            //    //Student
            //}
            ////Lecturer
            var modules = _context.LecturerAssignment.Where(la => la.Username.Equals(user.Username)).Select(la => la.Module).ToList();
            List<Test> tests = new List<Test>();
            if (moduleID == "-1")
            {
                tests = _context.Test.Where(t => t.Username.Equals(user.Username)).OrderBy(t => t.DueDate).ThenBy(t => t.Title).Take(6).ToList();
            }
            else
            {
                tests = _context.Test.Where(t => t.Username.Equals(user.Username) && t.ModuleId == moduleID).OrderBy(t => t.DueDate).ThenBy(t => t.Title).Take(6).ToList();
            }

            ViewBag.SelectedIndex = moduleID;
            ViewBag.Modules = modules;
            ViewBag.Tests = tests;

            return View("Index");
        }


        // GET: Tests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Test
                .Include(t => t.Module)
                .Include(t => t.UsernameNavigation)
                .FirstOrDefaultAsync(m => m.TestId == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        private bool TestExists(int id)
        {
            return _context.Test.Any(e => e.TestId == id);
        }
    }
}
