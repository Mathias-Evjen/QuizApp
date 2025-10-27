using System.ComponentModel.DataAnnotations;

namespace QuizApp.DTOs
{
    public class FlashCardQuizDto
    {
        public int FlashCardQuizId { get; set; }

        [Required(ErrorMessage = "Quiz must have a name")]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int NumOfQuestions { get; set; } = 0;
    }
}