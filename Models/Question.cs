using System;

namespace QuizApp.Models
{
    public abstract class Question()
    {
        public abstract int QuestionId { get; }
        public int QuizQuestionNum { get; set; } // Holder plasseringen i quizen
        // public abstract string QuestionType { get; }
    }
}