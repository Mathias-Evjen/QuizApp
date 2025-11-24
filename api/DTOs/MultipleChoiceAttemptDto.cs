namespace QuizApp.DTOs
{
    public class MultipleChoiceAttemptDto
    {
        public int? MultipleChoiceAttemptId { get; set; }
        public int QuizQuestionNum { get; set; }
        public string UserAnswer { get; set; } = string.Empty;
        public int MultipleChoiceId { get; set; }
        public int QuizAttemptId { get; set; }
        public bool AnsweredCorrectly { get; set; }
    }
}