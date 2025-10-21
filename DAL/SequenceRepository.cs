using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL;

public class SequenceRepository : IRepository<Sequence>
{
    private readonly QuizDbContext _db;
    private readonly ILogger<SequenceRepository> _logger;

    public SequenceRepository(QuizDbContext db, ILogger<SequenceRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Sequence>?> GetAll()
    {
        try
        {
            return await _db.SequenceQuestions.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[SequenceRepository] ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<Sequence?> GetById(int id)
    {
        try
        {
            return await _db.SequenceQuestions.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[SequenceRepository] Sequence FindAsync(id) failed when GetSequenceId {SequenceId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(Sequence Sequence)
    {
        try
        {
            _db.SequenceQuestions.Add(Sequence);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[SequenceRepository] Sequence creation failed for Sequence {@Sequence}, error message: {e}", Sequence, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(Sequence Sequence)
    {
        try
        {
            _db.SequenceQuestions.Update(Sequence);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[SequenceRepository] Sequence update failed for Sequence {@Sequence}, error message: {e}", Sequence, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var Sequence = await _db.SequenceQuestions.FindAsync(id);
            if (Sequence == null)
            {
                return false;
            }

            _db.SequenceQuestions.Remove(Sequence);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[SequenceRepository] Sequence deletion failed for the SequenceId {SequenceId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}