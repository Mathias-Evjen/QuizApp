using System.ComponentModel.DataAnnotations;

namespace QuizApp.DTOs
{
    public class FlashCardDto
    {
        public int FlashCardId { get; set; }

        [Required(ErrorMessage = "Must contain a question")]
        public string Question { get; set; } = string.Empty;

        [Required(ErrorMessage = "Must contain an answer")]
        public string Answer { get; set; } = string.Empty;
        public bool ShowAnswer { get; set; } = false;
        public int QuizQuestionNum { get; set; }
        public int QuizId { get; set; }
        public string? Color { get; set; }
    }
}