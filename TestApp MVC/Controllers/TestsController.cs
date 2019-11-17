using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestApp.MVC.Models;
using TestApp.MVC.ViewModels;

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
        public async Task<IActionResult> Index(string moduleID)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Users");
            }

            User user = _context.User.First(u => u.Username.Equals(HttpContext.Session.GetString("Username")));
            List<Module> allModules = new List<Module>();
            List<Test> tests = new List<Test>();
            bool isLectuer = user.UserType == 1;
            bool specifiedModule = false;
            if (!isLectuer)
            {
                //Student
                List<StudentTestsViewModel> viewModels = new List<StudentTestsViewModel>();
                allModules = _context.ModuleCourse.Where(mc => mc.CourseId == _context.StudentAssignment.First(sa => sa.Username == user.Username).CourseId).Select(mc => mc.Module).ToList();
                List<Module> modules = new List<Module>();
                if (moduleID != null && moduleID != "-1")
                {
                    //Filter modules based on parameter
                    modules = allModules.Where(m => m.ModuleId == moduleID).ToList();
                    specifiedModule = true;
                }
                else
                {
                    modules = allModules;
                }
                tests = _context.Test.Where(t => modules.Any(m => m.ModuleId == t.ModuleId)).OrderBy(t => t.DueDate).ThenBy(t => t.Title).ToList();
            }
            else
            {
                //Lecturer
                allModules = _context.LecturerAssignment.Where(la => la.Username == user.Username).Select(la => la.Module).ToList();
                List<Module> modules = new List<Module>();
                if (moduleID != null && moduleID != "-1")
                {
                    modules = allModules.Where(m => m.ModuleId == moduleID).ToList();
                    specifiedModule = true;
                }
                else
                {
                    modules = allModules;
                }
                // Show tests for specified module and that were created by logged in lecturer
                tests = _context.Test.Where(t => modules.Any(m => m.ModuleId == t.ModuleId) && t.Username == user.Username).ToList();

            }

            ViewBag.SelectedIndex = moduleID;
            ViewBag.Modules = allModules;
            ViewBag.Tests = tests;

            return (isLectuer) ? View("IndexLecturer") : View("IndexStudent");
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
