using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL;

public class MatchingRepository : IRepository<Matching>
{
    private readonly QuizDbContext _db;
    private readonly ILogger<MatchingRepository> _logger;

    public MatchingRepository(QuizDbContext db, ILogger<MatchingRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Matching>?> GetAll()
    {
        try
        {
            return await _db.MatchingQuestions.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[MatchingRepository] ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<Matching?> GetById(int id)
    {
        try
        {
            return await _db.MatchingQuestions.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[MatchingRepository] Matching FindAsync(id) failed when GetMatchingId {MatchingId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(Matching Matching)
    {
        try
        {
            _db.MatchingQuestions.Add(Matching);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[MatchingRepository] Matching creation failed for Matching {@Matching}, error message: {e}", Matching, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(Matching Matching)
    {
        try
        {
            _db.MatchingQuestions.Update(Matching);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[MatchingRepository] Matching update failed for Matching {@Matching}, error message: {e}", Matching, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var Matching = await _db.MatchingQuestions.FindAsync(id);
            if (Matching == null)
            {
                return false;
            }

            _db.MatchingQuestions.Remove(Matching);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[MatchingRepository] Matching deletion failed for the MatchingId {MatchingId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}