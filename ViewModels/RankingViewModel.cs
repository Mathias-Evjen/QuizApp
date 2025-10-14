using Microsoft.Net.Http.Headers;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class RankingViewModel : QuestionViewModel
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public IEnumerable<Ranking> Rankings { get; set; } = new List<Ranking>();
        public List<string> Keys { get; set; } = new List<string>();
        public List<string> Values { get; set; } = new List<string>();

        public RankingViewModel(IEnumerable<Ranking>? rankings)
        {
            Rankings = rankings!;
        }
        public RankingViewModel(Ranking ranking)
        {
            Id = ranking.Id;
            Question = ranking.Question;
            QuestionText = ranking.QuestionText;
        }
    }
}
