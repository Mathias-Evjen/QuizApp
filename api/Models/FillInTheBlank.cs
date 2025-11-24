using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class FillInTheBlank : Question
    {
        public int FillInTheBlankId { get; set; }

        [Required(ErrorMessage = "Must give a question")]
        public string Question { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Must give an answer")]
        public string CorrectAnswer { get; set; } = string.Empty;
        public int QuizId { get; set; } // Hvilken quiz spørsmålet tilhører
        public virtual Quiz? Quiz { get; set; } = default!;
    }
}