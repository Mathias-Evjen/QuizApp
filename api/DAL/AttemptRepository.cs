using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace QuizApp.DAL
{
    public class AttemptRepository<T> : IAttemptRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly ILogger<AttemptRepository<T>> _logger;

        public AttemptRepository(QuizDbContext context, ILogger<AttemptRepository<T>> logger)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _logger = logger;
        }

        public async Task<IEnumerable<T>?> GetAll()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[AttemptRepository] ToListAsync() falied when GetAll(), error message: {e}", e.Message);
                return null;
            }
        }

        public async Task<T?> GetById(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError("[AttemptRepository] FindAsync(id) failed when GetById {Id:0000}, error message: {e}", id, e.Message);
                return null;
            }
        }

        public async Task<bool> Create(T t)
        {
            try
            {
                _dbSet.Add(t);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[AttemptRepository] Creation failed for Model {@t}, error message: {e}", t, e.Message);
                return false;
            }
        }

        public async Task<bool> Update(T t)
        {
            try
            {
                _dbSet.Update(t);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[AttemptRepository] Update failed for Model {@t}, error message: {e}", t, e.Message);
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var flashCard = await _dbSet.FindAsync(id);
                if (flashCard == null)
                {
                    return false;
                }

                _dbSet.Remove(flashCard);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[AttemptRepository] Deletion failed for the Id {Id:0000}, error message: {e}", id, e.Message);
                return false;
            }
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }
    }
}