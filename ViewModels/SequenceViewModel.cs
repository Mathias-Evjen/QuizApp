using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class SequenceViewModel
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public IEnumerable<Sequence> Sequences { get; set; } = new List<Sequence>();
        public List<string> Keys { get; set; } = new List<string>();
        public List<string> Values { get; set; } = new List<string>();

        public SequenceViewModel(IEnumerable<Sequence>? sequences)
        {
            Sequences = sequences!;
        }
        public SequenceViewModel(Sequence sequence)
        {
            Id = sequence.Id;
            Question = sequence.Question;
            QuestionText = sequence.QuestionText;
        }
    }
}
