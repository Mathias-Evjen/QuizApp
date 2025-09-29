using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class FillInTheBlankViewModel
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string UserAnswer { get; set; }
        public bool? IsAnswerCorrect { get; set; }

        public FillInTheBlankViewModel() {}

        public FillInTheBlankViewModel(int id, string question)
        {
            Id = id;
            Question = question;
        }
    }
}