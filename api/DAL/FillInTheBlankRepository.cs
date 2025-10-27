using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL;

public class FillInTheBlankRepository : IRepository<FillInTheBlank>
{
    private readonly QuizDbContext _db;
    private readonly ILogger<FillInTheBlankRepository> _logger;

    public FillInTheBlankRepository(QuizDbContext db, ILogger<FillInTheBlankRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<FillInTheBlank>?> GetAll()
    {
        try
        {
            return await _db.FillInTheBlankQuestions.ToListAsync();
        }
        catch (Exception e) {
            _logger.LogError("[FillInTheBlankRepository] ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<FillInTheBlank?> GetById(int id)
    {
        try
        {
            return await _db.FillInTheBlankQuestions.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[FillInTheBlankRepository] FillInTheBlank FindAsync(id) failed when GetQuestionById) for FillInTheBlankId {FillInTheBlankId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(FillInTheBlank fillQuestion)
    {
        try
        {
            _db.FillInTheBlankQuestions.Add(fillQuestion);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[FillInTheBlankRepository] FillInTheBlank creation failed for question {@fillInTheBlank}, error message: {e}", fillQuestion, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(FillInTheBlank fillQuestion)
    {
        try
        {
            _db.FillInTheBlankQuestions.Update(fillQuestion);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[FillInTheBlankRepository] FillInTheBlank update failed for question {@fillInTheBlank}, error message: {e}", fillQuestion, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id) {
        try
        {
            var fillQuestion = await _db.FillInTheBlankQuestions.FindAsync(id);
            if (fillQuestion == null)
            {
                return false;
            }

            _db.FillInTheBlankQuestions.Remove(fillQuestion);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[FillInTheBlankRepository] FillInTheBlank deletion failed for the FillInTheblankId {FillInTheBlankId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}