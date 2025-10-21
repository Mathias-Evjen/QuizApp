using System.Collections.Generic;
using Humanizer;

namespace QuizApp.Models
{
    public class MultipleChoice : Question
    {
        public int MultipleChoiceId { get; set; }
        public virtual List<Option> Options { get; set; } = [];
        public string CorrectAnswer { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public int QuizId { get; set; } = default!;
        public virtual Quiz? Quiz { get; set; } = default!;
    }
}
