using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL
{
    public class MultipleChoiceRepository : IMultipleChoiceRepository
    {
        private readonly QuizDbContext _context;
        private readonly ILogger<MultipleChoiceRepository> _logger;

        public MultipleChoiceRepository(QuizDbContext context, ILogger<MultipleChoiceRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Henter alle MultipleChoice-spørsmål med tilhørende alternativer
        public async Task<List<MultipleChoice>> GetAllAsync()
        {
            try
            {
                return await _context.MultipleChoices
                    .Include(q => q.Options) // Laster inn alle alternativer sammen med spørsmålet
                    .AsNoTracking() // Gjør at resultatet ikke spores av EF for bedre ytelse ved kun lesing
                    .ToListAsync();
            }
            catch (Exception ex)
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
                return await _context.MultipleChoices
                    .Include(q => q.Options) // Inkluderer alle alternativer
                    .FirstOrDefaultAsync(q => q.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching multiple choice question with id {Id}", id);
                return null;
            }
        }

        // Legger til et nytt MultipleChoice-spørsmål
        public async Task AddAsync(MultipleChoice question)
        {
            try
            {
                // Tar bort alternativer som er tomme
                question.Options = (question.Options ?? new List<Option>())
                    .Where(o => !string.IsNullOrWhiteSpace(o.Text))
                    .ToList();

                // Knytter hvert alternativ tilbake til spørsmålet
                foreach (var opt in question.Options)
                {
                    opt.MultipleChoice = question;
                }

                // Legg til spørsmålet i databasen
                await _context.MultipleChoices.AddAsync(question);
                _logger.LogInformation("Added new multiple choice question: {Text}", question.QuestionTexts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding multiple choice question.");
                throw;
            }
        }

        // Oppdaterer et eksisterende spørsmål
        public async Task UpdateAsync(MultipleChoice question)
        {
            try
            {
                // Henter eksisterende spørsmål fra databasen
                var existing = await _context.MultipleChoices
                    .Include(q => q.Options)
                    .FirstOrDefaultAsync(q => q.Id == question.Id);

                if (existing == null)
                {
                    _logger.LogWarning("Attempted to update non-existing question Id={Id}", question.Id);
                    return;
                }

                // Oppdaterer spørsmålets tekst
                existing.QuestionTexts = question.QuestionTexts;

                // Fjerner gamle alternativer før nye legges til
                _context.Options.RemoveRange(existing.Options);

                // Legger til nye alternativer som ikke er tomme
                existing.Options = question.Options
                    .Where(o => !string.IsNullOrWhiteSpace(o.Text))
                    .ToList();

                // Knytter nye alternativer til spørsmålet
                foreach (var opt in existing.Options)
                {
                    opt.MultipleChoiceId = existing.Id;
                }

                // Oppdater spørsmålet i databasen
                _context.MultipleChoices.Update(existing);
                _logger.LogInformation("Updated question Id={Id}", question.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating question Id={Id}", question.Id);
                throw;
            }
        }

        // Sletter et spørsmål og alternativer
        public async Task DeleteAsync(int id)
        {
            try
            {
                var question = await _context.MultipleChoices
                    .Include(q => q.Options)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (question != null)
                {
                    // Fjern alle alternativer som hører til spørsmålet
                    _context.Options.RemoveRange(question.Options);

                    // Slett selve spørsmålet
                    _context.MultipleChoices.Remove(question);

                    _logger.LogInformation("Deleted multiple choice question Id={Id}", id);
                }
                else
                {
                    _logger.LogWarning("Attempted to delete non-existing question Id={Id}", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting question Id={Id}", id);
                throw;
            }
        }

        // Lagrer endringer i databasen
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
