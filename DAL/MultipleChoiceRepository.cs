using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL
{
    public class MultipleChoiceRepository : IMultipleChoiceRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MultipleChoiceRepository> _logger;

        public MultipleChoiceRepository(AppDbContext context, ILogger<MultipleChoiceRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<MultipleChoice>> GetAllAsync()
        {
            try
            {
                return await _context.MultipleChoices
                    .Include(q => q.Options)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all multiple choice questions.");
                return new List<MultipleChoice>();
            }
        }

        public async Task<MultipleChoice?> GetDetailedAsync(int id)
        {
            try
            {
                return await _context.MultipleChoices
                    .Include(q => q.Options)
                    .FirstOrDefaultAsync(q => q.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching multiple choice question with id {Id}", id);
                return null;
            }
        }

        public async Task AddAsync(MultipleChoice question)
        {
            try
            {
                question.Options = (question.Options ?? new List<Option>())
                    .Where(o => !string.IsNullOrWhiteSpace(o.Text))
                    .ToList();

                foreach (var opt in question.Options)
                {
                    opt.MultipleChoice = question;
                }

                await _context.MultipleChoices.AddAsync(question);
                _logger.LogInformation("Added new multiple choice question: {Text}", question.QuestionText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding multiple choice question.");
                throw;
            }
        }

        public async Task UpdateAsync(MultipleChoice question)
        {
            try
            {
                var existing = await _context.MultipleChoices
                    .Include(q => q.Options)
                    .FirstOrDefaultAsync(q => q.Id == question.Id);

                if (existing == null)
                {
                    _logger.LogWarning("Attempted to update non-existing question Id={Id}", question.Id);
                    return;
                }

                existing.QuestionText = question.QuestionText;

                _context.Options.RemoveRange(existing.Options);

                existing.Options = question.Options
                    .Where(o => !string.IsNullOrWhiteSpace(o.Text))
                    .ToList();

                foreach (var opt in existing.Options)
                {
                    opt.MultipleChoiceId = existing.Id;
                }

                _context.MultipleChoices.Update(existing);
                _logger.LogInformation("Updated question Id={Id}", question.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating question Id={Id}", question.Id);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var question = await _context.MultipleChoices
                    .Include(q => q.Options)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (question != null)
                {
                    _context.Options.RemoveRange(question.Options);
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

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
