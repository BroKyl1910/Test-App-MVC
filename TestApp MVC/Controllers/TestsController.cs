using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            if (HttpContext.Session.GetString("Username") == null) return RedirectToAction("Login", "Users");

            User user = _context.User.First(u => u.Username.Equals(HttpContext.Session.GetString("Username")));
            List<Module> allModules = new List<Module>();
            List<Test> tests = new List<Test>();
            List<StudentTestsViewModel> viewModels = new List<StudentTestsViewModel>();
            bool isLectuer = user.UserType == 1;

            if (!isLectuer)
            {
                //Student
                allModules = _context.ModuleCourse.Where(mc => mc.CourseId == _context.StudentAssignment.First(sa => sa.Username == user.Username).CourseId).Select(mc => mc.Module).ToList();
                List<Module> modules = new List<Module>();
                if (moduleID != null && moduleID != "-1")
                {
                    //Filter modules based on parameter
                    modules = allModules.Where(m => m.ModuleId == moduleID).ToList();
                }
                else
                {
                    modules = allModules;
                }
                tests = _context.Test.Where(t => modules.Any(m => m.ModuleId == t.ModuleId) && t.Published == true).OrderBy(t => t.DueDate).ThenBy(t => t.Title).ToList();
                viewModels = tests.Select(t => new StudentTestsViewModel()
                {
                    Test = t,
                    HasTaken = _context.Result.Any(r => r.Username == user.Username && r.TestId == t.TestId)
                }).OrderBy(vm=>vm.HasTaken).ToList();
            }
            else
            {
                //Lecturer
                allModules = _context.LecturerAssignment.Where(la => la.Username == user.Username).Select(la => la.Module).ToList();
                List<Module> modules = new List<Module>();
                if (moduleID != null && moduleID != "-1")
                {
                    modules = allModules.Where(m => m.ModuleId == moduleID).ToList();
                }
                else
                {
                    modules = allModules;
                }
                // Show tests for specified module and that were created by logged in lecturer
                tests = _context.Test.Where(t => modules.Any(m => m.ModuleId == t.ModuleId) && t.Username == user.Username).OrderByDescending(t => t.DueDate).ThenBy(t => t.Title).ToList();

            }

            ViewBag.SelectedIndex = moduleID;
            ViewBag.Modules = allModules;
            ViewBag.Tests = tests;
            ViewBag.StudentTestsViewModels = viewModels;

            return (isLectuer) ? View("IndexLecturer") : View("IndexStudent");
        }



        // GET: Tests/Details/5
        public async Task<IActionResult> Edit(int testID)
        {
            if (HttpContext.Session.GetString("Username") == null) return RedirectToAction("Login", "Users");
            User user = _context.User.First(u => u.Username.Equals(HttpContext.Session.GetString("Username")));
            bool isLecturer = user.UserType == 1;
            if (!isLecturer)
            {
                //students unauthorised
                return RedirectToAction("Index");
            }
            Test test = _context.Test.First(t => t.TestId == testID);
            test.Module = _context.Module.First(m => m.ModuleId == test.ModuleId);
            var modules = _context.LecturerAssignment.Where(la => la.Username == user.Username).Select(la => la.Module).ToList();

            ViewBag.Modules = modules;
            ViewBag.Test = test;
            return View();
        }

        public JsonTest MemoQuestions(int testID)
        {
            Test dbTest = _context.Test.First(t => t.TestId == testID);
            JsonTest jsonTest = new JsonTest()
            {
                TestID = dbTest.TestId,
                Title = dbTest.Title,
                ModuleID = dbTest.ModuleId,
                DueDate = dbTest.DueDate.ToShortDateString()
            };

            List<Question> dbQuestions = _context.Question.Where(q => q.TestId == testID).ToList();
            List<JsonQuestion> jsonQuestions = dbQuestions.Select(q => new JsonQuestion()
            {
                QuestionID = q.QuestionId,
                QuestionText = q.QuestionText,
                Answer1 = q.Answer1,
                Answer2 = q.Answer2,
                Answer3 = q.Answer3,
                CorrectAnswer = q.CorrectAnswer
            }).ToList();

            jsonTest.Questions = jsonQuestions;

            return jsonTest;
        }

        [HttpPost]
        public async Task<string> Edit(Test test)
        {
            User user = _context.User.First(u => u.Username.Equals(HttpContext.Session.GetString("Username")));
            Test dbTest = _context.Test.First(t => t.TestId == test.TestId);
            dbTest.Username = user.Username;
            dbTest.ModuleId = test.ModuleId;
            dbTest.Title = test.Title;
            dbTest.DueDate = test.DueDate;
            dbTest.PublishDate = DateTime.Now.Date;

            _context.SaveChanges();

            List<Question> dbQuestions = _context.Question.Where(q=> q.TestId == test.TestId).ToList();
            List<Question> questions = test.Question.ToList();
            for (int i = 0; i < test.Question.Count; i++)
            {
                dbQuestions[i].QuestionText = questions[i].QuestionText;
                dbQuestions[i].Answer1 = questions[i].Answer1;
                dbQuestions[i].Answer2 = questions[i].Answer2;
                dbQuestions[i].Answer3 = questions[i].Answer3;
                dbQuestions[i].CorrectAnswer = questions[i].CorrectAnswer;
            }
            _context.SaveChanges();

            return "200";
        }

        // GET: Tests/Create
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Users");
            }

            if (HttpContext.Session.GetInt32("UserType") != 1)
            {
                //Unauthorized
                return RedirectToAction("Index", "Tests");
            }

            User user = _context.User.First(u => u.Username.Equals(HttpContext.Session.GetString("Username")));
            var modules = _context.LecturerAssignment.Where(la => la.Username == user.Username).Select(la => la.Module).ToList();

            ViewBag.Modules = modules;
            return View();
        }

        // POST: Tests/Create
        [HttpPost]
        public string Create(Test test)
        {
            User user = _context.User.First(u => u.Username.Equals(HttpContext.Session.GetString("Username")));
            Test dbTest = new Test()
            {
                Username = user.Username,
                ModuleId = test.ModuleId,
                Title = test.Title,
                DueDate = test.DueDate,
                PublishDate = DateTime.Now.Date,
                Published = true
            };

            _context.Test.Add(dbTest);
            _context.SaveChanges();

            List<Question> questions = test.Question.ToList();
            foreach (Question question in questions)
            {
                question.TestId = dbTest.TestId;
                _context.Question.Add(question);
            }

            _context.SaveChanges();

            return "200";
        }

        // GET: Tests/Take
        public async Task<IActionResult> Take(int testID)
        {
            if (HttpContext.Session.GetString("Username") == null) return RedirectToAction("Login", "Users");
            User user = _context.User.First(u => u.Username.Equals(HttpContext.Session.GetString("Username")));
            Test test = _context.Test.First(t => t.TestId == testID);

            ViewBag.TestID = test.TestId;
            ViewBag.TestTitle = test.Title;
            ViewBag.Module = _context.Module.First(m => m.ModuleId == test.ModuleId);
            ViewBag.DueDate = test.DueDate.ToShortDateString();
            return View();
        }

        // GET: Tests/Questions
        public List<Question> Questions(int testID)
        {
            List<Question> questions = _context.Question.Where(q => q.TestId == testID).ToList();
            return questions;
        }

        // GET: Tests/Answers
        public string Answers(int testID)
        {
            User user = _context.User.First(u => u.Username.Equals(HttpContext.Session.GetString("Username")));
            var answers = _context.Answer.Where(a => a.TestId == testID && a.Username == user.Username).Select(a => new
            {
                UserAnswer = a.UserAnswer,
                Correct = a.Correct
            }).ToList();
            return JsonConvert.SerializeObject(answers);
        }

        // POST: Tests/Take
        [HttpPost]
        public ActionResult Take(int testID, List<Int32> answers)
        {
            User user = _context.User.First(u => u.Username.Equals(HttpContext.Session.GetString("Username")));
            Test test = _context.Test.First(t => t.TestId == testID);
            List<Question> questions = _context.Question.Where(q => q.TestId == testID).ToList();

            List<Answer> dbAnswers = new List<Answer>();

            int attemptNumber = 1;

            for (int i = 0; i < questions.Count; i++)
            {
                Answer answer = new Answer();
                answer.AttemptNumber = attemptNumber;
                answer.QuestionId = questions[i].QuestionId;
                answer.Username = user.Username;
                answer.TestId = test.TestId;
                answer.UserAnswer = answers[i];
                answer.Correct = answers[i] == questions[i].CorrectAnswer;

                _context.Answer.Add(answer);
            }

            _context.SaveChanges();

            // Calculate and save result
            int numCorrect = _context.Answer.Count(a => a.TestId == test.TestId && a.Username.Equals(user.Username) && a.AttemptNumber == attemptNumber && a.Correct);

            Result result = new Result();
            result.TestId = test.TestId;
            result.Username = user.Username;
            result.AttemptNumber = attemptNumber;
            result.UserResult = numCorrect;
            result.ResultPercentage = (decimal)Math.Round(((double)numCorrect) / questions.Count * 100, 3);
            result.ResultDate = DateTime.Now.Date;
            _context.Result.Add(result);


            _context.SaveChanges();

            return View();
        }

        // PUT: Tests/Publish
        [HttpPut]
        public void Publish(int testID, bool published)
        {
            Test test = _context.Test.First(t => t.TestId == testID);
            test.Published = published;
            _context.SaveChanges();
        }

        // GET: Tests/Memo
        public ActionResult Memo(int testID)
        {
            if (HttpContext.Session.GetString("Username") == null) return RedirectToAction("Login", "Users");
            User user = _context.User.First(u => u.Username.Equals(HttpContext.Session.GetString("Username")));
            Test test = _context.Test.First(t => t.TestId == testID);
            test.Module = _context.Module.First(m => m.ModuleId == test.ModuleId);
            bool isLecturer = user.UserType == 1;
            if (!isLecturer)
            {
                //student
                int testPerc = (int)_context.Result.First(r => r.Username == user.Username && r.TestId == testID).ResultPercentage;
                ViewBag.TestPerc = testPerc;

            }
            ViewBag.Test = test;
            return isLecturer ? View("ViewMemoLecturer") : View("ViewMemoStudent");
        }

        private bool TestExists(int id)
        {
            return _context.Test.Any(e => e.TestId == id);
        }
    }
}
