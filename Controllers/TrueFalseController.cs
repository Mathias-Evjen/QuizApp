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
            var questions = await _trueFalseRepository.GetAll();
            if (questions == null)
            {
                var questions = await _trueFalseRepository.GetAllAsync();
                return View(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load TrueFalse list.");
                return View("Error");
            }

            return View(questions);
        }

        public IActionResult Question(QuizViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitQuestion(int quizId, int quizAttemptId, int quizQuestionId, int quizQuestionNum, bool userAnswer)
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
                _logger.LogWarning("[TrueFalseController] Invalid model state while creating question");
                return View(question);
            }

            var ok = await _trueFalseRepository.Create(question);
            if (!ok)
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
            {
                var question = await _trueFalseRepository.GetByIdAsync(id);
                if (question == null)
                {
                    _logger.LogWarning("[TrueFalseController] Question not found for Id {Id:0000}", id);
                    return NotFound("Question not found");
                }

                return View(question);
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

            var ok = await _trueFalseRepository.Update(question);
            if (!ok)
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
            
            var question = await _trueFalseRepository.GetById(id);
            if (question == null)
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
            return View(question);
        }

        // POST: /Delete
        [HttpPost, ActionName("DeleteTrueFalseQuestion")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var ok = await _trueFalseRepository.Delete(id);
            if (!ok)
            {
                await _trueFalseRepository.DeleteAsync(id);
                await _trueFalseRepository.SaveAsync();

            return RedirectToAction("Index");
        }
    }
}
