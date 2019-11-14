using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        //Add in custom validation for uniqueness
        [Required(ErrorMessage = "Please enter username")]
        public string Username { get; set; }

        // Add in custom validation for complexity
        [Required(ErrorMessage = "Please enter password")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter surname")]
        public string Surname { get; set; }

        [Required]
        public int UserType { get; set; }

        [Required]
        public string UniversityIdentification { get; set; }

        public ICollection<Answer> Answer { get; set; }
        public ICollection<LecturerAssignment> LecturerAssignment { get; set; }
        public ICollection<Result> Result { get; set; }
        public ICollection<StudentAssignment> StudentAssignment { get; set; }
        public ICollection<Test> Test { get; set; }
    }
}
