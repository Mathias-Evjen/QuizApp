using QuizApp.Models;

namespace QuizApp.DAL;

public interface IMatchingRepository
{
    Task<IEnumerable<Matching>?> GetAll();
    Task<Matching?> GetMatchingById(int id);
    Task<bool> CreateMatching(Matching Matching);
    Task<bool> UpdateMatching(Matching Matching);
    Task<bool> DeleteMatching(int id);
}