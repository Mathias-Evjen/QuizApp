using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
 public class Sequence : Question
    {
        public int SequenceId { get; set; }

        [Required(ErrorMessage = "Must give a question")]
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        [Required(ErrorMessage = "Must give an answer")]
        public string CorrectAnswer { get; set; } = string.Empty;
        public int QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; } = default!;
    }   
}