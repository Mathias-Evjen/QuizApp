namespace QuizApp.Models
{
    public class TrueFalse : Question
    {
        public int TrueFalseId { get; set; }
        // public override int QuestionId => TrueFalseQuestionId;
        public string QuestionText { get; set; } = string.Empty;
        public bool CorrectAnswer { get; set; }
        public int QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; } = default!;
    }
}
