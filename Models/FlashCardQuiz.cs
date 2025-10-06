using System;

namespace QuizApp.Models
{
    public class FlashCardQuiz 
    {
        public int FlashCardQuizId { get; set; }
        //public User creator { get; set; }
        public List<FlashCard>? FlashCards { get; set; }
        public int NumOfQuestions = 0;
        public string Description { get; set; } = string.Empty;

        // Might add tags later for easy search for quizzes with the same theme
    }
}