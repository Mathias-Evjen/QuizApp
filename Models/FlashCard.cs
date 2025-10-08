using System;

namespace QuizApp.Models
{
    public class FlashCard
    {
        public int FlashCardId { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public bool ShowAnswer { get; set; } = false;
        public int QuizId { get; set; }
        public virtual FlashCardQuiz? Quiz { get; set; } = default!;
        public int QuizQuestionNum { get; set; }
        public bool AnsweredCorrectly { get; set; }
        public string BackgroundColor { get; set; } = string.Empty;
    }
}