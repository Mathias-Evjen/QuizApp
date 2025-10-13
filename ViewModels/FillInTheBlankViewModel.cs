using System.ComponentModel.DataAnnotations;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class FillInTheBlankViewModel
    {
        public int FillInTheBlankId { get; set; }
        public string Question { get; set; } = string.Empty;

        [Required(ErrorMessage = "Must answer")]
        public string UserAnswer { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public bool? IsAnswerCorrect { get; set; }

        public FillInTheBlankViewModel() {}

        public FillInTheBlankViewModel(int id, string question)
        {
            FillInTheBlankId = id;
            Question = question;
        }
    }
}