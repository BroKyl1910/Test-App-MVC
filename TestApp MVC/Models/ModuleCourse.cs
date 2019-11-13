using System;
using System.Collections.Generic;

namespace TestApp.MVC.Models
{
    public partial class ModuleCourse
    {
        public int ModuleCourseId { get; set; }
        public string ModuleId { get; set; }
        public string CourseId { get; set; }

        public Course Course { get; set; }
        public Module Module { get; set; }
    }
}
