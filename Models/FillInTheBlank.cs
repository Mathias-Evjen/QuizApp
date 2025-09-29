using System;

namespace QuizApp.Models
{
    public class FillInTheBlank
    {
        public int id { get; set; }
        public string questionText { get; set; } = string.Empty;
        public string correctAnswer { get; set; } = string.Empty;
    }
}