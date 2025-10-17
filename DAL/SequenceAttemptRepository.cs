using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using Serilog;

namespace QuizApp.DAL;

public class SequenceAttemptRepository : ISequenceAttemptRepository
{
    private readonly QuizDbContext _db;
    private readonly ILogger<SequenceAttemptRepository> _logger;

    public SequenceAttemptRepository(QuizDbContext db, ILogger<SequenceAttemptRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<SequenceAttempt?> GetSequenceAttemptById(int id)
    {
        try
        {
            return await _db.SequenceAttempts.FindAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[SequenceAttemptRepository] SequenceAttempt FindAsync(id) failed when GetSequenceAttemptId {SequenceAttemptId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> CreateSequenceAttempt(SequenceAttempt sequenceAttempt)
    {
        try
        {
            _db.SequenceAttempts.Add(sequenceAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[SequenceAttemptRepository] SequenceAttempt creation failed for SequenceAttempt {@SequenceAttempt}, error message: {e}", sequenceAttempt, e.Message);
            return false;
        }
    }

    public async Task<bool> UpdateSequenceAttempt(SequenceAttempt sequenceAttempt)
    {
        try
        {
            _db.SequenceAttempts.Update(sequenceAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[SequenceAttemptRepository] SequenceAttempt update failed for SequenceAttempt {@SequenceAttempt}, error message: {e}", sequenceAttempt, e.Message);
            return false;
        }
    }

    public async Task<bool> DeleteSequenceAttempt(int id)
    {
        try
        {
            var SequenceAttempt = await _db.SequenceAttempts.FindAsync(id);
            if (SequenceAttempt == null)
            {
                return false;
            }

            _db.SequenceAttempts.Remove(SequenceAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[SequenceAttemptRepository] SequenceAttempt deletion failed for the SequenceAttemptId {SequenceAttemptId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}