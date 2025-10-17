using QuizApp.Models;

namespace QuizApp.DAL;

public interface ITrueFalseAttemptRepository
{
    Task<TrueFalseAttempt?> GetTrueFalseAttemptById(int id);
    Task<bool> CreateTrueFalseAttempt(TrueFalseAttempt trueFalseAttempt);
    Task<bool> UpdateTrueFalseAttempt(TrueFalseAttempt trueFalseAttempt);
    Task<bool> DeleteTrueFalseAttempt(int id);
}