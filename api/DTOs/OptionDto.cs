using System.ComponentModel.DataAnnotations;

namespace QuizApp.DTOs
{
    public class OptionDto
    {
        [Required(ErrorMessage = "Option text is required")]
        public string Text { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}
