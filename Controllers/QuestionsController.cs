using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;

namespace QuizApp.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IMultipleChoiceRepository _repo;
        private readonly ILogger<QuestionsController> _logger;

        public QuestionsController(IMultipleChoiceRepository repo, ILogger<QuestionsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // ✅ LIST ALL QUESTIONS
        public async Task<IActionResult> Index()
        {
            var questions = await _repo.GetAllAsync();
            return View(questions);
        }

        // ✅ CREATE (GET)
        public IActionResult Create()
        {
            var question = new MultipleChoice
            {
                Options = new List<Option>
                {
                    new Option(),
                    new Option(),
                    new Option(),
                    new Option()
                }
            };
            return View(question);
        }

        // ✅ CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MultipleChoice question)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for Create.");
                return View(question);
            }

            try
            {
                // Fjern tomme alternativer
                question.Options = (question.Options ?? new List<Option>())
                    .Where(o => !string.IsNullOrWhiteSpace(o.Text))
                    .ToList();

                // Knytt sammen relasjonene
                foreach (var opt in question.Options)
                {
                    opt.MultipleChoice = question;
                }

                await _repo.AddAsync(question);
                await _repo.SaveAsync();

                _logger.LogInformation("Added question '{Text}' with {Count} options.",
                    question.QuestionText, question.Options.Count);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating new question.");
                return View("Error");
            }
        }

        // ✅ EDIT (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var question = await _repo.GetDetailedAsync(id);
            if (question == null)
            {
                _logger.LogWarning("Attempted to edit non-existing question Id={Id}", id);
                return NotFound();
            }
            return View(question);
        }

        // ✅ EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MultipleChoice question)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for Edit.");
                return View(question);
            }

            try
            {
                await _repo.UpdateAsync(question);
                await _repo.SaveAsync();

                _logger.LogInformation("Updated question Id={Id}", question.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating question Id={Id}", question.Id);
                return View("Error");
            }
        }

        // ✅ DELETE (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _repo.GetDetailedAsync(id);
            if (question == null)
            {
                _logger.LogWarning("Attempted to delete non-existing question Id={Id}", id);
                return NotFound();
            }

            return View(question);
        }

        // ✅ DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _repo.DeleteAsync(id);
                await _repo.SaveAsync();

                _logger.LogInformation("Deleted question Id={Id}", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting question Id={Id}", id);
                return View("Error");
            }
        }
    }
}
