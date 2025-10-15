using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL
{
    // Repository som håndterer alle databaseoperasjoner for True/False-spørsmål
    public class TrueFalseRepository : ITrueFalseRepository
    {
        private readonly QuizDbContext _context;                       // Databasekontekst
        private readonly ILogger<TrueFalseRepository> _logger;         // Logger for feilhåndtering og sporing

        // Konstruktør som mottar databasekonteksten og loggeren via dependency injection
        public TrueFalseRepository(QuizDbContext context, ILogger<TrueFalseRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Henter alle True/False-spørsmål fra databasen uten sporing
        public async Task<List<TrueFalseQuestion>> GetAllAsync()
        {
            try
            {
                return await _context.TrueFalseQuestions
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved henting av alle True/False-spørsmål.");
                return new List<TrueFalseQuestion>();
            }
        }

        // Henter ett True/False-spørsmål basert på ID
        public async Task<TrueFalseQuestion?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.TrueFalseQuestions.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved henting av True/False-spørsmål med Id={Id}", id);
                return null;
            }
        }

        // Henter detaljert informasjon om et True/False-spørsmål
        public async Task<TrueFalseQuestion?> GetDetailedAsync(int id)
        {
            try
            {
                return await _context.TrueFalseQuestions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(q => q.TrueFalseQuestionId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved henting av detaljert True/False-spørsmål med Id={Id}", id);
                return null;
            }
        }

        // Legger til et nytt True/False-spørsmål i databasen
        public async Task AddAsync(TrueFalseQuestion question)
        {
            try
            {
                await _context.TrueFalseQuestions.AddAsync(question);
                _logger.LogInformation("La til nytt True/False-spørsmål: {Text}", question.QuestionText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved lagring av nytt True/False-spørsmål.");
                throw;
            }
        }

        // Oppdaterer et eksisterende True/False-spørsmål
        public async Task UpdateAsync(TrueFalseQuestion question)
        {
            try
            {
                _context.TrueFalseQuestions.Update(question);
                _logger.LogInformation("Oppdatert True/False-spørsmål med Id={Id}", question.TrueFalseQuestionId);
                await Task.CompletedTask; // behold async-signaturen
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved oppdatering av True/False-spørsmål Id={Id}", question.TrueFalseQuestionId);
                throw;
            }
        }

        // Sletter et spørsmål hvis det finnes i databasen
        public async Task DeleteAsync(int id)
        {
            try
            {
                var question = await _context.TrueFalseQuestions.FindAsync(id);
                if (question != null)
                {
                    _context.TrueFalseQuestions.Remove(question);
                    _logger.LogInformation("Slettet True/False-spørsmål med Id={Id}", id);
                }
                else
                {
                    _logger.LogWarning("Forsøkte å slette ikke-eksisterende True/False-spørsmål Id={Id}", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved sletting av True/False-spørsmål Id={Id}", id);
                throw;
            }
        }

        // Lagrer alle endringer i databasen
        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
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
