using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            List<Result> results = _context.Result.Where(r => r.TestId==testID).ToList();
            List<string> xDates = new List<string>();
            List<int> ySubmissions = new List<int>();

            DateTime startDate = test.PublishDate;
            DateTime endDate = DateTime.Now;

            DateTime currentDate = startDate;
            while(currentDate.CompareTo(endDate) < 0)
            {
                if(results.Any(r=> r.ResultDate.Date.Equals(currentDate.Date)))
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

    }
}
