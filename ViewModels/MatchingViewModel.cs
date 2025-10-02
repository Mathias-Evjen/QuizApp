using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class MatchingViewModel
    {
        public Matching Question { get; set; }
        public List<string> Keys { get; set; } = new List<string>();
        public List<string> Values { get; set; } = new List<string>();

        public MatchingViewModel() { }

        public MatchingViewModel(Matching question)
        {
            Question = question;


            foreach (var pair in question.SplitQuestion())
            {
                Keys.Add(pair.Key);
            }

            for (int i = 0; i < Keys.Count; i++)
            {
                Values.Add(string.Empty);
            }
        }
    }

}