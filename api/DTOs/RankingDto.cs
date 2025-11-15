using System.ComponentModel.DataAnnotations;

namespace QuizApp.DTOs
{
    public class RankingDto
    {
        public int RankingId { get; set; }
        public string QuestionText { get; set; } = string.Empty;

        
        public string Question { get; set; } = string.Empty;

        
        public string CorrectAnswer { get; set; } = string.Empty;

        public int QuizId { get; set; }
        public int QuizQuestionNum { get; set; }
    }
}
