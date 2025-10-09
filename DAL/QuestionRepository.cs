using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL
{
    // eksempel: QuestionRepository
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _db;
        public QuestionRepository(AppDbContext db) => _db = db;

        public async Task<List<Question>> GetAllAsync() =>
            await _db.Questions.AsNoTracking().ToListAsync();

        public async Task<Question?> GetByIdAsync(int id) =>
            await _db.Questions.FirstOrDefaultAsync(q => q.Id == id);

        public async Task AddAsync(Question q) =>
            await _db.Questions.AddAsync(q);

        public async Task DeleteAsync(int id)
        {
            var q = await _db.Questions.FindAsync(id);
            if (q != null) _db.Questions.Remove(q);
        }

        public Task SaveAsync() => _db.SaveChangesAsync();
    }
}
