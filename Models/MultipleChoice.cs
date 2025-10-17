using System.Collections.Generic;

namespace QuizApp.Models
{
    public class MultipleChoice : Question
    {
        public int MultipleChoiceId { get; set; }
        // public override int QuestionId => MultipleChoiceId;
        public virtual List<Option> Options { get; set; } = new List<Option>();
        public string QuestionText { get; set; } = string.Empty;
        public int QuizId { get; set; } = default!;
        public virtual Quiz Quiz { get; set; } = default!;
    }
}
