using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.MVC.ViewModels
{
    public class StudentAnswerViewModel
    {
        public int UserAnswer { get; set; }
        public int CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
