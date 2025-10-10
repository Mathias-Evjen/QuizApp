using QuizApp.Models;

namespace QuizApp.DAL;

public interface IRankingRepository
{
    Task<IEnumerable<Ranking>?> GetAll();
    Task<Ranking?> GetRankingById(int id);
    Task<bool> CreateRanking(Ranking Ranking);
    Task<bool> UpdateRanking(Ranking Ranking);
    Task<bool> DeleteRanking(int id);
}