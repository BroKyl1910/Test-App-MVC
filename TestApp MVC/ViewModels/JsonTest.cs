using System.Collections.Generic;

namespace TestApp.MVC.ViewModels
{
    public class JsonTest
    {
        public int TestID { get; set; }
        public string Title { get; set; }
        public string ModuleID { get; set; }
        public string DueDate { get; set; }
        public List<JsonQuestion> Questions { get; set; }
    }
}