using System;

namespace QuizApp.Models
{
    public class FillInTheBlank
    {
        public int FillInTheBlankId { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }
}