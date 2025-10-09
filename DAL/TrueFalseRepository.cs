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
            return await _context.TrueFalseQuestions.ToListAsync();
        }

        public async Task<TrueFalseQuestion?> GetByIdAsync(int id)
        {
            return await _context.TrueFalseQuestions.FindAsync(id);
        }

        public async Task AddAsync(TrueFalseQuestion q)
        {
            await _context.TrueFalseQuestions.AddAsync(q);
        }

        public async Task UpdateAsync(TrueFalseQuestion q)
        {
            _context.TrueFalseQuestions.Update(q);
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
