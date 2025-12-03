using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class TrueFalse : Question
    {
        public int TrueFalseId { get; set; }

        [Required(ErrorMessage = "Must give a question")]
        public string Question { get; set; } = string.Empty;

        [Required(ErrorMessage = "Must give an answer")]
        public bool CorrectAnswer { get; set; }
        public int QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; } = default!;
    }
}
