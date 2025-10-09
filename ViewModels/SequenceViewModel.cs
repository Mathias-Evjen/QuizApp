using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class SequenceViewModel
    {
        public Sequence Sequence { get; set; }
        public IEnumerable<Sequence> Sequences { get; set; } = new List<Sequence>();
        public List<string> Keys { get; set; } = new List<string>();
        public List<string> Values { get; set; } = new List<string>();

        public SequenceViewModel(IEnumerable<Sequence>? sequences)
        {
            Sequences = sequences;
        }
        public SequenceViewModel(Sequence sequence)
        {
            Sequence = sequence;
        }
    }
}
