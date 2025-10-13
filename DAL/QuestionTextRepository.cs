using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL
{
    // Repository som håndterer alle databaseoperasjoner for QuestionText-objekter
    public class QuestionTextRepository : IQuestionTextRepository
    {
        private readonly QuizDbContext _db;                     // Databasekontekst
        private readonly ILogger<QuestionTextRepository> _logger; // Logger for feilsøking og logging

        // Konstruktør som mottar databasekonteksten og loggeren via dependency injection
        public QuestionTextRepository(QuizDbContext db, ILogger<QuestionTextRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        // Henter alle spørsmål fra databasen uten sporing 
        public async Task<List<QuestionText>> GetAllAsync()
        {
            try
            {
                return await _db.Questions.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved henting av alle spørsmål.");
                return new List<QuestionText>();
            }
        }

        // Henter ett spesifikt spørsmål basert på ID
        public async Task<QuestionText?> GetByIdAsync(int id)
        {
            try
            {
                return await _db.Questions.FirstOrDefaultAsync(q => q.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved henting av spørsmål med Id={Id}", id);
                return null;
            }
        }

        // Legger til et nytt spørsmål i databasen
        public async Task AddAsync(QuestionText q)
        {
            try
            {
                await _db.Questions.AddAsync(q);
                _logger.LogInformation("La til nytt spørsmål: {Text}", q.QuestionTexts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved lagring av nytt spørsmål.");
                throw;
            }
        }

        // Sletter et spørsmål hvis det finnes i databasen
        public async Task DeleteAsync(int id)
        {
            try
            {
                var q = await _db.Questions.FindAsync(id);
                if (q != null)
                {
                    _db.Questions.Remove(q);
                    _logger.LogInformation("Slettet spørsmål med Id={Id}", id);
                }
                else
                {
                    _logger.LogWarning("Forsøkte å slette spørsmål som ikke eksisterer. Id={Id}", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved sletting av spørsmål Id={Id}", id);
                throw;
            }
        }

        // Lagrer alle endringer som er gjort i databasen
        public async Task SaveAsync()
        {
            try
            {
                await _db.SaveChangesAsync();
                _logger.LogInformation("Endringer lagret til databasen.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved lagring av endringer til databasen.");
                throw;
            }
        }
    }
}
