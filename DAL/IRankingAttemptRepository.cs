using QuizApp.Models;

namespace QuizApp.DAL;

public interface IRankingAttemptRepository
{
    Task<RankingAttempt?> GetRankingAttemptById(int id);
    Task<bool> CreateRankingAttempt(RankingAttempt rankingAttempt);
    Task<bool> UpdateRankingAttempt(RankingAttempt rankingAttempt);
    Task<bool> DeleteRankingAttempt(int id);
}