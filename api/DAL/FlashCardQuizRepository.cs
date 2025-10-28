using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL;

public class FlashCardQuizRepository : IQuizRepository<FlashCardQuiz> 
{
    private readonly QuizDbContext _db;
    private readonly ILogger<FlashCardQuizRepository> _logger;

    public FlashCardQuizRepository(QuizDbContext db, ILogger<FlashCardQuizRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<FlashCardQuiz>?> GetAll()
    {
        try 
        {
            return await _db.FlashCardQuizzes
                                .Include(fc => fc.FlashCards)
                                .ToListAsync();
        }
        catch (Exception e) 
        {
            _logger.LogError("[FlashCardQuizRepository] ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<FlashCardQuiz?> GetById(int id) {
        try
        {
            return await _db.FlashCardQuizzes
                                .Include(q => q.FlashCards)
                                .FirstOrDefaultAsync(q => q.FlashCardQuizId == id);
        }
        catch (Exception e)
        {
            _logger.LogError("[FlashCardQuizRepository] FlashCardQuiz FindAsync(id) failed when GetFlashCardQuizById {Id:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(FlashCardQuiz quiz)
    {
        try
        {
            _db.FlashCardQuizzes.Add(quiz);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[FlashCardQuizRepository] FlashCarQuiz creation failed for FlashCardQuiz {@quiz}, error message: {e}", quiz, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(FlashCardQuiz quiz)
    {
        try
        {
            _db.FlashCardQuizzes.Update(quiz);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[FlashCardQuizRepository] FlashCardQuiz update failed for FlashCardQuiz {@quiz}, error message: {e}", quiz, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id) 
    {
        try
        {
            var quiz = await _db.FlashCardQuizzes.FindAsync(id);
            if (quiz == null) return false;

            _db.FlashCardQuizzes.Remove(quiz);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[FlashCardQuizRepository] FlashCardQuiz deletion failed for FlashCardQuizId {id:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}