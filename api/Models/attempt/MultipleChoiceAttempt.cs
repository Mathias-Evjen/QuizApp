using System;

namespace QuizApp.Models
{
    public class MultipleChoiceAttempt : QuestionAttempt
    {
        public int MultiplechoiceAttemptId { get; set; }
        public int MultiplechoiceId { get; set; }
        public int QuizAttemptId { get; set; }
        public virtual QuizAttempt QuizAttempt { get; set; } = default!;
        public string UserAnswer { get; set; } = string.Empty;
        public bool? AnsweredCorrectly { get; set; }
    }
}