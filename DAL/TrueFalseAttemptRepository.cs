using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using Serilog;

namespace QuizApp.DAL;

public class TrueFalseAttemptRepository : ITrueFalseAttemptRepository
{
    private readonly QuizDbContext _db;
    private readonly ILogger<TrueFalseAttemptRepository> _logger;

    public TrueFalseAttemptRepository(QuizDbContext db, ILogger<TrueFalseAttemptRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<TrueFalseAttempt?> GetTrueFalseAttemptById(int id)
    {
        try
        {
            return await _db.TrueFalseQuestionAttempts.FindAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[TrueFalseAttemptRepository] TrueFalseAttempt FindAsync(id) failed when GetTrueFalseQuestionAttemptId {TrueFalseQuestionAttemptId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> CreateTrueFalseAttempt(TrueFalseAttempt trueFalseAttempt)
    {
        try
        {
            _db.TrueFalseQuestionAttempts.Add(trueFalseAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[TrueFalseAttemptRepository] TrueFalseAttempt creation failed for TrueFalseQuestionAttempt {@TrueFalseAttempt}, error message: {e}", trueFalseAttempt, e.Message);
            return false;
        }
    }

    public async Task<bool> UpdateTrueFalseAttempt(TrueFalseAttempt trueFalseAttempt)
    {
        try
        {
            _db.TrueFalseQuestionAttempts.Update(trueFalseAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[TrueFalseAttemptRepository] TrueFalseAttempt update failed for TrueFalseQuestionAttempt {@TrueFalseAttempt}, error message: {e}", trueFalseAttempt, e.Message);
            return false;
        }
    }

    public async Task<bool> DeleteTrueFalseAttempt(int id)
    {
        try
        {
            var trueFalseAttempt = await _db.TrueFalseQuestionAttempts.FindAsync(id);
            if (trueFalseAttempt == null)
            {
                return false;
            }

            _db.TrueFalseQuestionAttempts.Remove(trueFalseAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[TrueFalseAttemptRepository] TrueFalseAttempt deletion failed for the TrueFalseQuestionAttemptId {TrueFalseQuestionAttemptId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}