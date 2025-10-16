using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;

namespace QuizApp.Controllers
{
    public class TrueFalseController : Controller
    {
        private readonly ITrueFalseRepository _trueFalseRepository;
        private readonly ILogger<TrueFalseController> _logger;

        public TrueFalseController(ITrueFalseRepository trueFalseRepository, ILogger<TrueFalseController> logger)
        {
            _trueFalseRepository = trueFalseRepository;
            _logger = logger;
        }

        // GET: /TrueFalse
        public async Task<IActionResult> Index()
        {
            var questions = await _trueFalseRepository.GetAll();
            if (questions == null)
            {
                _logger.LogError("[TrueFalseController] Could not fetch True/False questions from repository.");
                return NotFound("Questions not found");
            }

            return View(questions);
        }

        // GET: /Create
        [HttpGet]
        public IActionResult CreateTrueFalseQuestion()
        {
            return View();
        }

        // POST: /Create
        [HttpPost]
        public async Task<IActionResult> CreateTrueFalseQuestion(TrueFalseQuestion question)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("[TrueFalseController] Invalid model state while creating question");
                return View(question);
            }

            var ok = await _trueFalseRepository.Create(question);
            if (!ok)
            {
                _logger.LogError("[TrueFalseController] Failed to create question {@Question}", question);
                return View(question);
            }

            return RedirectToAction("Index");
        }

        // GET: /Edit
        [HttpGet]
        public async Task<IActionResult> EditTrueFalseQuestion(int id)
        {
            {
                var question = await _trueFalseRepository.GetById(id);
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
        public async Task<IActionResult> EditTrueFalseQuestion(TrueFalseQuestion question)
        {
            if (!ModelState.IsValid)
            {
                return View(question);
            }

            var ok = await _trueFalseRepository.Update(question);
            if (!ok)
            {
                _logger.LogError("[TrueFalseController] Failed to update question with Id {Id:0000}", question.Id);
                return View(question);
            }

            return RedirectToAction("Index");
        }

        // GET: /Delete
        [HttpGet]
        public async Task<IActionResult> DeleteTrueFalseQuestion(int id)
        {
            
            var question = await _trueFalseRepository.GetById(id);
            if (question == null)
            {
                _logger.LogWarning("[TrueFalseController] Question not found for deletion, Id={Id:0000}", id);
                return NotFound();
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
                _logger.LogError("[TrueFalseController] Failed to delete question with Id={Id:0000}", id);
            }

            return RedirectToAction("Index");
        }
    }
}
