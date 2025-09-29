using System;

namespace QuizApp.Models
{
    public class FillInTheBlank
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public bool AnsweredCorrectly { get; set; }
    }
}