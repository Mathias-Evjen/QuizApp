using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class QuestionsViewModel
    {
        public IEnumerable<FillInTheBlankViewModel> Questions;

        public QuestionsViewModel(IEnumerable<FillInTheBlank> questions)
        {
            Questions = questions.Select(q => new FillInTheBlankViewModel(q.Id, q.Question)).ToList();
        }
    }
}