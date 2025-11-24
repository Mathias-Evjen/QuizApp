using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class Matching : Question
    {
        public int MatchingId { get; set; }

        [Required(ErrorMessage = "Must give a question")]
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        [Required(ErrorMessage = "Must give an answer")]
        public string CorrectAnswer { get; set; } = string.Empty;
        public int QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; } = default!;
        public int TotalRows { get; set; }

        public KeyValuePair<string,string>[] SplitCorrectAnswer()
        {
            if (string.IsNullOrEmpty(CorrectAnswer))
            {
                return Array.Empty<KeyValuePair<string, string>>();
            }
            var partsCorrectAnswer = CorrectAnswer.Split(',');
            if (partsCorrectAnswer.Length % 2 != 0)
            {
                throw new InvalidOperationException("Amount of values in Question needs to be even");
            }
            var pairsCorrectAnswer = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < partsCorrectAnswer.Length; i += 2)
            {
                pairsCorrectAnswer.Add(new KeyValuePair<string, string>(partsCorrectAnswer[i].Trim(), partsCorrectAnswer[i + 1].Trim()));
            }
            return pairsCorrectAnswer.ToArray();
        }

        public string Assemble(List<string> keys, List<string> value, int task)
        {
            if (keys.Count == 0 || value.Count == 0)
            {
                return "Empty lists!";
            }
            if (keys.Count != value.Count)
            {
                return "Lists is not the same length!";
            }
            string questionOrAnswer = "";
            for (int i = 0; i < keys.Count; i++)
            {
                if (i != 0) { questionOrAnswer += ","; }
                questionOrAnswer += keys[i] + "," + value[i];
            }
            if (task == 1)
            {
                CorrectAnswer = questionOrAnswer;
            }
            else if (task == 2)
            {
            }
            else if (task == 3)
            {
                Question = questionOrAnswer;
            }
            return questionOrAnswer;
        }
    }
}