using System;

namespace QuizApp.Models
{
    public class FillInTheBlank : Question
    {
        public int FillInTheBlankId { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        public int QuizId { get; set; } // Hvilken quiz spørsmålet tilhører
        public int QuizQuestionNum { get; set; }    // Holder plasseringen i quizen

        public bool AnsweredCorrectly { get; set; }
    }
}