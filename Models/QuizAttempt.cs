using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
    public class QuizAttempt
    {
        public int QuizAttemptId { get; set; }
        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; } = default!;
        public int NumOfCorrectAnswers { get; set; }
        public virtual List<FillInTheBlankAttempt> FillInTheBlankAttempts { get; set; } = new List<FillInTheBlankAttempt>();
        public virtual List<TrueFalseAttempt> TrueFalseQuestionAttempts { get; set; } = new List<TrueFalseAttempt>();
        public virtual List<MultipleChoiceAttempt> MultipleChoiceAttempts { get; set; } = new List<MultipleChoiceAttempt>();

        [NotMapped]
        public IEnumerable<QuestionAttempt> AllQuestionAttempts =>
            FillInTheBlankAttempts.Cast<QuestionAttempt>()
            .Concat(TrueFalseQuestionAttempts)
            .Concat(MultipleChoiceAttempts);
    }
}