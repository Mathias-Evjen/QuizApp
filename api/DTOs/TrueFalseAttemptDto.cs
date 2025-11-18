namespace QuizApp.DTOs
{
    public class TrueFalseAttemptDto
    {
        public int? TrueFalseAttemptId { get; set; }
        public int QuizQuestionNum { get; set; }
        public bool UserAnswer { get; set; }
        public int TrueFalseId { get; set; }
        public int QuizAttemptId { get; set; }
    }
}