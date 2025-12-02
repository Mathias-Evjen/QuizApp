using System.ComponentModel.DataAnnotations.Schema;
using QuizApp.Models;

namespace QuizApp.DTOs
{
    public class QuizAttemptDto
    {
        public int QuizAttemptId { get; set; }
        public int QuizId { get; set; }

        public int NumOfCorrectAnswers { get; set; }

        public virtual List<FillInTheBlankAttempt> FillInTheBlankAttempts { get; set; } = [];
        public virtual List<MatchingAttempt> MatchingAttempts { get; set; } = [];
        public virtual List<SequenceAttempt> SequenceAttempts { get; set; } = [];
        public virtual List<RankingAttempt> RankingAttempts { get; set; } = [];
        public virtual List<TrueFalseAttempt> TrueFalseAttempts { get; set; } = [];
        public virtual List<MultipleChoiceAttempt> MultipleChoiceAttempts { get; set; } = [];

        [NotMapped]
        public IEnumerable<QuestionAttempt> AllQuestionAttempts => 
            FillInTheBlankAttempts.Cast<QuestionAttempt>()
                .Concat(MatchingAttempts)
                .Concat(SequenceAttempts)
                .Concat(RankingAttempts)
                .Concat(TrueFalseAttempts)
                .Concat(MultipleChoiceAttempts)
                .OrderBy(q => q.QuizQuestionNum);
    }
}