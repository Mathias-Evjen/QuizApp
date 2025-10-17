using QuizApp.Models;

namespace QuizApp.DAL;

public interface ISequenceAttemptRepository
{
    Task<SequenceAttempt?> GetSequenceAttemptById(int id);
    Task<bool> CreateSequenceAttempt(SequenceAttempt sequenceAttempt);
    Task<bool> UpdateSequenceAttempt(SequenceAttempt sequenceAttempt);
    Task<bool> DeleteSequenceAttempt(int id);
}