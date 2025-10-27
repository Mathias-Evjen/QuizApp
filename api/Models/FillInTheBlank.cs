using System;

namespace QuizApp.Models
{
    public class FillInTheBlank : Question
    {
        public int FillInTheBlankId { get; set; }
        public string Question { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public int QuizId { get; set; } // Hvilken quiz spørsmålet tilhører
        public virtual Quiz? Quiz { get; set; } = default!;
    }
}