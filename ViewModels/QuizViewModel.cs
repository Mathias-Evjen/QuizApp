using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class QuizViewModel
    {
        public int QuizId { get; set; }
        public int CurrentQuestionNum { get; set; } = 0;
        public List<QuestionViewModel> QuestionViewModels { get; set; } = new List<QuestionViewModel>();
        public string QuizName { get; set; } = string.Empty;
        public string QuizDescription { get; set; } = string.Empty;

        public QuizViewModel() { }
        public QuizViewModel(Quiz quiz)
        {
            QuizId = quiz.QuizId;
            QuizName = quiz.Name;
            QuizDescription = quiz.Description;
            
            foreach(var question in quiz.AllQuestions)
            {
                if (question is FillInTheBlank fibQuestion)
                    QuestionViewModels.Add(new FillInTheBlankViewModel(fibQuestion));
                if (question is Matching mQuestion)
                    QuestionViewModels.Add(new MatchingViewModel(mQuestion));
                if (question is Sequence sQuestion)
                    QuestionViewModels.Add(new SequenceViewModel(sQuestion));
                if (question is Ranking rQuestion)
                    QuestionViewModels.Add(new RankingViewModel(rQuestion));
            }
        }

    }
}