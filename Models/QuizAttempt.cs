using System;
using System.Collections.Generic;

namespace QuizApp.Models
{
    public class QuizAttempt
    {
        public int Score { get; set; }
        public int Total { get; set; }
        public List<AnsweredQuestion> Answers { get; set; } = new();
    }

    public class AnsweredQuestion
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public List<string> SelectedAnswers { get; set; } = new();
        public List<string> CorrectAnswers { get; set; } = new();
        public bool IsCorrect { get; set; }
    }
}