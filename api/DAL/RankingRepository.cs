using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL;

public class RankingRepository : IRepository<Ranking>
{
    private readonly QuizDbContext _db;
    private readonly ILogger<RankingRepository> _logger;

    public RankingRepository(QuizDbContext db, ILogger<RankingRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Ranking>?> GetAll()
    {
        try
        {
            return await _db.RankingQuestions.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[RankingRepository] ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<Ranking?> GetById(int id)
    {
        try
        {
            return await _db.RankingQuestions.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[RankingRepository] Ranking FindAsync(id) failed when GetRankingId {RankingId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(Ranking Ranking)
    {
        try
        {
            _db.RankingQuestions.Add(Ranking);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[RankingRepository] Ranking creation failed for Ranking {@Ranking}, error message: {e}", Ranking, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(Ranking Ranking)
    {
        try
        {
            _db.RankingQuestions.Update(Ranking);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[RankingRepository] Ranking update failed for Ranking {@Ranking}, error message: {e}", Ranking, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var Ranking = await _db.RankingQuestions.FindAsync(id);
            if (Ranking == null)
            {
                return false;
            }

            _db.RankingQuestions.Remove(Ranking);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[RankingRepository] Ranking deletion failed for the RankingId {RankingId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}