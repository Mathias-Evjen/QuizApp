using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizApp.DAL
{
    public class TrueFalseRepository : ITrueFalseRepository
    {
        private readonly AppDbContext _context;

        public TrueFalseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrueFalseQuestion>> GetAllAsync()
        {
            return await _context.TrueFalseQuestions
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TrueFalseQuestion?> GetByIdAsync(int id)
        {
            return await _context.TrueFalseQuestions.FindAsync(id);
        }

        // ✅ Ny metode lagt til for å oppfylle interfacet
        public async Task<TrueFalseQuestion?> GetDetailedAsync(int id)
        {
            return await _context.TrueFalseQuestions
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task AddAsync(TrueFalseQuestion question)
        {
            await _context.TrueFalseQuestions.AddAsync(question);
        }

        public async Task UpdateAsync(TrueFalseQuestion question)
        {
            _context.TrueFalseQuestions.Update(question);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var question = await _context.TrueFalseQuestions.FindAsync(id);
            if (question != null)
            {
                _context.TrueFalseQuestions.Remove(question);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
