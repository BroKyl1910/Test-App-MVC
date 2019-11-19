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
            if (user.UserType == 0)
            {
                return IndexStudent(user);
            }
            return IndexLecturer(user);
        }

        private IActionResult IndexLecturer(User user)
        {
            var modules = _context.LecturerAssignment.Where(la => la.Username.Equals(user.Username)).Select(la => la.Module).ToList();
            List<Test> tests = _context.Test.Where(t => t.Username.Equals(user.Username) && t.DueDate.CompareTo(DateTime.Today) >= 0).OrderBy(t => t.DueDate).ThenBy(t => t.Title).Take(6).ToList();

            ViewBag.Tests = tests;
            return View("IndexLecturer");
        }

        private IActionResult IndexStudent(User user)
        {
            //Get list of modules the student does
            List<Course> studentCourses = _context.StudentAssignment.Where(sa => sa.Username == user.Username).Select(sa => sa.Course).ToList();
            List<Module> studentModules = _context.ModuleCourse.Where(mc => studentCourses.Any(sc => sc.CourseId == mc.CourseId)).Select(mc => mc.Module).ToList();
            List<Test> tests = _context.Test.Where(t => studentModules.Any(sm => sm.ModuleId == t.ModuleId) && t.Published == true && t.DueDate.CompareTo(DateTime.Today) >= 0).OrderBy(t => t.DueDate).ThenBy(t => t.Title).ToList();

            ViewBag.Tests = tests.Where(t => !_context.Result.Any(r => r.Username == user.Username && r.TestId == t.TestId)).Take(6).ToList();
            return View("IndexStudent");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
