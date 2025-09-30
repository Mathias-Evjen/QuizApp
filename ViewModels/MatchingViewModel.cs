using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class MatchingViewModel
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public int AmountCorrect { get; set; }

        public MatchingViewModel() { }

        public MatchingViewModel(int id, string question)
        {
            Id = id;
            Question = question;
        }
    }

}