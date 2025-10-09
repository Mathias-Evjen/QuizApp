using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL
{
    public class MultipleChoiceRepository : IMultipleChoiceRepository
    {
        private readonly AppDbContext _db;
        public MultipleChoiceRepository(AppDbContext db) => _db = db;

        public async Task<List<MultipleChoice>> GetAllWithOptionsAsync() =>
            await _db.MultipleChoices.Include(q => q.Options)
                .AsNoTracking().ToListAsync();

        public async Task<MultipleChoice?> GetWithOptionsAsync(int id) =>
            await _db.MultipleChoices.Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);

        public async Task AddAsync(MultipleChoice q) =>
            await _db.MultipleChoices.AddAsync(q);

        public Task UpdateAsync(MultipleChoice q)
        {
            _db.MultipleChoices.Update(q);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var q = await _db.MultipleChoices.FindAsync(id);
            if (q != null) _db.MultipleChoices.Remove(q);
        }

        public Task SaveAsync() => _db.SaveChangesAsync();
    }
}
