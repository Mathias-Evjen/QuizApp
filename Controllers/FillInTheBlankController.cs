using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.ViewModels;
using QuizApp.Services;

namespace QuizApp.Controllers;

public class FillInTheBlankController : Controller
{
    private readonly IFillInTheBlankRepository _fillInTheBlankRepository;
    private readonly QuizService _quizService;
    private readonly ILogger<FillInTheBlankController> _logger;

    public FillInTheBlankController(IFillInTheBlankRepository fillInTheBlankRepository, QuizService quizService, ILogger<FillInTheBlankController> logger)
    {
        _fillInTheBlankRepository = fillInTheBlankRepository;
        _quizService = quizService;
        _logger = logger;
    }

    // public async Task

    [HttpGet]
    public async Task<IActionResult> Question(int id)
    {
        var fillInTheBlank = await _fillInTheBlankRepository.GetQuestionById(id);
        if (fillInTheBlank == null)
        {
            _logger.LogError("[FillInTheBlankController - Get Question] FillInTheBlank question not found for the Id {Id: 0000}", id);
            return NotFound("FillInTheBlank question not found.");
        }

        var fillInTheBlankViewModel = new FillInTheBlankViewModel(fillInTheBlank.Id, fillInTheBlank.Question);

        return View(fillInTheBlankViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Question(FillInTheBlankViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var questionFromDb = await _fillInTheBlankRepository.GetQuestionById(model.Id);
        if (questionFromDb == null)
        {
            _logger.LogError("[FillInTheBlankController - Post Question] FillInTheBlank question not found for the Id {Id: 0000}", model.Id);
            return NotFound("FillInTheBlank question not found.");
        }

        model.Question = questionFromDb.Question;

        model.IsAnswerCorrect = await _quizService.CheckAnswer(questionFromDb, model.UserAnswer);
        return View(model);
    
    }
}