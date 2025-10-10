using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.DAL
{
    public class MultipleChoiceRepository : IMultipleChoiceRepository
    {
        private readonly AppDbContext _context;

        public MultipleChoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MultipleChoice>> GetAllAsync()
        {
            return await _context.MultipleChoices
                .Include(q => q.Options)
                .ToListAsync();
        }

        public async Task<MultipleChoice?> GetByIdAsync(int id)
        {
            return await _context.MultipleChoices.FindAsync(id);
        }

        public async Task<MultipleChoice?> GetDetailedAsync(int id)
        {
            return await _context.MultipleChoices
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<MultipleChoice?> GetWithOptionsAsync(int id)
        {
            return await _context.MultipleChoices
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task AddAsync(MultipleChoice question)
        {
            await _context.MultipleChoices.AddAsync(question);
        }

        public async Task UpdateAsync(MultipleChoice question)
        {
            _context.MultipleChoices.Update(question);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var question = await _context.MultipleChoices.FindAsync(id);
            if (question != null)
            {
                _context.MultipleChoices.Remove(question);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
