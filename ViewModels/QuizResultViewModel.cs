using System.Collections.Generic;

namespace QuizApp.ViewModels
{
    public class QuizResultViewModel
    {
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public double Percentage => TotalQuestions == 0 ? 0 : (double)CorrectAnswers / TotalQuestions * 100;

        public List<ResultQuestion> Results { get; set; } = new();
    }

    public class ResultQuestion
    {
        public string QuestionText { get; set; } = string.Empty;
        public List<string> CorrectAnswers { get; set; } = new();
        public List<string> UserAnswers { get; set; } = new();
        public bool IsCorrect { get; set; }
    }
}
