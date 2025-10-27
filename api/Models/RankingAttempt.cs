using System;

namespace QuizApp.Models
{
    public class RankingAttempt : QuestionAttempt
    {
        public int RankingAttemptId { get; set; }
        public int RankingId { get; set; }
        public int QuizAttemptId { get; set; }
        public virtual QuizAttempt QuizAttempt { get; set; } = default!;
        public string UserAnswer { get; set; } = string.Empty;
        public int AmountCorrect { get; set; }
        public bool? AnsweredCorrectly { get; set; }
    }
}