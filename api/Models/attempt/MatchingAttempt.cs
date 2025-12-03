namespace QuizApp.Models
{
    public class MatchingAttempt : QuestionAttempt
    {
        public int MatchingAttemptId { get; set; }
        public int MatchingId { get; set; }
        public int QuizAttemptId { get; set; }
        public virtual QuizAttempt QuizAttempt { get; set; } = default!;
        public string UserAnswer { get; set; } = string.Empty;
        public int AmountCorrect { get; set; }
        public int TotalRows { get; set; }
        public bool? AnsweredCorrectly { get; set; }
    }
}