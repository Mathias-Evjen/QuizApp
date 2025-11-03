
namespace QuizApp.DTOs
{
    public class FillInTheBlankAttemptdto
    {
        public int? FillInTheBlankAttemptId { get; set; }
        public int QuizQuestionNum { get; set; }
        public string UserAnswer { get; set; } = string.Empty;
        public int FillInTheBlankId { get; set; }
        public int QuizAttemptId { get; set; }
    }
}