using System.ComponentModel.DataAnnotations;

namespace QuizApp.DTOs
{
    public class RankingDto
    {
        public int RankingId { get; set; }
        public string QuestionText { get; set; } = string.Empty;

        [Required(ErrorMessage = "Must contain a question")]
        public string Question { get; set; } = string.Empty;

        [Required(ErrorMessage = "Must contain an answer")]
        public string CorrectAnswer { get; set; } = string.Empty;

        public int QuizId { get; set; }
        public int QuizQuestionNum { get; set; }
    }
}
