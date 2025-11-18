using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
 public class Sequence : Question
    {
        public int SequenceId { get; set; }
        // public override int QuestionId => Id;
        public string Question { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public int QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; } = default!;
        public string Assemble(List<string> values, int task)
        {
            string questionOrAnswer = "";
            for (int i = 0; i < values.Count; i++)
            {
                if (i != 0)
                {
                    questionOrAnswer += ",";
                }
                questionOrAnswer += values[i];
            }
            if (task == 1)
            {
                CorrectAnswer = questionOrAnswer;
            }
            else if (task == 2)
            {
                Answer = questionOrAnswer;
            }
            else if (task == 3)
            {
                Question = questionOrAnswer;
            }
            return questionOrAnswer;
        }
        public string ShuffleQuestion(List<string> values)
        {
            if (values == null || values.Count == 0)
            {
                Console.WriteLine("Values er tom");
                throw new InvalidOperationException("Values list is empty or null");
            }

            Random random = new Random();
            int n = values.Count;

            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                string temp = values[i];
                values[i] = values[j];
                values[j] = temp;
            }

            string shuffledQuestion = Assemble(values, 3);

            return shuffledQuestion; // Returner den stokket listen
        }
    }   
}