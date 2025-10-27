using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using Serilog;

namespace QuizApp.DAL;

public class MatchingAttemptRepository : IAttemptRepository<MatchingAttempt>
{
    private readonly QuizDbContext _db;
    private readonly ILogger<MatchingAttemptRepository> _logger;

    public MatchingAttemptRepository(QuizDbContext db, ILogger<MatchingAttemptRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<MatchingAttempt?> GetById(int id)
    {
        try
        {
            return await _db.MatchingAttempts.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[MatchingAttemptRepository] MatchingAttempt FindAsync(id) failed when GetMatchingAttemptId {MatchingAttemptId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }
    public bool Exists(int id)
    {
        return _db.MatchingAttempts.Any(m => m.MatchingAttemptId == id);
    }

    public async Task<bool> Create(MatchingAttempt matchingAttempt)
    {
        try
        {
            _db.MatchingAttempts.Add(matchingAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[MatchingAttemptRepository] MatchingAttempt creation failed for MatchingAttempt {@MatchingAttempt}, error message: {e}", matchingAttempt, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(MatchingAttempt matchingAttempt)
    {
        try
        {
            _db.MatchingAttempts.Update(matchingAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[MatchingAttemptRepository] MatchingAttempt update failed for MatchingAttempt {@MatchingAttempt}, error message: {e}", matchingAttempt, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var MatchingAttempt = await _db.MatchingAttempts.FindAsync(id);
            if (MatchingAttempt == null)
            {
                return false;
            }

            _db.MatchingAttempts.Remove(MatchingAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[MatchingAttemptRepository] MatchingAttempt deletion failed for the MatchingAttemptId {MatchingAttemptId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}