using System;

namespace QuizApp.Models
{
    public class FlashCardQuiz 
    {
        public int FlashCardQuizId { get; set; }
        public string Name { get; set; } = string.Empty;
        //public User creator { get; set; }
        public virtual List<FlashCard>? FlashCards { get; set; }
        public int NumOfQuestions { get; set; } = 0;
        public string? Description { get; set; }
    }
}