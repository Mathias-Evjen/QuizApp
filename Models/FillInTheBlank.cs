using System;

namespace QuizApp.Models
{
    public class FillInTheBlank : Question
    {
        public int FillInTheBlankId { get; set; }
        public override string QuestionType => "FillInTheBlank";
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        public int QuizId { get; set; } // Hvilken quiz spørsmålet tilhører
        public virtual Quiz? Quiz { get; set; } = default!;

        public bool AnsweredCorrectly { get; set; }
    }
}