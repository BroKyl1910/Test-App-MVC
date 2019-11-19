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

namespace TestApp_MVC.Controllers
{
    public class GraphsController : Controller
    {
        private readonly TestAppContext _context;

        public GraphsController(TestAppContext context)
        {
            _context = context;
        }

        public string TestSubmissionsPerDate(int testID)
        {
            Test test = _context.Test.First(t => t.TestId == testID);
            List<Result> results = _context.Result.Where(r => r.TestId == testID).ToList();
            List<string> xDates = new List<string>();
            List<int> ySubmissions = new List<int>();

            DateTime startDate = test.PublishDate;
            DateTime endDate = DateTime.Now;

            DateTime currentDate = startDate;
            while (currentDate.CompareTo(endDate) < 0)
            {
                if (results.Any(r => r.ResultDate.Date.Equals(currentDate.Date)))
                {
                    xDates.Add(currentDate.ToShortDateString().Replace('/', '-'));
                    ySubmissions.Add(results.Count(r => r.ResultDate.Equals(currentDate)));
                }

                currentDate = currentDate.AddDays(1);
            }

            int totalSubmissions = ySubmissions.Count;


            return JsonConvert.SerializeObject(new {
                totalSubmissions,
                ySubmissions,
                xDates
            });
        }



        public string AveragePerModule(string moduleID)
        {
            User user = _context.User.First(u => u.Username == HttpContext.Session.GetString("Username"));
            Module module = _context.Module.First(m => m.ModuleId == moduleID);
            var tests = _context.Test.Where(t => t.ModuleId == moduleID && t.Username==user.Username).ToList();
            var results = _context.Result.Where(r => tests.Any(t => t.TestId == r.TestId)).ToList();

            List<double> averages = new List<double>();
            foreach(Test test in tests)
            {
                if (results.Any(r => r.TestId == test.TestId))
                {
                    averages.Add((double)results.Where(r => r.TestId == test.TestId).Average(r => r.ResultPercentage));
                }
                else
                {
                    averages.Add(0);
                }
            }


            List<string> xTests = tests.Select(t => t.Title).ToList();
            List<int> yAverages = averages.Select(a=> (int) a).ToList();

            return JsonConvert.SerializeObject(new
            {
                testIDs = tests.Select(t=> t.TestId).ToList(),
               xTests,
               yAverages
            });
        }

        public string TestResults(int testID)
        {
            Test test = _context.Test.First(t => t.TestId == testID);
            List<Result> results = _context.Result.Where(r => r.TestId == testID).ToList();

            return JsonConvert.SerializeObject(results.Select(r => new {
                title = test.Title,
                name = _context.User.First(u => r.Username == u.Username).FirstName + " " + _context.User.First(u => r.Username == u.Username).Surname,
                result = r.ResultPercentage
            }));
        }

    }
}


