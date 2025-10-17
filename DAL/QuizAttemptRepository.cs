using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using Serilog;

namespace QuizApp.DAL;

public class QuizAttemptRepository : IQuizAttemptRepository
{
    private readonly QuizDbContext _db;
    private readonly ILogger<QuizAttemptRepository> _logger;

    public QuizAttemptRepository(QuizDbContext db, ILogger<QuizAttemptRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<QuizAttempt>?> GetAll()
    {
        try
        {
            return await _db.QuizAttempts.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[QuizAttemptRepository] ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<QuizAttempt?> GetQuizAttemptById(int id)
    {
        try
        {
            return await _db.QuizAttempts.Include(q => q.FillInTheBlankAttempts).FirstOrDefaultAsync(q => q.QuizAttemptId == id);
        }
        catch (Exception e)
        {
            _logger.LogError("[QuizAttemptRepository] QuizAttempt FindAsync(id) failed when GetQuizAttemptId {QuizAttemptId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> CreateQuizAttempt(QuizAttempt QuizAttempt)
    {
        try
        {
            _db.QuizAttempts.Add(QuizAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[QuizAttemptRepository] QuizAttempt creation failed for QuizAttempt {@QuizAttempt}, error message: {e}", QuizAttempt, e.Message);
            return false;
        }
    }

    public async Task<bool> UpdateQuizAttempt(QuizAttempt QuizAttempt)
    {
        try
        {
            _db.QuizAttempts.Update(QuizAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[QuizAttemptRepository] QuizAttempt update failed for QuizAttempt {@QuizAttempt}, error message: {e}", QuizAttempt, e.Message);
            return false;
        }
    }

    public async Task<bool> DeleteQuizAttempt(int id)
    {
        try
        {
            var QuizAttempt = await _db.QuizAttempts.FindAsync(id);
            if (QuizAttempt == null)
            {
                return false;
            }

            _db.QuizAttempts.Remove(QuizAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[QuizAttemptRepository] QuizAttempt deletion failed for the QuizAttemptId {QuizAttemptId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}