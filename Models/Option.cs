namespace QuizApp.Models
{
    public class Option
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        // Foreign key
        public int MultipleChoiceId { get; set; }
        public MultipleChoice? MultipleChoice { get; set; }
    }
}
