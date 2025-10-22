using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using Serilog;

namespace QuizApp.DAL;

public class TrueFalseAttemptRepository : IAttemptRepository<TrueFalseAttempt>
{
    private readonly QuizDbContext _db;
    private readonly ILogger<TrueFalseAttemptRepository> _logger;

    public TrueFalseAttemptRepository(QuizDbContext db, ILogger<TrueFalseAttemptRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<TrueFalseAttempt?> GetById(int id)
    {
        try
        {
            return await _db.TrueFalseAttempts.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[TrueFalseAttemptRepository] TrueFalseAttempt FindAsync(id) failed when GetTrueFalseQuestionAttemptId {TrueFalseQuestionAttemptId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }
    public bool Exists(int id)
    {
        return _db.TrueFalseAttempts.Any(tf => tf.TrueFalseAttemptId == id);
    }

    public async Task<bool> Create(TrueFalseAttempt trueFalseAttempt)
    {
        try
        {
            _db.TrueFalseAttempts.Add(trueFalseAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[TrueFalseAttemptRepository] TrueFalseAttempt creation failed for TrueFalseQuestionAttempt {@TrueFalseAttempt}, error message: {e}", trueFalseAttempt, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(TrueFalseAttempt trueFalseAttempt)
    {
        try
        {
            _db.TrueFalseAttempts.Update(trueFalseAttempt);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[TrueFalseAttemptRepository] TrueFalseAttempt update failed for TrueFalseQuestionAttempt {@TrueFalseAttempt}, error message: {e}", trueFalseAttempt, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var trueFalseAttempt = await _db.TrueFalseAttempts.FindAsync(id);
            if (trueFalseAttempt == null)
            {
                return false;
            }

            _db.TrueFalseAttempts.Remove(trueFalseAttempt);
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