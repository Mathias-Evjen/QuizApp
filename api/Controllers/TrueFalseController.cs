using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.Services;
using QuizApp.ViewModels;

namespace QuizApp.Controllers
{
    public class TrueFalseController : Controller
    {
        private readonly IQuestionRepository<TrueFalse> _trueFalseRepository;
        private readonly IAttemptRepository<TrueFalseAttempt> _trueFalseAttemptRepository;
        private readonly QuizService _quizService;
        private readonly ILogger<TrueFalseController> _logger;

        public TrueFalseController(IQuestionRepository<TrueFalse> trueFalseRepository,
                                   IAttemptRepository<TrueFalseAttempt> trueFalseAttemptRepository,
                                   QuizService quizService,
                                   ILogger<TrueFalseController> logger)
        {
            _trueFalseRepository = trueFalseRepository;
            _trueFalseAttemptRepository = trueFalseAttemptRepository;
            _quizService = quizService;
            _logger = logger;
        }

        // GET: /TrueFalse
        public async Task<IActionResult> Index()
        {
            try
            {
                var questions = await _trueFalseRepository.GetAll();
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
            var trueFalse = await _trueFalseRepository.GetById(quizQuestionId);
            if (trueFalse == null)
            {
                _logger.LogError("[TrueFalseController - Submit question] TrueFalse question not found for the Id {Id: 0000}", quizQuestionId);
                return NotFound("TrueFalse question not found.");
            }

            var trueFalseAttempt = new TrueFalseAttempt
            {
                TrueFalseId = trueFalse.TrueFalseId,
                QuizAttemptId = quizAttemptId,
                UserAnswer = userAnswer
            };

            var returnOk = await _trueFalseAttemptRepository.Create(trueFalseAttempt);
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
        public IActionResult Create(int quizId, int numOfQuestions) {
            var question = new TrueFalse
            {
                QuizId = quizId,
                QuizQuestionNum = numOfQuestions + 1
            };
            return View(question);
        }

        // POST: /Create
        [HttpPost]
        public async Task<IActionResult> Create(TrueFalse question)
        {
            if (ModelState.IsValid)
            {
                bool returnOk = await _trueFalseRepository.Create(question);
                if (returnOk)
                {
                    _logger.LogInformation("TrueFalse created: {Question}", question.TrueFalseId);
                    await _quizService.ChangeQuestionCount(question.QuizId, true);
                    return RedirectToAction("ManageQuiz", "Quiz", new { quizId = question.QuizId});
                }
            }

            _logger.LogError("[TrueFalseController] Error creating TrueFalse.");
            return View("Error");
        }

        // GET: /Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var question = await _trueFalseRepository.GetById(id);
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
                bool returnOk = await _trueFalseRepository.Update(question);
                _logger.LogInformation("TrueFalse updated: Id={Id}", question.TrueFalseId);
                return RedirectToAction("ManageQuiz", "Quiz", new { quizId = question.QuizId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating TrueFalse. Id={Id}", question.TrueFalseId);
                return View(question);
            }
        }

        // GET: /Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var question = await _trueFalseRepository.GetById(id);
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
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int questionId, int qNum, int quizId)
        {
            try
            {
                bool returnOk = await _trueFalseRepository.Delete(questionId);
                if (!returnOk)
                {
                    _logger.LogError("Error deletinSg TrueFalse. Id={Id}", questionId);
                    return BadRequest("Question deletion failed");
                }

                _logger.LogInformation("TrueFalse deleted: Id={Id}", questionId);

                await _quizService.ChangeQuestionCount(quizId, false);
                await _quizService.UpdateQuestionNumbers(qNum, quizId);
                return RedirectToAction("ManageQuiz", "Quiz", new { quizId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deletinSg TrueFalse. Id={Id}", questionId);
                return View("Error");
            }
        }
    }
}