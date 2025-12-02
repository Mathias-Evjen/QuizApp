using System.ComponentModel.DataAnnotations;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class RankingViewModel : QuestionViewModel
    {
        public int RankingId { get; set; }
        public string Question { get; set; } = string.Empty;
        public int QuizQuestionNum { get; set; }
        
        [Required(ErrorMessage = "Must answer")]
        public string UserAnswer { get; set; } = string.Empty;
        public List<string> Keys { get; set; } = [];
        public List<string> Values { get; set; } = [];

        public RankingViewModel(Ranking ranking)
        {
            RankingId = ranking.RankingId;
            Question = ranking.Question;
        }
    }
}
