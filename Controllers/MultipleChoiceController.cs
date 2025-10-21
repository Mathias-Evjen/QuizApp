using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.ViewModels;

namespace QuizApp.Controllers
{
    public class MultipleChoiceController : Controller
    {
        private readonly IRepository<MultipleChoice> _multipleChoiceRepository;
        private readonly IAttemptRepository<MultipleChoiceAttempt> _multipleChoiceAttemptRepository;
        private readonly ILogger<MultipleChoiceController> _logger;

        public MultipleChoiceController(IRepository<MultipleChoice> multipleChoiceRepository,
                                        IAttemptRepository<MultipleChoiceAttempt> multipleChoiceAttemptRepository,
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
                var questions = await _multipleChoiceRepository.GetAll();
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
        public async Task<IActionResult> SubmitQuestion(int quizId, int quizAttemptId, int quizQuestionId, int quizQuestionNum, int numOfQuestions, string userAnswer)
        {
            var multipleChoice = await _multipleChoiceRepository.GetById(quizQuestionId);
            if (multipleChoice == null)
            {
                _logger.LogError("[MultipleChoiceController - Submit question] MultipleChoice question not found for the Id {Id: 0000}", quizQuestionId);
                return NotFound("MultipleChoice question not found.");
            }

            var multipleChoiceAttempt = new MultipleChoiceAttempt
            {
                MultiplechoiceId = multipleChoice.MultipleChoiceId,
                QuizAttemptId = quizAttemptId,
                UserAnswer = userAnswer
            };

            var returnOk = await _multipleChoiceAttemptRepository.Create(multipleChoiceAttempt);
            if (!returnOk)
            {
                _logger.LogError("[MultipleChoiceController] Question attempt creation failed {@attempt}", multipleChoiceAttempt);
                return RedirectToAction("Quizzes", "Quiz");
            }

            if (multipleChoice.QuizQuestionNum == numOfQuestions)
                return RedirectToAction("Results", "Quiz", new { quizAttemptId = quizAttemptId });

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

            var question = new MultipleChoice
            {
                Options =
                [
                    new Option(),
                    new Option(),
                    new Option(),
                    new Option()
                ]
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
                    opt.MultipleChoice = question;

                var ok = await _multipleChoiceRepository.Create(question);
                if (!ok)
                {
                    _logger.LogError("Failed to create MultipleChoice question.");
                    return View("Error");
                }

                _logger.LogInformation("Created MultipleChoice question: {Question}", question.QuestionText);
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
                var question = await _multipleChoiceRepository.GetById(id);
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
                var returnOk = await _multipleChoiceRepository.Update(question);
                if (!returnOk)
                {
                    _logger.LogError("Failed to update MultipleChoice question Id={Id}", question.MultipleChoiceId);
                    return View("Error");
                }

                _logger.LogInformation("Updated MultipleChoice question Id={Id}", question.MultipleChoiceId);
                return RedirectToAction("ManageQuiz", "Quiz", new { quizId = question.QuizId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating MultipleChoice question Id={Id}", question.MultipleChoiceId);
                return View("Error");
            }
        }

        // GET: /Delete
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var question = await _multipleChoiceRepository.GetById(id);
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
                var ok = await _multipleChoiceRepository.Delete(id);
                if (!ok)
                {
                    _logger.LogError("Failed to delete MultipleChoice question Id={Id}", id);
                    return View("Error");
                }

                _logger.LogInformation("Deleted MultipleChoice question Id={Id}", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting MultipleChoice question Id={Id}", id);
                return View("Error");
            }
        }
    }
}