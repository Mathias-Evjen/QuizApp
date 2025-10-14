using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class MatchingViewModel : QuestionViewModel
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public IEnumerable<Matching> Matchings { get; set; } = new List<Matching>();
        public List<string> Keys { get; set; } = new List<string>();
        public List<string> Values { get; set; } = new List<string>();

        public MatchingViewModel(IEnumerable<Matching>? matchings)
        {
            Matchings = matchings!;
        }

        public MatchingViewModel(Matching question)
        {
            Id = question.Id;
            Question = question.Question;


            foreach (var pair in question.SplitQuestion())
            {
                Keys.Add(pair.Key);
                Values.Add(pair.Value);
            }
        }
    }

}