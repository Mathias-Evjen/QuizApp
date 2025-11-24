using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class MultipleChoice : Question
    {
        public int MultipleChoiceId { get; set; }
        public virtual List<Option> Options { get; set; } = new List<Option>();

        [Required(ErrorMessage = "Must give an answer")]
        public string? CorrectAnswer { get; set; } = string.Empty;

        [Required(ErrorMessage = "Must give a question")]
        public string Question { get; set; } = string.Empty;
        public int QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; } = default!;
    }
}
