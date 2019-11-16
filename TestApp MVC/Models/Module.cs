using System;
using System.Collections.Generic;

namespace TestApp.MVC.Models
{
    public partial class Module
    {
        public Module()
        {
            LecturerAssignment = new HashSet<LecturerAssignment>();
            ModuleCourse = new HashSet<ModuleCourse>();
            Test = new HashSet<Test>();
        }

        public string ModuleId { get; set; }
        public string ModuleName { get; set; }

        public ICollection<LecturerAssignment> LecturerAssignment { get; set; }
        public ICollection<ModuleCourse> ModuleCourse { get; set; }
        public ICollection<Test> Test { get; set; }

        public override string ToString()
        {
            return ModuleName;
        }
    }
}
