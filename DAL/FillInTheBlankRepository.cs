using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL;

public class FillInTheBlankRepository : IFillInTheBlankRepository
{
    private readonly QuizDbContext _db;
    private readonly ILogger<FillInTheBlankRepository> _logger;

    public FillInTheBlankRepository(QuizDbContext db, ILogger<FillInTheBlankRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<FillInTheBlank?> GetQuestionById(int id)
    {
        try
        {
            return await _db.FillInTheBlankQuestions.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[FillInTheBlankRepository] FillInTheBlank FindAsync(id) failed whtn GetQuestionById) for FillInTheBlankId {FillInTheBlankId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(FillInTheBlank fillInTheBlank)
    {
        try
        {
            _db.FillInTheBlankQuestions.Add(fillInTheBlank);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[FillInTheBlankRepository] FillInTheBlank creation failed for item {@fillInTheBlank}, error message: {e}", fillInTheBlank, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(FillInTheBlank fillInTheBlank)
    {
        try
        {
            _db.FillInTheBlankQuestions.Update(fillInTheBlank);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[FillInTheBlankRepository] FillInTheBlank update failed for item {@fillInTheBlank}, error message: {e}", fillInTheBlank, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id) {
        try
        {
            var fillInTheBlank = await _db.FillInTheBlankQuestions.FindAsync(id);
            if (fillInTheBlank == null)
            {
                return false;
            }

            _db.FillInTheBlankQuestions.Remove(fillInTheBlank);
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