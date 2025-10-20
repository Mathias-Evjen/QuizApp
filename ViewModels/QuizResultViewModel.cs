using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class QuizResultViewModel
    {
        public virtual Quiz Quiz { get; set; } = default!;
        public virtual QuizAttempt QuizAttempt { get; set; } = default!;

        public QuizResultViewModel(Quiz quiz, QuizAttempt quizAttempt)
        {
            Quiz = quiz;
            QuizAttempt = quizAttempt;
        }
    }
}