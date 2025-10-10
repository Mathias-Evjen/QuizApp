using System.ComponentModel.DataAnnotations;

namespace QuizApp.ViewModels
{
    public class TrueFalseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        public string QuestionText { get; set; } = string.Empty;

        [Display(Name = "Correct Answer")]
        public bool CorrectAnswer { get; set; }
    }
}
