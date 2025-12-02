namespace QuizApp.DTOs
{
    public class MatchingAttemptDto
    {
        public int? MatchingAttemptId { get; set; }
        public int QuizQuestionNum { get; set; }
        public string UserAnswer { get; set; } = string.Empty;
        public int MatchingId { get; set; }
        public int QuizAttemptId { get; set; }
        public bool AnsweredCorrectly { get; set; }
    }
}