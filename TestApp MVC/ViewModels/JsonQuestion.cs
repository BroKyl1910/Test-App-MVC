namespace TestApp.MVC.ViewModels
{
    public class JsonQuestion
    {
        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public int CorrectAnswer { get; set; }
    }
}