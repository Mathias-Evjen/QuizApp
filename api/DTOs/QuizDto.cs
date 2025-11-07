using System.ComponentModel.DataAnnotations;

namespace QuizApp.DTOs
{
    public class QuizDto
    {
        public int QuizId { get; set; }

        [Required(ErrorMessage = "Quiz must have a name")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int NumOfQuestions { get; set; } = 0;
    }
}