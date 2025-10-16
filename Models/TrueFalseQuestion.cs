namespace QuizApp.Models
{
    public class TrueFalseQuestion : Question
    {
        public override int QuestionId => Id;
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public bool CorrectAnswer { get; set; }
        public int QuizId { get; set; } = default!;
        public virtual Quiz? Quiz { get; set; }
    }
}
