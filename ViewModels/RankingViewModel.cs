using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class RankingViewModel
    {
        public Ranking Ranking { get; set; }
        public IEnumerable<Ranking> Rankings { get; set; } = new List<Ranking>();
        public List<string> Keys { get; set; } = new List<string>();
        public List<string> Values { get; set; } = new List<string>();

        public RankingViewModel(IEnumerable<Ranking>? rankings)
        {
            Rankings = rankings;
        }
        public RankingViewModel(Ranking ranking)
        {
            Ranking = ranking;
        }
    }
}
