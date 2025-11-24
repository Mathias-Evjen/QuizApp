using System.ComponentModel.DataAnnotations;

namespace QuizApp.DTOs
{
    public class FillInTheBlankDto
    {
        public int FillInTheBlankId { get; set; }

        [Required(ErrorMessage = "Must give a question")]
        public string Question { get; set; } = string.Empty;

        [Required(ErrorMessage = "Must give an answer")]
        public string? CorrectAnswer { get; set; }
        public int QuizId { get; set; }
        public int QuizQuestionNum { get; set; }
    }
}