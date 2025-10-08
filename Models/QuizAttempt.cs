namespace QuizApp.Models
{
    public class QuizAttempt
    {
        public int Total { get; set; }
        public int Score { get; set; }
        public List<AnsweredQuestion> Answers { get; set; } = new List<AnsweredQuestion>();
    }

    public class AnsweredQuestion
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public List<string> CorrectAnswers { get; set; } = new List<string>();
        public List<string> SelectedAnswers { get; set; } = new List<string>();
        public bool IsCorrect { get; set; }
    }
}