using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.DAL;

namespace QuizApp.Controllers;

public class FlashCardQuizController : Controller
{
    private readonly IRepository<FlashCardQuiz> _flashCardQuizRepository;
    private readonly ILogger<FlashCardQuizController> _logger;
    
    public FlashCardQuizController(IRepository<FlashCardQuiz> flashCardQuizRepository, ILogger<FlashCardQuizController> logger)
    {
        _flashCardQuizRepository = flashCardQuizRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Quizzes()
    {
        var quizzes = await _flashCardQuizRepository.GetAll();
        if (quizzes == null)
        {
            _logger.LogError("[FlashCardQuizcontroller] FlashCardQuizzes list not found while executing _flashCardQuizRepository.GetAll()");
            return NotFound("FlashCardQuizzes not found");
        }
        return View(quizzes);
    }

    [HttpGet]
    public async Task<IActionResult> FlashCardQuiz(int id) 
    {
        var quiz = await _flashCardQuizRepository.GetById(id);
        if (quiz == null)
        {
            _logger.LogError("[FlashCardQuizController] FlashCardQuiz not found for the Id {Id: 0000}", id);
            return NotFound("FlashCardQuiz not found");
        }
        return View(quiz);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(FlashCardQuiz quiz)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _flashCardQuizRepository.Create(quiz);
            if (returnOk)
                return RedirectToAction(nameof(Quizzes));
        }
        _logger.LogError("[FlashCardQuizController] FlashCardQuiz creation failed {@question}", quiz);
        return View(quiz);
    }

    [HttpGet]
    public async Task<IActionResult> ManageQuiz(int quizId)
    {
        var quiz = await _flashCardQuizRepository.GetById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[FlashCardQuizcontroller] ManageQuiz not found for the Id {Id: 0000}", quizId);
            return NotFound("FlashCardQuiz not found for the FlashCardQuizId");
        }
        return View(quiz);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int quizId)
    {
        var quiz = await _flashCardQuizRepository.GetById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[FlashCardQuizcontroller] ManageQuiz not found for the Id {Id: 0000}", quizId);
            return NotFound("FlashCardQuiz not found for the FlashCardQuizId");
        }
        return View(quiz);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int flashCardQuizId, string name, string description)
    {
        var quiz = await _flashCardQuizRepository.GetById(flashCardQuizId);
        if (quiz == null)
        {
            _logger.LogError("[FlashCardQuizcontroller] ManageQuiz not found for the Id {Id: 0000}", flashCardQuizId);
            return NotFound("FlashCardQuiz not found for the FlashCardQuizId");
        }

        quiz.Name = name;
        quiz.Description = description;

        if (ModelState.IsValid)
        {
            bool returnOk = await _flashCardQuizRepository.Update(quiz);
            if (returnOk)
                return RedirectToAction("ManageQuiz", new { quizId = quiz.FlashCardQuizId });
        }
        _logger.LogError("[FlashCardQuizController] FlashCardQuiz update failed {@quiz}", quiz);
        return RedirectToAction("ManageQuiz", new { quizId = quiz.FlashCardQuizId });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var quiz = await _flashCardQuizRepository.GetById(id);
        if (quiz == null)
        {
            _logger.LogError("[FlashCardQuizController] lashCardQuiz not found for the Id {Id: 0000}", id);
            return BadRequest("FlashCardQuiz not found for the FlashCardQuizId");
        }
        return View(quiz);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool returnOk = await _flashCardQuizRepository.Delete(id);
        if (!returnOk)
        {
            _logger.LogError("[FlashCardQuizController] FlashCardQuiz deletion failed for FlashCardQuizId {Id:0000}", id);
            return BadRequest("FlashCardQuiz deletion failed");
        }
        return RedirectToAction(nameof(Quizzes));
    }
}