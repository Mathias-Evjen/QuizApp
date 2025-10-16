using System.ComponentModel.DataAnnotations;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class MatchingViewModel : QuestionViewModel
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public int QuizQuestionNum { get; set; }
        
        [Required(ErrorMessage = "Must answer")]
        public string UserAnswer { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public int AmountCorrect { get; set; }
        public List<string> Keys { get; set; } = new List<string>();
        public List<string> Values { get; set; } = new List<string>();


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