using System;
using System.Security.Cryptography.X509Certificates;

namespace QuizApp.Models
{
    public class Matching
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public int AmountCorrect { get; set; }


        public KeyValuePair<string, string>[] SplitQuestion()
        {
            if (string.IsNullOrEmpty(Question))
            {
                return Array.Empty<KeyValuePair<string, string>>();
            }
            var partsQuestion = Question.Split(',');
            if (partsQuestion.Length % 2 != 0)
            {
                throw new InvalidOperationException("Amount of values in Question needs to be even");
            }
            var pairsQuestion = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < partsQuestion.Length; i += 2)
            {
                pairsQuestion.Add(new KeyValuePair<string, string>(partsQuestion[i].Trim(), partsQuestion[i + 1].Trim()));
            }
            return pairsQuestion.ToArray();
        }

        public string AssembleQuestion(string[] pairsQuestion)
        {
            if (pairsQuestion.Length == 0 || pairsQuestion == null)
            {
                return "";
            }
            string question = "";
            for (int i = 0; i < pairsQuestion.Length; i++)
            {
                if (i != 0) { question += ","; }
                question += pairsQuestion[i][0] + "," + pairsQuestion[i][1];
            }
            return question;
        }

    }
}