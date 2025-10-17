using QuizApp.Models;

namespace QuizApp.DAL;

public interface IMultipleChoiceAttemptRepository
{
    Task<MultipleChoiceAttempt?> GetMultipleChoiceAttemptById(int id);
    Task<bool> CreateMultipleChoiceAttempt(MultipleChoiceAttempt multipleChoiceAttempt);
    Task<bool> UpdateMultipleChoiceAttempt(MultipleChoiceAttempt multipleChoiceAttempt);
    Task<bool> DeleteMultipleChoiceAttempt(int id);
}