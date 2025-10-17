namespace QuizApp.Models
{
    public class TrueFalseQuestion : Question
    {
        public int TrueFalseQuestionId { get; set; }
        // public override int QuestionId => TrueFalseQuestionId;
        public string QuestionText { get; set; } = string.Empty;
        public bool CorrectAnswer { get; set; }
    }
}
