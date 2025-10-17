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
        public virtual List<MatchingAttempt> MatchingAttempts { get; set; } = new List<MatchingAttempt>();
        public virtual List<SequenceAttempt> SequenceAttempts { get; set; } = new List<SequenceAttempt>();
        public virtual List<RankingAttempt> RankingAttempts { get; set; } = new List<RankingAttempt>();


        [NotMapped]
        public IEnumerable<QuestionAttempt> AllQuestionAttempts =>
            FillInTheBlankAttempts.Cast<QuestionAttempt>()
                .Concat(MatchingAttempts)
                .Concat(SequenceAttempts)
                .Concat(RankingAttempts);
    }
}