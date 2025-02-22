﻿using System;
using System.Collections.Generic;

namespace TestApp.MVC.Models
{
    public partial class Question
    {
        public Question()
        {
            Answer = new HashSet<Answer>();
        }

        public int QuestionId { get; set; }
        public int TestId { get; set; }
        public string QuestionText { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public int CorrectAnswer { get; set; }

        public Test Test { get; set; }
        public ICollection<Answer> Answer { get; set; }
    }
}
