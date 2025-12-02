using System.ComponentModel.DataAnnotations;

namespace QuizApp.DTOs
{
    public class SequenceDto
    {
        public int SequenceId { get; set; }

        [Required(ErrorMessage = "Must contain question text")]
        public string Question { get; set; } = string.Empty;

        [Required(ErrorMessage = "Must contain a correct answer")]
        public string CorrectAnswer { get; set; } = string.Empty;
        
        public int QuizId { get; set; }
        public int QuizQuestionNum { get; set; }
    }
}
