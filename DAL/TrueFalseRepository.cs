using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL;

public class TrueFalseRepository : ITrueFalseRepository
{
    private readonly QuizDbContext _db;
    private readonly ILogger<TrueFalseRepository> _logger;

    public TrueFalseRepository(QuizDbContext db, ILogger<TrueFalseRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    // Henter alle True/False-spørsmål
    public async Task<IEnumerable<TrueFalseQuestion>?> GetAll()
    {
        try
        {
            return await _db.TrueFalseQuestions.AsNoTracking().ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[TrueFalseRepository] ToListAsync() failed in GetAll(), error: {e}", e.Message);
            return null;
        }
    }

    // Henter ett spørsmål basert på ID
    public async Task<TrueFalseQuestion?> GetById(int id)
    {
        try
        {
            return await _db.TrueFalseQuestions.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[TrueFalseRepository] FindAsync(id) failed for Id={Id}, error: {e}", id, e.Message);
            return null;
        }
    }

    // Opprett nytt spørsmål
    public async Task<bool> Create(TrueFalseQuestion question)
    {
        try
        {
            _db.TrueFalseQuestions.Add(question);
            await _db.SaveChangesAsync();

            _logger.LogInformation("[TrueFalseRepository] Created TrueFalseQuestion {@Question}", question);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[TrueFalseRepository] Creation failed for {@Question}, error: {e}", question, e.Message);
            return false;
        }
    }

    // Oppdater eksisterende spørsmål
    public async Task<bool> Update(TrueFalseQuestion question)
    {
        try
        {
            _db.TrueFalseQuestions.Update(question);
            await _db.SaveChangesAsync();

            _logger.LogInformation("[TrueFalseRepository] Updated TrueFalseQuestion Id={Id}", question.Id);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[TrueFalseRepository] Update failed for {@Question}, error: {e}", question, e.Message);
            return false;
        }
    }

    // Slett spørsmål basert på ID
    public async Task<bool> Delete(int id)
    {
        try
        {
            var question = await _db.TrueFalseQuestions.FindAsync(id);
            if (question == null)
            {
                _logger.LogWarning("[TrueFalseRepository] Tried to delete non-existing question Id={Id}", id);
                return false;
            }

            _db.TrueFalseQuestions.Remove(question);
            await _db.SaveChangesAsync();

            _logger.LogInformation("[TrueFalseRepository] Deleted TrueFalseQuestion Id={Id}", id);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[TrueFalseRepository] Deletion failed for Id={Id}, error: {e}", id, e.Message);
            return false;
        }
    }
}
