using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL;

public class FlashCardRepository : IFlashCardRepository
{
    private readonly QuizDbContext _db;
    private readonly ILogger<FlashCardRepository> _logger;

    public FlashCardRepository(QuizDbContext db, ILogger<FlashCardRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<FlashCard>?> GetAll()
    {
        try
        {
            return await _db.FlashCards.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[FlashCardRepository] ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<IEnumerable<FlashCard>?> GetAll(int quizId)
    {
        try
        {
            return await _db.FlashCards
                                .Where(fc => fc.QuizId == quizId)
                                .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[FlashCardRepository] ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<FlashCard?> GetById(int id)
    {
        try
        {
            return await _db.FlashCards.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[FlashCardRepository] FlashCard FindAsync(id) failed when GetFlashCardId {FlashCardId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(FlashCard flashCard)
    {
        try
        {
            _db.FlashCards.Add(flashCard);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[FlashCardRepository] FlashCard creation failed for FlashCard {@flashCard}, error message: {e}", flashCard, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(FlashCard flashCard)
    {
        try
        {
            _db.FlashCards.Update(flashCard);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[FlashCardRepository] FlashCard update failed for FlashCard {@flashCard}, error message: {e}", flashCard, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var flashCard = await _db.FlashCards.FindAsync(id);
            if (flashCard == null)
            {
                return false;
            }

            _db.FlashCards.Remove(flashCard);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[FlashCardRepository] FlashCard deletion failed for the FlashCardId {FlashCardId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}

public interface IFlashCardRepository : IRepository<FlashCard>
{
    Task<IEnumerable<FlashCard>?> GetAll(int quizId);
}