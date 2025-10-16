using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
    public class Quiz 
    {
        public int QuizId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int NumOfQuestions { get; set; } = 0;
        public string Description { get; set; } = string.Empty;

        public virtual List<FillInTheBlank> FillInTheBlankQuestions { get; set; } = new();
        public virtual List<Matching> MatchingQuestions { get; set; } = new();
        public virtual List<Sequence> SequenceQuestions { get; set; } = new();
        public virtual List<Ranking> RankingQuestions { get; set; } = new();
        public virtual List<MultipleChoice> MultipleChoiceQuestions { get; set; } = new();
        public virtual List<TrueFalseQuestion> TrueFalseQuestions { get; set; } = new();

        [NotMapped]
        public IEnumerable<Question> AllQuestions =>
            FillInTheBlankQuestions.Cast<Question>()
            .Concat(MatchingQuestions)
            .Concat(SequenceQuestions)
            .Concat(RankingQuestions)
            .Concat(MultipleChoiceQuestions)
            .Concat(TrueFalseQuestions);

        }
}