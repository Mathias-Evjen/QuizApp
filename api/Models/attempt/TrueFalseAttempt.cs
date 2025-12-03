using System;

namespace QuizApp.Models
{
    public class TrueFalseAttempt : QuestionAttempt
    {
        public int TrueFalseAttemptId { get; set; }
        public int TrueFalseId { get; set; }
        public int QuizAttemptId { get; set; }
        public virtual QuizAttempt QuizAttempt { get; set; } = default!;
        public bool UserAnswer { get; set; }
        public bool? AnsweredCorrectly { get; set; }
    }
}