using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using Serilog;

namespace QuizApp.DAL;

public class FillInTheBlankAttemptRepository : IAttemptRepository<FillInTheBlankAttempt>
{
    private readonly QuizDbContext _db;
    private readonly ILogger<FillInTheBlankAttemptRepository> _logger;

    public FillInTheBlankAttemptRepository(QuizDbContext db, ILogger<FillInTheBlankAttemptRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<FillInTheBlankAttempt?> GetById(int id)
    {
        try
        {
            return await _db.FillInTheBlankAttempts.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[FillInTheBlankAttemptRepository] FillInTheBlankAttempt FindAsync(id) failed when GetFillInTheBlankAttemptId {FillInTheBlankAttemptId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }
    public bool Exists(int id)
    {
        return _db.FillInTheBlankAttempts.Any(fib => fib.FillInTheBlankAttemptId == id);
    }

    public async Task<bool> Create(FillInTheBlankAttempt fillInTheBlankAttempt)
    {
        try
        {
            _db.FillInTheBlankAttempts.Add(fillInTheBlankAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[FillInTheBlankAttemptRepository] FillInTheBlankAttempt creation failed for FillInTheBlankAttempt {@FillInTheBlankAttempt}, error message: {e}", fillInTheBlankAttempt, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(FillInTheBlankAttempt fillInTheBlankAttempt)
    {
        try
        {
            _db.FillInTheBlankAttempts.Update(fillInTheBlankAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[FillInTheBlankAttemptRepository] FillInTheBlankAttempt update failed for FillInTheBlankAttempt {@FillInTheBlankAttempt}, error message: {e}", fillInTheBlankAttempt, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var FillInTheBlankAttempt = await _db.FillInTheBlankAttempts.FindAsync(id);
            if (FillInTheBlankAttempt == null)
            {
                return false;
            }

            _db.FillInTheBlankAttempts.Remove(FillInTheBlankAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[FillInTheBlankAttemptRepository] FillInTheBlankAttempt deletion failed for the FillInTheBlankAttemptId {FillInTheBlankAttemptId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}