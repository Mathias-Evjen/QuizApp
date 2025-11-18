using System.ComponentModel.DataAnnotations;

namespace QuizApp.DTOs
{
    public class MatchingDto
    {
        public int MatchingId { get; set; }
        public string QuestionText { get; set; } = string.Empty;

        [Required(ErrorMessage = "Must contain a question")]
        public string Question { get; set; } = string.Empty;

        [Required(ErrorMessage = "Must contain an answer")]
        public string CorrectAnswer { get; set; } = string.Empty;

        [Required]
        public List<string> Keys { get; set; } = new();
    
        [Required]
        public List<string> Values { get; set; } = new();

        public int TotalRows { get; set; }
        public int QuizId { get; set; }
        public int QuizQuestionNum { get; set; }
    }
}
