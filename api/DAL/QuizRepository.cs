using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL;

public class QuizRepository : IRepository<Quiz>
{
    private readonly QuizDbContext _db;
    private readonly ILogger<QuizRepository> _logger;

    public QuizRepository(QuizDbContext db, ILogger<QuizRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Quiz>?> GetAll()
    {
        try
        {
            return await _db.Quizzes
                            .Include(q => q.FillInTheBlankQuestions)
                            .Include(q => q.MatchingQuestions)
                            .Include(q => q.SequenceQuestions)
                            .Include(q => q.RankingQuestions)
                            .Include(q => q.TrueFalseQuestions)
                            .Include(q => q.MultipleChoiceQuestions)
                            .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[QuizRepository] ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<Quiz?> GetById(int id)
    {
        try
        {
            return await _db.Quizzes
                            .Include(q => q.FillInTheBlankQuestions)
                            .Include(q => q.MatchingQuestions)
                            .Include(q => q.SequenceQuestions)
                            .Include(q => q.RankingQuestions)
                            .Include(q => q.TrueFalseQuestions)
                            .Include(q => q.MultipleChoiceQuestions)
                            .FirstOrDefaultAsync(q => q.QuizId == id);
        }
        catch (Exception e)
        {
            _logger.LogError("[QuizRepository] Quiz FindAsync(id) failed when GetQuizId {QuizId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(Quiz Quiz)
    {
        try
        {
            _db.Quizzes.Add(Quiz);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[QuizRepository] Quiz creation failed for Quiz {@Quiz}, error message: {e}", Quiz, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(Quiz Quiz)
    {
        try
        {
            _db.Quizzes.Update(Quiz);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[QuizRepository] Quiz update failed for Quiz {@Quiz}, error message: {e}", Quiz, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var Quiz = await _db.Quizzes.FindAsync(id);
            if (Quiz == null)
            {
                return false;
            }

            _db.Quizzes.Remove(Quiz);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[QuizRepository] Quiz deletion failed for the QuizId {QuizId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}