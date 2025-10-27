using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using Serilog;

namespace QuizApp.DAL;

public class MultipleChoiceAttemptRepository : IAttemptRepository<MultipleChoiceAttempt>
{
    private readonly QuizDbContext _db;
    private readonly ILogger<MultipleChoiceAttemptRepository> _logger;

    public MultipleChoiceAttemptRepository(QuizDbContext db, ILogger<MultipleChoiceAttemptRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<MultipleChoiceAttempt?> GetById(int id)
    {
        try
        {
            return await _db.MultipleChoiceAttempts.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[MultipleChoiceAttemptRepository] MultipleChoiceAttempt FindAsync(id) failed when GetMultipleChoiceAttemptId {MultipleChoiceAttemptId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }
    public bool Exists(int id)
    {
        return _db.MultipleChoiceAttempts.Any(mc => mc.MultiplechoiceAttemptId == id);
    }

    public async Task<bool> Create(MultipleChoiceAttempt MultipleChoiceAttempt)
    {
        try
        {
            _db.MultipleChoiceAttempts.Add(MultipleChoiceAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[MultipleChoiceAttemptRepository] MultipleChoiceAttempt creation failed for MultipleChoiceAttempt {@MultipleChoiceAttempt}, error message: {e}", MultipleChoiceAttempt, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(MultipleChoiceAttempt MultipleChoiceAttempt)
    {
        try
        {
            _db.MultipleChoiceAttempts.Update(MultipleChoiceAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[MultipleChoiceAttemptRepository] MultipleChoiceAttempt update failed for MultipleChoiceAttempt {@MultipleChoiceAttempt}, error message: {e}", MultipleChoiceAttempt, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var MultipleChoiceAttempt = await _db.MultipleChoiceAttempts.FindAsync(id);
            if (MultipleChoiceAttempt == null)
            {
                return false;
            }

            _db.MultipleChoiceAttempts.Remove(MultipleChoiceAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[MultipleChoiceAttemptRepository] MultipleChoiceAttempt deletion failed for the MultipleChoiceAttemptId {MultipleChoiceAttemptId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}