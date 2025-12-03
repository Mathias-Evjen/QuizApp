using System;

namespace QuizApp.Models
{
    public class SequenceAttempt : QuestionAttempt
    {
        public int SequenceAttemptId { get; set; }
        public int SequenceId { get; set; }
        public int QuizAttemptId { get; set; }
        public virtual QuizAttempt QuizAttempt { get; set; } = default!;
        public string UserAnswer { get; set; } = string.Empty;
        public bool? AnsweredCorrectly { get; set; }
    }
}