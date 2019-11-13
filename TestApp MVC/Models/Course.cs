using System;
using System.Collections.Generic;

namespace TestApp.MVC.Models
{
    public partial class Course
    {
        public Course()
        {
            ModuleCourse = new HashSet<ModuleCourse>();
            StudentAssignment = new HashSet<StudentAssignment>();
        }

        public string CourseId { get; set; }
        public string CourseName { get; set; }

        public ICollection<ModuleCourse> ModuleCourse { get; set; }
        public ICollection<StudentAssignment> StudentAssignment { get; set; }
    }
}
