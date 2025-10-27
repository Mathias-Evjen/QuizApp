namespace QuizApp.Models
{
    public class Option
    {
        public int OptionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        public int? MultipleChoiceId { get; set; }
        public virtual MultipleChoice? MultipleChoice { get; set; } = default;
    }
}
