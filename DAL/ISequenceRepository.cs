using QuizApp.Models;

namespace QuizApp.DAL;

public interface ISequenceRepository
{
    Task<IEnumerable<Sequence>?> GetAll();
    Task<Sequence?> GetSequenceById(int id);
    Task<bool> CreateSequence(Sequence Sequence);
    Task<bool> UpdateSequence(Sequence Sequence);
    Task<bool> DeleteSequence(int id);
}