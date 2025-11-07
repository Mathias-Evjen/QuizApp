using System.ComponentModel.DataAnnotations;

namespace QuizApp.DTOs
{
    public class MultipleChoiceDto
    {
        public int MultipleChoiceId { get; set; }

        [Required(ErrorMessage = "Must contain a question")]
        public string Question { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Must contain an answer")]
        public string? CorrectAnswer { get; set; }
        public int QuizId { get; set; }
        public int QuizQuestionNum { get; set; }

        [Required(ErrorMessage = "At least one option is required")]
        [MinLength(4, ErrorMessage = "There must be at least four options")]
        public List<OptionDto> Options { get; set; } = new();
    }
}
