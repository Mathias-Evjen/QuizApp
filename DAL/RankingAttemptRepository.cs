using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using Serilog;

namespace QuizApp.DAL;

public class RankingAttemptRepository : IRankingAttemptRepository
{
    private readonly QuizDbContext _db;
    private readonly ILogger<RankingAttemptRepository> _logger;

    public RankingAttemptRepository(QuizDbContext db, ILogger<RankingAttemptRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<RankingAttempt?> GetRankingAttemptById(int id)
    {
        try
        {
            return await _db.RankingAttempts.FindAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[RankingAttemptRepository] RankingAttempt FindAsync(id) failed when GetRankingAttemptId {RankingAttemptId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> CreateRankingAttempt(RankingAttempt rankingAttempt)
    {
        try
        {
            _db.RankingAttempts.Add(rankingAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[RankingAttemptRepository] RankingAttempt creation failed for RankingAttempt {@RankingAttempt}, error message: {e}", rankingAttempt, e.Message);
            return false;
        }
    }

    public async Task<bool> UpdateRankingAttempt(RankingAttempt rankingAttempt)
    {
        try
        {
            _db.RankingAttempts.Update(rankingAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[RankingAttemptRepository] RankingAttempt update failed for RankingAttempt {@RankingAttempt}, error message: {e}", rankingAttempt, e.Message);
            return false;
        }
    }

    public async Task<bool> DeleteRankingAttempt(int id)
    {
        try
        {
            var RankingAttempt = await _db.RankingAttempts.FindAsync(id);
            if (RankingAttempt == null)
            {
                return false;
            }

            _db.RankingAttempts.Remove(RankingAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[RankingAttemptRepository] RankingAttempt deletion failed for the RankingAttemptId {RankingAttemptId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}