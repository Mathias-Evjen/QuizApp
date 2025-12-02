using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuizApp.Models;

namespace QuizApp.DTOs
{
    public class QuizDto
    {
        public int QuizId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int NumOfQuestions { get; set; } = 0;
        public virtual List<FillInTheBlank> FillInTheBlankQuestions { get; set; } = [];
        public virtual List<Matching> MatchingQuestions { get; set; } = [];
        public virtual List<Sequence> SequenceQuestions { get; set; } = [];
        public virtual List<Ranking> RankingQuestions { get; set; } = [];
        public virtual List<TrueFalse> TrueFalseQuestions { get; set; } = [];
        public virtual List<MultipleChoice> MultipleChoiceQuestions { get; set; } = [];

        [NotMapped]
        public IEnumerable<Question> AllQuestions =>
            FillInTheBlankQuestions.Cast<Question>()
                .Concat(MatchingQuestions)
                .Concat(SequenceQuestions)
                .Concat(RankingQuestions)
                .Concat(TrueFalseQuestions)
                .Concat(MultipleChoiceQuestions)
                .OrderBy(q => q.QuizQuestionNum);
    
    }
}
    