using System.Collections.Generic;
using Humanizer;

namespace QuizApp.Models
{
    public class MultipleChoice : Question
    {
        public override int QuestionId => Id;
        public int Id { get; set; }
        public string QuestionTexts { get; set; } = string.Empty;
        public virtual List<Option> Options { get; set; } = new();
        public int QuizId { get; set; } = default!;
        public virtual Quiz? Quiz { get; set; }
    }
}
