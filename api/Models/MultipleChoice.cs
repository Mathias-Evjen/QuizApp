namespace QuizApp.Models
{
    public class MultipleChoice : Question
    {
        public int MultipleChoiceId { get; set; }
        public virtual List<Option> Options { get; set; } = new List<Option>();
        public string? CorrectAnswer { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public int QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; } = default!;
    }
}
