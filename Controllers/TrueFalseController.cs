using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;

namespace QuizApp.Controllers
{
    public class TrueFalseController : Controller
    {
        private readonly ITrueFalseRepository _repo;
        private readonly ILogger<TrueFalseController> _logger;

        public TrueFalseController(ITrueFalseRepository repo, ILogger<TrueFalseController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // GET: /TrueFalse
        public async Task<IActionResult> Index()
        {
            try
            {
                var questions = await _repo.GetAllAsync();
                return View(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load TrueFalse list.");
                return View("Error");
            }
        }

        // GET: /TrueFalse/Create
        public IActionResult Create() => View();

        // POST: /TrueFalse/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrueFalseQuestion question)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state on TrueFalse Create.");
                return View(question);
            }

            try
            {
                await _repo.AddAsync(question);
                await _repo.SaveAsync();

                _logger.LogInformation("TrueFalse created: {Question}", question.QuestionText);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating TrueFalse.");
                return View("Error");
            }
        }

        // GET: /TrueFalse/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var question = await _repo.GetByIdAsync(id);
                if (question == null)
                {
                    _logger.LogWarning("Edit requested for non-existing TrueFalse Id={Id}", id);
                    return NotFound();
                }
                return View(question);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load TrueFalse for edit. Id={Id}", id);
                return View("Error");
            }
        }

        // POST: /TrueFalse/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TrueFalseQuestion question)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state on TrueFalse Edit. Id={Id}", question.Id);
                return View(question);
            }

            try
            {
                await _repo.UpdateAsync(question);
                await _repo.SaveAsync();

                _logger.LogInformation("TrueFalse updated: Id={Id}", question.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating TrueFalse. Id={Id}", question.Id);
                return View("Error");
            }
        }

        // GET: /TrueFalse/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var question = await _repo.GetByIdAsync(id);
                if (question == null)
                {
                    _logger.LogWarning("Delete requested for non-existing TrueFalse Id={Id}", id);
                    return NotFound();
                }
                return View(question);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load TrueFalse for delete. Id={Id}", id);
                return View("Error");
            }
        }

        // POST: /TrueFalse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _repo.DeleteAsync(id);
                await _repo.SaveAsync();

                _logger.LogInformation("TrueFalse deleted: Id={Id}", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting TrueFalse. Id={Id}", id);
                return View("Error");
            }
        }
    }
}
