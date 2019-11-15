using System;
using System.Collections.Generic;

namespace TestApp.MVC.Models
{
    public partial class Result
    {
        public int ResultId { get; set; }
        public int? TestId { get; set; }
        public string Username { get; set; }
        public int AttemptNumber { get; set; }
        public int UserResult { get; set; }
        public decimal ResultPercentage { get; set; }
        public DateTime ResultDate { get; set; }

        public Test Test { get; set; }
        public User UsernameNavigation { get; set; }
    }
}
