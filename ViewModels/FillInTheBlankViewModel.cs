using System.ComponentModel.DataAnnotations;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class FillInTheBlankViewModel : QuestionViewModel
    {
        public int FillInTheBlankId { get; set; }
        public string Question { get; set; } = string.Empty;
        public int QuizQuestionNum { get; set; }

        [Required(ErrorMessage = "Must answer")]
        public string UserAnswer { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public bool? IsAnswerCorrect { get; set; }

        public FillInTheBlankViewModel() {}

        public FillInTheBlankViewModel(FillInTheBlank question)
        {
            FillInTheBlankId = question.FillInTheBlankId;
            Question = question.Question;
            QuizQuestionNum = question.QuizQuestionNum;
        }
    }
}