using System.ComponentModel.DataAnnotations;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class SequenceViewModel : QuestionViewModel
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public int QuizQuestionNum { get; set; }
        
        [Required(ErrorMessage = "Must answer")]
        public string UserAnswer { get; set; } = string.Empty;
        public List<string> Keys { get; set; } = new List<string>();
        public List<string> Values { get; set; } = new List<string>();

        public SequenceViewModel(Sequence sequence)
        {
            Id = sequence.Id;
            Question = sequence.Question;
            QuestionText = sequence.QuestionText;
        }
    }
}
