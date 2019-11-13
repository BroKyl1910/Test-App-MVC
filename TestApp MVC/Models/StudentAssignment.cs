using System;
using System.Collections.Generic;

namespace TestApp.MVC.Models
{
    public partial class StudentAssignment
    {
        public int StudentAssignmentId { get; set; }
        public string Username { get; set; }
        public string CourseId { get; set; }

        public Course Course { get; set; }
        public User UsernameNavigation { get; set; }
    }
}
