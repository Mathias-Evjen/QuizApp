using QuizApp.Models;

namespace QuizApp.DAL;

public interface IMatchingAttemptRepository
{
    Task<MatchingAttempt?> GetMatchingAttemptById(int id);
    Task<bool> CreateMatchingAttempt(MatchingAttempt matchingAttempt);
    Task<bool> UpdateMatchingAttempt(MatchingAttempt matchingAttempt);
    Task<bool> DeleteMatchingAttempt(int id);
}