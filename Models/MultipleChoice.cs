using System.Collections.Generic;

namespace QuizApp.Models
{
    public class MultipleChoice : QuestionText
    {
        public List<Option> Options { get; set; } = new List<Option>();
    }
}
