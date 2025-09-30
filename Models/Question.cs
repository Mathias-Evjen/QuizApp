using System;
using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; } = "";

        public List<Option> Options { get; set; } = new();

        public bool AllowMultiple { get; set; } = false;
    }

    public class Option
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; } = "";

        public bool IsCorrect { get; set; }
    }
}