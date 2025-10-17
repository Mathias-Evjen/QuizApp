using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.ViewModels;

namespace QuizApp.Controllers
{
    public class TrueFalseController : Controller
    {
        private readonly ITrueFalseRepository _trueFalseRepository;
        private readonly ITrueFalseAttemptRepository _trueFalseAttemptRepository;
        private readonly ILogger<TrueFalseController> _logger;

        public TrueFalseController(ITrueFalseRepository trueFalseRepository,
                                   ITrueFalseAttemptRepository trueFalseAttemptRepository,
                                   ILogger<TrueFalseController> logger)
        {
            _trueFalseRepository = trueFalseRepository;
            _trueFalseAttemptRepository = trueFalseAttemptRepository;
            _logger = logger;
        }

        // GET: /TrueFalse
        public async Task<IActionResult> Index()
        {
            try
            {
                var questions = await _trueFalseRepository.GetAllAsync();
                return View(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load TrueFalse list.");
                return View("Error");
            }
        }

        public IActionResult Question(QuizViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitQuestion(int quizId, int quizAttemptId, int quizQuestionId, int quizQuestionNum, int numOfQuestions, bool userAnswer)
        {
            var trueFalse = await _trueFalseRepository.GetByIdAsync(quizQuestionId);
            if (trueFalse == null)
            {
                _logger.LogError("[TrueFalseController - Submit question] TrueFalse question not found for the Id {Id: 0000}", quizQuestionId);
                return NotFound("TrueFalse question not found.");
            }

            var trueFalseAttempt = new TrueFalseAttempt();
            trueFalseAttempt.TrueFalseId = trueFalse.TrueFalseId;
            trueFalseAttempt.QuizAttemptId = quizAttemptId;
            trueFalseAttempt.UserAnswer = userAnswer;

            var returnOk = await _trueFalseAttemptRepository.CreateTrueFalseAttempt(trueFalseAttempt);
            if (!returnOk)
            {
                _logger.LogError("[TrueFalseController] Question attempt creation failed {@attempt}", trueFalseAttempt);
                return RedirectToAction("Quizzes", "Quiz");
            }

            if (trueFalse.QuizQuestionNum == numOfQuestions)
                return RedirectToAction("Results", "Quiz", new { quizAttemptId = quizAttemptId });

            return RedirectToAction("NextQuestion", "Quiz", new
            {
                quizId = quizId,
                quizAttemptId = quizAttemptId,
                quizQuestionNum = quizQuestionNum
            });
        }

        // GET: /Create
        [HttpGet]
        public IActionResult Create() => View();

        // POST: /Create
        [HttpPost]
        public async Task<IActionResult> Create(TrueFalse question)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state on TrueFalse Create.");
                return View(question);
            }

            try
            {
                await _trueFalseRepository.AddAsync(question);
                await _trueFalseRepository.SaveAsync();

                _logger.LogInformation("TrueFalse created: {Question}", question.TrueFalseId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating TrueFalse.");
                return View("Error");
            }
        }

        // GET: /Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var question = await _trueFalseRepository.GetByIdAsync(id);
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

        // POST: /Edit
        [HttpPost]
        public async Task<IActionResult> Edit(TrueFalse question)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state on TrueFalse Edit. Id={Id}", question.TrueFalseId);
                return View(question);
            }

            try
            {
                await _trueFalseRepository.UpdateAsync(question);
                await _trueFalseRepository.SaveAsync();

                _logger.LogInformation("TrueFalse updated: Id={Id}", question.TrueFalseId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating TrueFalse. Id={Id}", question.TrueFalseId);
                return View("Error");
            }
        }

        // GET: /Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var question = await _trueFalseRepository.GetByIdAsync(id);
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

        // POST: /Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _trueFalseRepository.DeleteAsync(id);
                await _trueFalseRepository.SaveAsync();

                _logger.LogInformation("TrueFalse deleted: Id={Id}", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deletinSg TrueFalse. Id={Id}", id);
                return View("Error");
            }
        }
    }
}
