using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class FlashCardQuiz 
    {
        public int FlashCardQuizId { get; set; }

        [Required(ErrorMessage = "Must give a name")]
        [StringLength(60)]
        public string Name { get; set; } = string.Empty;
        //public User creator { get; set; }
        public virtual List<FlashCard>? FlashCards { get; set; }
        public int NumOfQuestions { get; set; } = 0;

        [StringLength(400)]
        public string? Description { get; set; }
    }
}