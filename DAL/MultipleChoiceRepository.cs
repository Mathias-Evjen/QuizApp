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

        // Hent alle MultipleChoice-spørsmål
        public async Task<IEnumerable<MultipleChoice>?> GetAll()
        {
            try
            {
                return await _db.MultipleChoices
                    .Include(q => q.Options)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[MultipleChoiceRepository] ToListAsync() failed in GetAll(), error: {e}", e.Message);
                return null;
            }
        }

        // Hent ett MultipleChoice-spørsmål basert på ID
        public async Task<MultipleChoice?> GetById(int id)
        {
            try
            {
                return await _db.MultipleChoices
                    .Include(q => q.Options)
                    .FirstOrDefaultAsync(q => q.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError("[MultipleChoiceRepository] FindAsync(id) failed for Id={Id}, error: {e}", id, e.Message);
                return null;
            }
        }

        // Opprett nytt spørsmål
        public async Task<bool> Create(MultipleChoice question)
        {
            try
            {
                // Fjern tomme alternativer
                question.Options = (question.Options ?? new List<Option>())
                    .Where(o => !string.IsNullOrWhiteSpace(o.Text))
                    .ToList();

                // Knytt alternativer til spørsmålet
                foreach (var opt in question.Options)
                {
                    opt.MultipleChoice = question;
                }

                _db.MultipleChoices.Add(question);
                await _db.SaveChangesAsync();

                _logger.LogInformation("[MultipleChoiceRepository] Created new MultipleChoice: {@Question}", question);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[MultipleChoiceRepository] Creation failed for MultipleChoice {@Question}, error: {e}", question, e.Message);
                return false;
            }
        }

        // Oppdater eksisterende spørsmål
        public async Task<bool> Update(MultipleChoice question)
        {
            try
            {
                var existing = await _db.MultipleChoices
                    .Include(q => q.Options)
                    .FirstOrDefaultAsync(q => q.Id == question.Id);

                if (existing == null)
                {
                    _logger.LogWarning("[MultipleChoiceRepository] Tried to update non-existing question Id={Id}", question.Id);
                    return false;
                }

                existing.QuestionTexts = question.QuestionTexts;

                // Fjern gamle alternativer
                _db.Options.RemoveRange(existing.Options);

                // Legg til nye alternativer
                existing.Options = question.Options
                    .Where(o => !string.IsNullOrWhiteSpace(o.Text))
                    .ToList();

                foreach (var opt in existing.Options)
                {
                    opt.MultipleChoiceId = existing.Id;
                }

                _db.MultipleChoices.Update(existing);
                await _db.SaveChangesAsync();

                _logger.LogInformation("[MultipleChoiceRepository] Updated MultipleChoice Id={Id}", question.Id);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[MultipleChoiceRepository] Update failed for MultipleChoice {@Question}, error: {e}", question, e.Message);
                return false;
            }
        }

        // Slett spørsmål
        public async Task<bool> Delete(int id)
        {
            try
            {
                var question = await _db.MultipleChoices
                    .Include(q => q.Options)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (question == null)
                {
                    _logger.LogWarning("[MultipleChoiceRepository] Tried to delete non-existing question Id={Id}", id);
                    return false;
                }

                _db.Options.RemoveRange(question.Options);
                _db.MultipleChoices.Remove(question);
                await _db.SaveChangesAsync();

                _logger.LogInformation("[MultipleChoiceRepository] Deleted MultipleChoice Id={Id}", id);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[MultipleChoiceRepository] Deletion failed for Id={Id}, error: {e}", id, e.Message);
                return false;
            }
        }
    }
}
