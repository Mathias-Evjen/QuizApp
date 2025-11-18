using System.ComponentModel.DataAnnotations;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class TrueFalseViewModel : QuestionViewModel
    {
        public int TrueFalseId { get; set; }
        public int QuizQuestionNum { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        public string QuestionText { get; set; } = string.Empty;

        [Display(Name = "Correct Answer")]
        public bool UserAnswer { get; set; }
        public bool? IsAnswerCorrect { get; set; }

        public TrueFalseViewModel() { }

        public TrueFalseViewModel(TrueFalse question)
        {
            TrueFalseId = question.TrueFalseId;
            QuestionText = question.Question;
            QuizQuestionNum = question.QuizQuestionNum;
        }
    }
}