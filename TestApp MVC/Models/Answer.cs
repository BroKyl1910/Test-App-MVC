using System;
using System.Collections.Generic;

namespace TestApp.MVC.Models
{
    public partial class Answer
    {
        public int AnswerId { get; set; }
        public int TestId { get; set; }
        public int QuestionId { get; set; }
        public string Username { get; set; }
        public int AttemptNumber { get; set; }
        public int UserAnswer { get; set; }
        public bool Correct { get; set; }

        public Question Question { get; set; }
        public Test Test { get; set; }
        public User UsernameNavigation { get; set; }
    }
}
