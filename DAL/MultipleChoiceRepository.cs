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
                return await _context.MultipleChoiceQuestions
                    .Include(q => q.Options) // Laster inn alle alternativer sammen med spørsmålet
                    .AsNoTracking() // Gjør at resultatet ikke spores av EF for bedre ytelse ved kun lesing
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(ex, "Error fetching all multiple choice questions.");
                return new List<MultipleChoice>();
            }
        }

        // Henter ett spesifikt spørsmål med alle alternativer
        public async Task<MultipleChoice?> GetDetailedAsync(int id)
        {
            try
            {
                return await _context.MultipleChoiceQuestions
                    .Include(q => q.Options) // Inkluderer alle alternativer
                    .FirstOrDefaultAsync(q => q.MultipleChoiceId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching multiple choice question with id {Id}", id);
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

                // Legg til spørsmålet i databasen
                await _context.MultipleChoiceQuestions.AddAsync(question);
                _logger.LogInformation("Added new multiple choice question: {Text}", question.QuestionText);
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
                // Henter eksisterende spørsmål fra databasen
                var existing = await _context.MultipleChoiceQuestions
                    .Include(q => q.Options)
                    .FirstOrDefaultAsync(q => q.Id == question.Id);

                if (existing == null)
                {
                    _logger.LogWarning("[MultipleChoiceRepository] Tried to update non-existing question Id={Id}", question.Id);
                    return false;
                }

                // Oppdaterer spørsmålets tekst
                existing.QuestionText = question.QuestionText;

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

                // Oppdater spørsmålet i databasen
                _context.MultipleChoiceQuestions.Update(existing);
                _logger.LogInformation("Updated question Id={Id}", question.MultipleChoiceId);
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
                var question = await _context.MultipleChoiceQuestions
                    .Include(q => q.Options)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (question == null)
                {
                    // Fjern alle alternativer som hører til spørsmålet
                    _context.Options.RemoveRange(question.Options);

                    // Slett selve spørsmålet
                    _context.MultipleChoiceQuestions.Remove(question);

                    _logger.LogInformation("Deleted multiple choice question Id={Id}", id);
                }
                else
                {
                    _logger.LogWarning("Attempted to delete non-existing question Id={Id}", id);
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
