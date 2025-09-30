using System;

namespace QuizApp.Models
{
    public class FlashCard
    {
        public int FlashCardId { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public int QuizId { get; set; }
        public int QuizQuestionNum { get; set; }
        public bool AnsweredCorrectly { get; set; }
    }
}