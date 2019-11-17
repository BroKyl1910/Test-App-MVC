using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.MVC.Models;

namespace TestApp.MVC.ViewModels
{
    public class StudentTestsViewModel
    {
        public Test Test { get; set; }
        public  bool HasTaken { get; set; }
    }
}
