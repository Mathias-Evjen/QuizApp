using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class QuizViewModel
    {
        public int QuizId { get; set; }
        public int QuizAttemptId { get; set; }
        public int CurrentQuestionNum { get; set; } = 0;
        public int NumOfQuestions { get; set; }
        public List<QuestionViewModel> QuestionViewModels { get; set; } = [];
        public string QuizName { get; set; } = string.Empty;
        public string QuizDescription { get; set; } = string.Empty;

        public QuizViewModel() { }
        public QuizViewModel(Quiz quiz, int quizAttemptId)
        {
            QuizId = quiz.QuizId;
            QuizAttemptId = quizAttemptId;
            QuizName = quiz.Name;
            QuizDescription = quiz.Description;
            NumOfQuestions = quiz.NumOfQuestions;
            
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
                if (question is TrueFalse tfQuestion)
                    QuestionViewModels.Add(new TrueFalseViewModel(tfQuestion));
                if (question is MultipleChoice mcQuestion)
                    QuestionViewModels.Add(new MultipleChoiceViewModel(mcQuestion));
            }
        }

    }
}