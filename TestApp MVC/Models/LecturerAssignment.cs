using System;
using System.Collections.Generic;

namespace TestApp.MVC.Models
{
    public partial class LecturerAssignment
    {
        public int LecturerAssignmentId { get; set; }
        public string Username { get; set; }
        public string ModuleId { get; set; }

        public Module Module { get; set; }
        public User UsernameNavigation { get; set; }
    }
}
