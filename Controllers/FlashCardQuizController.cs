using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.ViewModels;
using QuizApp.DAL;
using Serilog;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace QuizApp.Controllers;

public class FlashCardQuizController : Controller
{
    private readonly IFlashCardQuizRepository _flashCardQuizRepository;
    private readonly ILogger<FlashCardQuizController> _logger;
    
    public FlashCardQuizController(IFlashCardQuizRepository flashCardQuizRepository, ILogger<FlashCardQuizController> logger)
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
        var quiz = await _flashCardQuizRepository.GetFlashCardQuizById(id);
        if (quiz == null)
        {
            _logger.LogError("[FlashCardQuizcontroller] FlashCardQuiz not found for the Id {Id: 0000}", id);
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
            bool returnOk = await _flashCardQuizRepository.CreateFlashCardQuiz(quiz);
            if (returnOk)
                return RedirectToAction(nameof(Quizzes));
        }
        _logger.LogError("[FlashCardQuizController] FlashCardQuiz creation failed {@question}", quiz);
        return View(quiz);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var quiz = await _flashCardQuizRepository.GetFlashCardQuizById(id);
        if (quiz == null)
        {
            _logger.LogError("[FlashCardQuizcontroller] FlashCardQuiz not found for the Id {Id: 0000}", id);
            return NotFound("FlashCardQuiz not found for the FlashCardQuizId");
        }
        return View(quiz);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(FlashCardQuiz quiz)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _flashCardQuizRepository.UpdateFlashCardQuiz(quiz);
            if (returnOk)
                return RedirectToAction(nameof(Quizzes));
        }
        _logger.LogError("[FlashCardQuizController] FlashCardQuiz update failed {@quiz}", quiz);
        return View(quiz);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var quiz = await _flashCardQuizRepository.GetFlashCardQuizById(id);
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
        bool returnOk = await _flashCardQuizRepository.DeleteFlashCardQuiz(id);
        if (!returnOk)
        {
            _logger.LogError("[FlashCardQuizController] FlashCardQuiz deletion failed for FlashCardQuizId {Id:0000}", id);
            return BadRequest("FlashCardQuiz deletion failed");
        }
        return RedirectToAction(nameof(Quizzes));
    }
}