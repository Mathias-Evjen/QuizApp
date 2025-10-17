using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.ViewModels;

namespace QuizApp.Controllers;

public class MultipleChoiceController : Controller
{
    private readonly IMultipleChoiceRepository _multipleChoiceRepository;
    private readonly ILogger<MultipleChoiceController> _logger;

    public MultipleChoiceController(IMultipleChoiceRepository multipleChoiceRepository, ILogger<MultipleChoiceController> logger)
    {
        private readonly IMultipleChoiceRepository _multipleChoiceRepository;
        private readonly IMultipleChoiceAttemptRepository _multipleChoiceAttemptRepository;
        private readonly ILogger<MultipleChoiceController> _logger;

        public MultipleChoiceController(IMultipleChoiceRepository multipleChoiceRepository,
                                        IMultipleChoiceAttemptRepository multipleChoiceAttemptRepository,
                                        ILogger<MultipleChoiceController> logger)
        {
            _multipleChoiceRepository = multipleChoiceRepository;
            _multipleChoiceAttemptRepository = multipleChoiceAttemptRepository;
            _logger = logger;
        }

        // GET: /MultipleChoice
        public async Task<IActionResult> Index()
        {
            try
            {
                var questions = await _multipleChoiceRepository.GetAllAsync();
                return View(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load Multiple Choice list.");
                return View("Error");
            }
        }

        public IActionResult Question(QuizViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitQuestion(int quizId, int quizAttemptId, int quizQuestionId, int quizQuestionNum, string userAnswer)
        {
            var multipleChoice = await _multipleChoiceRepository.GetDetailedAsync(quizQuestionId);
            if (multipleChoice == null)
            {
                _logger.LogError("[MultipleChoiceController - Submit question] MultipleChoice question not found for the Id {Id: 0000}", quizQuestionId);
                return NotFound("MultipleChoice question not found.");
            }

            var multipleChoiceAttempt = new MultipleChoiceAttempt();
            multipleChoiceAttempt.MultiplechoiceId = multipleChoice.MultipleChoiceId;
            multipleChoiceAttempt.QuizAttemptId = quizAttemptId;
            multipleChoiceAttempt.UserAnswer = userAnswer;

            var returnOk = await _multipleChoiceAttemptRepository.CreateMultipleChoiceAttempt(multipleChoiceAttempt);
            if (!returnOk)
            {
                _logger.LogError("[MultipleChoiceController] Question attempt creation failed {@attempt}", multipleChoiceAttempt);
                return RedirectToAction("Quizzes", "Quiz");
            }

            return RedirectToAction("NextQuestion", "Quiz", new
            {
                quizId = quizId,
                quizAttemptId = quizAttemptId,
                quizQuestionNum = quizQuestionNum
            });
        }

        // GET: /Create
        public IActionResult Create()
        {

    [HttpPost]
    public async Task<IActionResult> CreateMultipleChoiceQuestion(MultipleChoice question)
    {
        if (!ModelState.IsValid)
        {
            return View(question);
        }

        var ok = await _multipleChoiceRepository.Create(question);
        if (!ok)
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

                await _multipleChoiceRepository.AddAsync(question);
                await _multipleChoiceRepository.SaveAsync();

                _logger.LogInformation("MultipleChoice created: {Question}", question.QuestionText);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating MultipleChoice question.");
                return View("Error");
            }
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> EditMultipleChoiceQuestion(int id)
    {
        var question = await _multipleChoiceRepository.GetById(id);
        if (question == null)
        {
            try
            {
                var question = await _multipleChoiceRepository.GetDetailedAsync(id);
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

        return View(question);
    }

    [HttpPost]
    public async Task<IActionResult> EditMultipleChoiceQuestion(MultipleChoice question)
    {
        if (!ModelState.IsValid)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state on MultipleChoice Edit. Id={Id}", question.MultipleChoiceId);
                return View(question);
            }

            try
            {
                await _multipleChoiceRepository.UpdateAsync(question);
                await _multipleChoiceRepository.SaveAsync();

                _logger.LogInformation("MultipleChoice updated: Id={Id}", question.MultipleChoiceId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating MultipleChoice. Id={Id}", question.MultipleChoiceId);
                return View("Error");
            }
        }

        var ok = await _multipleChoiceRepository.Update(question);
        if (!ok)
        {
            try
            {
                var question = await _multipleChoiceRepository.GetDetailedAsync(id);
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
                await _multipleChoiceRepository.DeleteAsync(id);
                await _multipleChoiceRepository.SaveAsync();

    [HttpGet]
    public async Task<IActionResult> DeleteMultipleChoiceQuestion(int id)
    {
        var question = await _multipleChoiceRepository.GetById(id);
        if (question == null)
        {
            _logger.LogError("[MultipleChoiceController] Question not found for deletion, Id={Id:0000}", id);
            return NotFound();
        }

        return View(question);
    }

    [HttpPost, ActionName("DeleteMultipleChoiceQuestion")]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        var ok = await _multipleChoiceRepository.Delete(id);
        if (!ok)
        {
            _logger.LogError("[MultipleChoiceController] Failed to delete question Id={Id:0000}", id);
        }

        return RedirectToAction("Index");
    }
}
