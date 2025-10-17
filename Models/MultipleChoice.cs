using System.Collections.Generic;
using Humanizer;

namespace QuizApp.Models
{
    public class MultipleChoice : Question
    {
        public int MultipleChoiceId { get; set; }
        // public override int QuestionId => MultipleChoiceId;
        public virtual List<Option> Options { get; set; } = new();
        public string QuestionText { get; set; } = string.Empty;
        public int QuizId { get; set; } = default!;
        public virtual Quiz Quiz { get; set; } = default!;
    }
}
