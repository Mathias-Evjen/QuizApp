using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class QuizzesViewModel
    {
        public int CurrentQuizNum { get; set; } = 0;
        public IEnumerable<Question> Questions { get; set; } = new List<Question>();

        public string QuizName { get; set; } = string.Empty;
        public string QuizDescription { get; set; } = string.Empty;

        public QuizzesViewModel() { }
        public QuizzesViewModel(Quiz quiz)
        {
            QuizName = quiz.Name;
            QuizDescription = quiz.Description;
            Questions = quiz.AllQuestions;
        }

    }
}