using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class FlashCard
    {
        public int FlashCardId { get; set; }

        [Required(ErrorMessage = "Must write a question")]
        public string Question { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Must write an answer")]
        public string Answer { get; set; } = string.Empty;
        public int QuizId { get; set; }
        public virtual FlashCardQuiz? Quiz { get; set; } = default!;
        public int QuizQuestionNum { get; set; }
        public string BackgroundColor { get; set; } = string.Empty;
    }
}