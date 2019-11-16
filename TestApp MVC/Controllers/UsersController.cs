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
    public class UsersController : Controller
    {
        private readonly TestAppContext _context;

        public UsersController(TestAppContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if(username == null || password == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }
            User user = new User();
            AuthenticateUser(username, password, out user);
            if(user == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetInt32("UserType", user.UserType);

            return RedirectToAction("Index", "Home");
        }

        private void AuthenticateUser(string username, string password, out User user)
        {
            user = null;
            foreach (User u in _context.User)
            {
                if (u.Username.ToLower().Equals(username.ToLower()) && BCrypt.CheckPassword(password, u.Password))
                {
                    user = u;
                    break;
                }
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Users");
        }

        // GET: Users/Register
        public IActionResult Register()
        {
            var modules = _context.Module.ToList();
            var courses = _context.Course.ToList();
            ViewBag.Modules = modules;
            ViewBag.Courses = courses;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> Validate([Bind("Username,Password,FirstName,Surname,UserType,UniversityIdentification")] User user, string confirmPassword, string[] modules, string course)
        {
            if (_context.User.Any(u => u.Username.Equals(user.Username)))
            {
                return "Username already taken";
            }

            if (!user.Password.Equals(confirmPassword))
            {
                return "Passwords do not match";
            }

            //Students need to have selected a course
            if (user.UserType == 0 && course == null)
            {
                return "Please select a course";
            }

            //Lecturers need to have selected modules
            if (user.UserType == 1 && modules[0].Equals("null"))
            {
                return "Please select a module";
            }


            if (ModelState.IsValid)
            {
                return "OK";
            }
            return ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)).ToList()[0]; ;
        }

        // POST: Users/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Password,FirstName,Surname,UserType,UniversityIdentification")] User user, string confirmPassword, string[] modules, string course)
        {
            user.Password = BCrypt.HashPassword(user.Password, BCrypt.GenerateSalt());
            _context.Add(user);
            await _context.SaveChangesAsync();
            if (user.UserType == 0)
            {
                //Student
                StudentAssignment studentAssignment = new StudentAssignment();
                studentAssignment.Username = user.Username;
                studentAssignment.CourseId = course;

                _context.StudentAssignment.Add(studentAssignment);
            }
            else
            {
                //Lecturer
                // JS sends modules as comma delimited string
                foreach (string module in modules[0].Split(','))
                {
                    LecturerAssignment la = new LecturerAssignment();
                    la.ModuleId = module;
                    la.Username = user.Username;

                    _context.LecturerAssignment.Add(la);
                }
            }

            _context.SaveChanges();

            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetInt32("UserType", user.UserType);
            return View("Index","Home");
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.Username == id);
        }
    }
}
