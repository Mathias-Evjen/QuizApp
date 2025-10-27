using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using NuGet.Protocol.Plugins;

namespace QuizApp.Models
{
    public class Matching : Question
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public int QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; } = default!;
        public int TotalRows { get; set; }


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
        public string ShuffleQuestion(List<string> keys, List<string> values)
        {
            if (values == null || values.Count == 0)
            {
                Console.WriteLine("Values er tom");
                throw new InvalidOperationException("Values list is empty or null");
            }

            Console.WriteLine("Shuffling");

            Random random = new Random(); // Opprett en ny Random-instans
            int n = values.Count;

            // Fisher-Yates-algoritmen for stokking
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1); // Velg en tilfeldig indeks mellom 0 og i
                // Bytt elementene på indeks i og j
                string temp = values[i];
                values[i] = values[j];
                values[j] = temp;
            }

            string shuffledQuestion = Assemble(keys, values, 3);

            return shuffledQuestion; // Returner den stokket listen
        }

    }
}