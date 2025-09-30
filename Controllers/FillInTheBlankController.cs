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

    public async Task<IActionResult> Questions()
    {
        var questions = await _fillInTheBlankRepository.GetAll();
        if (questions == null)
        {
            _logger.LogError("[FillInTheBlankController] Questions list not found while executing _fillInTheBlankRepository.GetAll()");
            return NotFound("Questions not found");
        }
        var questionsViewModelList = new QuestionsViewModel(questions);
        return View(questionsViewModelList);
    }

    [HttpGet]
    public async Task<IActionResult> Question(int id)
    {   
        // Ge the question from the databse
        var question = await _fillInTheBlankRepository.GetQuestionById(id);
        if (question == null)
        {
            _logger.LogError("[FillInTheBlankController - Get Question] FillInTheBlank question not found for the Id {Id: 0000}", id);
            return NotFound("FillInTheBlank question not found.");
        }

        // Map question to viewmodel without answer
        var fillInTheBlankViewModel = new FillInTheBlankViewModel(question.Id, question.Question);
        return View(fillInTheBlankViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Question(FillInTheBlankViewModel model)
    {
        if (!ModelState.IsValid) return View(model); // Check if the model is correct

        // Get the question from the database
        var questionFromDb = await _fillInTheBlankRepository.GetQuestionById(model.Id);
        if (questionFromDb == null)
        {
            _logger.LogError("[FillInTheBlankController - Post Question] FillInTheBlank question not found for the Id {Id: 0000}", model.Id);
            return NotFound("FillInTheBlank question not found.");
        }

        // Set the answer to correct or false in the model
        model.IsAnswerCorrect = _quizService.CheckAnswer(questionFromDb, model.UserAnswer);
        return View(model);
    
    }
}