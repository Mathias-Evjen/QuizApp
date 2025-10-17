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

        // Henter alle True/False-spørsmål fra databasen uten sporing
        public async Task<List<TrueFalse>> GetAllAsync()
        {
            try
            {
                return await _context.TrueFalseQuestions
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved henting av alle True/False-spørsmål.");
                return new List<TrueFalse>();
            }
        }
    }

        // Henter ett True/False-spørsmål basert på ID
        public async Task<TrueFalse?> GetByIdAsync(int id)
        {
            return await _db.TrueFalseQuestions.FindAsync(id);
        }

        // Henter detaljert informasjon om et True/False-spørsmål
        public async Task<TrueFalse?> GetDetailedAsync(int id)
        {
            try
            {
                return await _context.TrueFalseQuestions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(q => q.TrueFalseId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved henting av detaljert True/False-spørsmål med Id={Id}", id);
                return null;
            }
        }
    }

        // Legger til et nytt True/False-spørsmål i databasen
        public async Task AddAsync(TrueFalse question)
        {
            _db.TrueFalseQuestions.Add(question);
            await _db.SaveChangesAsync();

            _logger.LogInformation("[TrueFalseRepository] Created TrueFalseQuestion {@Question}", question);
            return true;
        }

        // Oppdaterer et eksisterende True/False-spørsmål
        public async Task UpdateAsync(TrueFalse question)
        {
            try
            {
                _context.TrueFalseQuestions.Update(question);
                _logger.LogInformation("Oppdatert True/False-spørsmål med Id={Id}", question.TrueFalseId);
                await Task.CompletedTask; // behold async-signaturen
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved oppdatering av True/False-spørsmål Id={Id}", question.TrueFalseId);
                throw;
            }
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
