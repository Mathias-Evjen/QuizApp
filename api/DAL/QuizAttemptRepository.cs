using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using System.Linq.Expressions;

namespace QuizApp.DAL
{
    public class QuizAttemptRepository(QuizDbContext db, ILogger<QuizAttemptRepository> logger) : IQuizRepository<QuizAttempt>
    {
        private readonly QuizDbContext _db = db;
        private readonly ILogger<QuizAttemptRepository> _logger = logger;

        public async Task<IEnumerable<QuizAttempt>?> GetAll()
        {
            try
            {
                return await _db.QuizAttempts
                                    .Include(q => q.FillInTheBlankAttempts)
                                    .Include(q => q.TrueFalseQuestionAttempts)
                                    .Include(q => q.MultipleChoiceAttempts)
                                    .Include(q => q.RankingAttempts)
                                    .Include(q => q.SequenceAttempts)
                                    .Include(q => q.MatchingAttempts)
                                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[QuizAttemptRepository] ToListAsync() failed when GetAll(), error message: {e}", e.Message);
                return null;
            }
        }

        public async Task<QuizAttempt?> GetById(int id)
        {
            try
            {
                return await _db.QuizAttempts
                                    .Include(q => q.FillInTheBlankAttempts)
                                    .Include(q => q.TrueFalseQuestionAttempts)
                                    .Include(q => q.MultipleChoiceAttempts)
                                    .Include(q => q.RankingAttempts)
                                    .Include(q => q.SequenceAttempts)
                                    .Include(q => q.MatchingAttempts)
                                    .FirstOrDefaultAsync(q => q.QuizAttemptId == id);
            }
            catch (Exception e)
            {
                _logger.LogError("[QuizAttemptRepository] QuizAttempt FindAsync(id) failed when GetQuizAttemptId {QuizAttemptId:0000}, error message: {e}", id, e.Message);
                return null;
            }
        }
        public bool Exists(int id)
        {
            return _db.QuizAttempts.Any(q => q.QuizAttemptId == id);
        }

        public async Task<bool> Create(QuizAttempt QuizAttempt)
        {
            try
            {
                _db.QuizAttempts.Add(QuizAttempt);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[QuizAttemptRepository] QuizAttempt creation failed for QuizAttempt {@QuizAttempt}, error message: {e}", QuizAttempt, e.Message);
                return false;
            }
        }

        public async Task<bool> Update(QuizAttempt QuizAttempt)
        {
            try
            {
                _db.QuizAttempts.Update(QuizAttempt);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[QuizAttemptRepository] QuizAttempt update failed for QuizAttempt {@QuizAttempt}, error message: {e}", QuizAttempt, e.Message);
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var QuizAttempt = await _db.QuizAttempts.FindAsync(id);
                if (QuizAttempt == null)
                {
                    return false;
                }

                _db.QuizAttempts.Remove(QuizAttempt);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[QuizAttemptRepository] QuizAttempt deletion failed for the QuizAttemptId {QuizAttemptId:0000}, error message: {e}", id, e.Message);
                return false;
            }
        }

        public bool Exists(Expression<Func<QuizAttempt, bool>> predicate)
            {
                return _db.QuizAttempts.Any(predicate);
            }
    }
}