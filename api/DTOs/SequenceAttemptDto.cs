namespace QuizApp.DTOs
{
    public class SequenceAttemptDto
    {
        public int? SequenceAttemptId { get; set; }
        public int QuizQuestionNum { get; set; }
        public string UserAnswer { get; set; } = string.Empty;
        public int SequenceId { get; set; }
        public int QuizAttemptId { get; set; }
        public bool AnsweredCorrectly { get; set; }
    }
}