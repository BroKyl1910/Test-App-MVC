using System;
using System.Collections.Generic;

namespace TestApp.MVC.Models
{
    public partial class User
    {
        public User()
        {
            Answer = new HashSet<Answer>();
            LecturerAssignment = new HashSet<LecturerAssignment>();
            Result = new HashSet<Result>();
            StudentAssignment = new HashSet<StudentAssignment>();
            Test = new HashSet<Test>();
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int UserType { get; set; }
        public string UniversityIdentification { get; set; }

        public ICollection<Answer> Answer { get; set; }
        public ICollection<LecturerAssignment> LecturerAssignment { get; set; }
        public ICollection<Result> Result { get; set; }
        public ICollection<StudentAssignment> StudentAssignment { get; set; }
        public ICollection<Test> Test { get; set; }
    }
}
