using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL
{
    public class MultipleChoiceRepository : IMultipleChoiceRepository
    {
        private readonly QuizDbContext _db;
        private readonly ILogger<MultipleChoiceRepository> _logger;

        public MultipleChoiceRepository(QuizDbContext db, ILogger<MultipleChoiceRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        // Henter alle MultipleChoice-spørsmål
        public async Task<IEnumerable<MultipleChoice>?> GetAll()
        {
            try
            {
                return await _db.MultipleChoices
                    .Include(mc => mc.Options)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[MultipleChoiceRepository] GetAll() failed: {Message}", e.Message);
                return null;
            }
        }

        // Henter et spørsmål basert på ID
        public async Task<MultipleChoice?> GetById(int id)
        {
            try
            {
                return await _db.MultipleChoices
                    .Include(mc => mc.Options)
                    .FirstOrDefaultAsync(mc => mc.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError("[MultipleChoiceRepository] GetById({Id}) failed: {Message}", id, e.Message);
                return null;
            }
        }

        // Oppretter nytt spørsmål
        public async Task<bool> Create(MultipleChoice question)
        {
            try
            {
                _db.MultipleChoices.Add(question);
                await _db.SaveChangesAsync();
                _logger.LogInformation("[MultipleChoiceRepository] Created MultipleChoice: {Question}", question.QuestionText);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[MultipleChoiceRepository] Create() failed: {Message}", e.Message);
                return false;
            }
        }

        // Oppdaterer eksisterende spørsmål
        public async Task<bool> Update(MultipleChoice question)
        {
            try
            {
                _db.MultipleChoices.Update(question);
                await _db.SaveChangesAsync();
                _logger.LogInformation("[MultipleChoiceRepository] Updated MultipleChoice Id={Id}", question.Id);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[MultipleChoiceRepository] Update() failed for Id={Id}: {Message}", question.Id, e.Message);
                return false;
            }
        }

        // Sletter spørsmål
        public async Task<bool> Delete(int id)
        {
            try
            {
                var question = await _db.MultipleChoices.FindAsync(id);
                if (question == null)
                {
                    _logger.LogWarning("[MultipleChoiceRepository] Tried to delete non-existing Id={Id}", id);
                    return false;
                }

                _db.MultipleChoices.Remove(question);
                await _db.SaveChangesAsync();
                _logger.LogInformation("[MultipleChoiceRepository] Deleted MultipleChoice Id={Id}", id);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[MultipleChoiceRepository] Delete() failed for Id={Id}: {Message}", id, e.Message);
                return false;
            }
        }
    }
}
