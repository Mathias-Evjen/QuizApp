using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.ViewModels;
using QuizApp.DAL;
using Serilog;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace QuizApp.Controllers;

public class QuizController : Controller
{
    private readonly IQuizRepository _quizRepository;
    private readonly ILogger<QuizController> _logger;
    
    public QuizController(IQuizRepository quizRepository, ILogger<QuizController> logger)
    {
        _quizRepository = quizRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Quizzes()
    {
        var quizzes = await _quizRepository.GetAll();
        if (quizzes == null)
        {
            _logger.LogError("[Quizcontroller] Quizzes list not found while executing _quizRepository.GetAll()");
            return NotFound("Quizzes not found");
        }
        return View(quizzes);
    }

    [HttpGet]
    public async Task<IActionResult> Quiz(int id) 
    {
        var quiz = await _quizRepository.GetQuizById(id);
        if (quiz == null)
        {
            _logger.LogError("[QuizController] Quiz not found for the Id {Id: 0000}", id);
            return NotFound("Quiz not found");
        }
        return View(quiz);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Quiz quiz)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _quizRepository.CreateQuiz(quiz);
            if (returnOk)
                return RedirectToAction(nameof(Quizzes));
        }
        _logger.LogError("[QuizController] Quiz creation failed {@question}", quiz);
        return View(quiz);
    }

    [HttpGet]
    public async Task<IActionResult> ManageQuiz(int quizId)
    {
        var quiz = await _quizRepository.GetQuizById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[Quizcontroller] ManageQuiz not found for the Id {Id: 0000}", quizId);
            return NotFound("Quiz not found for the QuizId");
        }
        return View(quiz);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int quizId)
    {
        var quiz = await _quizRepository.GetQuizById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[Quizcontroller] ManageQuiz not found for the Id {Id: 0000}", quizId);
            return NotFound("Quiz not found for the QuizId");
        }
        return View(quiz);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Quiz quiz)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _quizRepository.UpdateQuiz(quiz);
            if (returnOk)
                return RedirectToAction("ManageQuiz", new { quizId = quiz.QuizId });
        }
        _logger.LogError("[QuizController] Quiz update failed {@quiz}", quiz);
        return RedirectToAction("ManageQuiz", new { quizId = quiz.QuizId });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var quiz = await _quizRepository.GetQuizById(id);
        if (quiz == null)
        {
            _logger.LogError("[QuizController] lashCardQuiz not found for the Id {Id: 0000}", id);
            return BadRequest("Quiz not found for the QuizId");
        }
        return View(quiz);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool returnOk = await _quizRepository.DeleteQuiz(id);
        if (!returnOk)
        {
            _logger.LogError("[QuizController] Quiz deletion failed for QuizId {Id:0000}", id);
            return BadRequest("Quiz deletion failed");
        }
        return RedirectToAction(nameof(Quizzes));
    }
}