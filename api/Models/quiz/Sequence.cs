using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
 public class Sequence : Question
    {
        public int SequenceId { get; set; }

        [Required(ErrorMessage = "Must give a question")]
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        [Required(ErrorMessage = "Must give an answer")]
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
    }   
}