using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;

namespace QuizApp.Controllers
{
    public class MultipleChoiceController : Controller
    {
        private readonly IMultipleChoiceRepository _repo;
        private readonly ILogger<MultipleChoiceController> _logger;

        public MultipleChoiceController(IMultipleChoiceRepository repo, ILogger<MultipleChoiceController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // GET: /MultipleChoice
        public async Task<IActionResult> Index()
        {
            try
            {
                var questions = await _repo.GetAllAsync();
                return View(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load Multiple Choice list.");
                return View("Error");
            }
        }

        // GET: /Create
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

        // POST: /Create
        [HttpPost]
        public async Task<IActionResult> Create(MultipleChoice question)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state on MultipleChoice Create.");
                return View(question);
            }

            try
            {
                question.Options = (question.Options ?? new List<Option>())
                    .Where(o => !string.IsNullOrWhiteSpace(o.Text))
                    .ToList();

                foreach (var opt in question.Options)
                {
                    opt.MultipleChoice = question;
                }

                await _repo.AddAsync(question);
                await _repo.SaveAsync();

                _logger.LogInformation("MultipleChoice created: {Question}", question.QuestionTexts);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating MultipleChoice question.");
                return View("Error");
            }
        }

        // GET: /Edit
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var question = await _repo.GetDetailedAsync(id);
                if (question == null)
                {
                    _logger.LogWarning("Edit requested for non-existing MultipleChoice Id={Id}", id);
                    return NotFound();
                }

                while (question.Options.Count < 4)
                {
                    question.Options.Add(new Option());
                }

                return View(question);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load MultipleChoice for edit. Id={Id}", id);
                return View("Error");
            }
        }

        // POST: /Edit
        [HttpPost]
        public async Task<IActionResult> Edit(MultipleChoice question)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state on MultipleChoice Edit. Id={Id}", question.MultipleChoiceId);
                return View(question);
            }

            try
            {
                await _repo.UpdateAsync(question);
                await _repo.SaveAsync();

                _logger.LogInformation("MultipleChoice updated: Id={Id}", question.MultipleChoiceId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating MultipleChoice. Id={Id}", question.MultipleChoiceId);
                return View("Error");
            }
        }

        // GET: /Delete
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var question = await _repo.GetDetailedAsync(id);
                if (question == null)
                {
                    _logger.LogWarning("Delete requested for non-existing MultipleChoice Id={Id}", id);
                    return NotFound();
                }

                return View(question);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load MultipleChoice for delete. Id={Id}", id);
                return View("Error");
            }
        }

        // POST: /DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _repo.DeleteAsync(id);
                await _repo.SaveAsync();

                _logger.LogInformation("MultipleChoice deleted: Id={Id}", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting MultipleChoice. Id={Id}", id);
                return View("Error");
            }
        }
    }
}
