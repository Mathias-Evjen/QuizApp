using System.ComponentModel.DataAnnotations;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class RankingViewModel : QuestionViewModel
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public int QuizQuestionNum { get; set; }
        
        [Required(ErrorMessage = "Must answer")]
        public string UserAnswer { get; set; } = string.Empty;
        public List<string> Keys { get; set; } = [];
        public List<string> Values { get; set; } = [];

        public RankingViewModel(Ranking ranking)
        {
            Id = ranking.Id;
            Question = ranking.Question;
            QuestionText = ranking.QuestionText;
        }
    }
}
