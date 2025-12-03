namespace QuizApp.Models
{
    public class FillInTheBlankAttempt : QuestionAttempt
    {
        public int FillInTheBlankAttemptId { get; set; }
        public int FillInTheBlankId { get; set; }
        public int QuizAttemptId { get; set; }
        public virtual QuizAttempt? QuizAttempt { get; set; } = default!;
        public string UserAnswer { get; set; } = string.Empty;
        public bool AnsweredCorrectly { get; set; }
    }
}