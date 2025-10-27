using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL
{
    public class TrueFalseRepository : IRepository<TrueFalse>
    {
        private readonly QuizDbContext _db;
        private readonly ILogger<TrueFalseRepository> _logger;

        public TrueFalseRepository(QuizDbContext db, ILogger<TrueFalseRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        // Henter alle True/False-spørsmål
        public async Task<IEnumerable<TrueFalse>?> GetAll()
        {
            try
            {
                return await _db.TrueFalseQuestions
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[TrueFalseRepository] GetAll() failed: {Message}", e.Message);
                return null;
            }
        }

        // Henter et spesifikt spørsmål basert på ID
        public async Task<TrueFalse?> GetById(int id)
        {
            try
            {
                // AsNoTracking for “read-only” scenarier
                return await _db.TrueFalseQuestions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(q => q.TrueFalseId == id);
            }
            catch (Exception e)
            {
                _logger.LogError("[TrueFalseRepository] GetById({Id}) failed: {Message}", id, e.Message);
                return null;
            }
        }

        // Oppretter et nytt True/False-spørsmål
        public async Task<bool> Create(TrueFalse question)
        {
            try
            {
                _db.TrueFalseQuestions.Add(question);
                await _db.SaveChangesAsync();
                _logger.LogInformation("[TrueFalseRepository] Created True/False question: Id={Id}, Text={Text}",
                    question.TrueFalseId, question.QuestionText);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[TrueFalseRepository] Create() failed: {Message}", e.Message);
                return false;
            }
        }

        // Oppdaterer et eksisterende True/False-spørsmål
        public async Task<bool> Update(TrueFalse question)
        {
            try
            {
                _db.TrueFalseQuestions.Update(question);
                await _db.SaveChangesAsync();
                _logger.LogInformation("[TrueFalseRepository] Updated True/False question Id={Id}", question.TrueFalseId);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[TrueFalseRepository] Update() failed for Id={Id}: {Message}", question.TrueFalseId, e.Message);
                return false;
            }
        }

        // Sletter et True/False-spørsmål
        public async Task<bool> Delete(int id)
        {
            try
            {
                var question = await _db.TrueFalseQuestions.FirstOrDefaultAsync(q => q.TrueFalseId == id);
                if (question == null)
                {
                    _logger.LogWarning("[TrueFalseRepository] Delete() called for non-existent Id={Id}", id);
                    return false;
                }

                _db.TrueFalseQuestions.Remove(question);
                await _db.SaveChangesAsync();
                _logger.LogInformation("[TrueFalseRepository] Deleted True/False question Id={Id}", id);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[TrueFalseRepository] Delete() failed for Id={Id}: {Message}", id, e.Message);
                return false;
            }
        }
    }
}
