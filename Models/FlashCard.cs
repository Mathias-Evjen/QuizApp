using System;

namespace QuizApp.Models
{
    public class FlashCard
    {
        public int id { get; set; }
        public string qestion { get; set; } = string.Empty;
        public string answer { get; set; } = string.Empty;
    }
}