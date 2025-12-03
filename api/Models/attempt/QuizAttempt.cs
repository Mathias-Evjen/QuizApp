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
        public virtual List<FillInTheBlankAttempt> FillInTheBlankAttempts { get; set; } = [];
        public virtual List<TrueFalseAttempt> TrueFalseAttempts { get; set; } = [];
        public virtual List<MultipleChoiceAttempt> MultipleChoiceAttempts { get; set; } = [];
        public virtual List<MatchingAttempt> MatchingAttempts { get; set; } = [];
        public virtual List<SequenceAttempt> SequenceAttempts { get; set; } = [];
        public virtual List<RankingAttempt> RankingAttempts { get; set; } = [];


        [NotMapped]
        public IEnumerable<QuestionAttempt> AllQuestionAttempts =>
            FillInTheBlankAttempts.Cast<QuestionAttempt>()
                .Concat(TrueFalseAttempts)
                .Concat(MultipleChoiceAttempts)
                .Concat(MatchingAttempts)
                .Concat(SequenceAttempts)
                .Concat(RankingAttempts)
                .OrderBy(q => q.QuizQuestionNum);
    }
}