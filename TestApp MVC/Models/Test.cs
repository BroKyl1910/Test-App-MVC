using System;
using System.Collections.Generic;

namespace TestApp.MVC.Models
{
    public partial class Test
    {
        public Test()
        {
            Answer = new HashSet<Answer>();
            Question = new HashSet<Question>();
            Result = new HashSet<Result>();
        }

        public int TestId { get; set; }
        public string Username { get; set; }
        public string ModuleId { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime PublishDate { get; set; }

        public Module Module { get; set; }
        public User UsernameNavigation { get; set; }
        public ICollection<Answer> Answer { get; set; }
        public ICollection<Question> Question { get; set; }
        public ICollection<Result> Result { get; set; }
    }
}
