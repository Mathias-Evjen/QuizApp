namespace QuizApp.DTOs
{
    public class RankingAttemptDto
    {
        public int? RankingAttemptId { get; set; }
        public int QuizQuestionNum { get; set; }
        public string UserAnswer { get; set; } = string.Empty;
        public int RankingId { get; set; }
        public int QuizAttemptId { get; set; }
    }
}