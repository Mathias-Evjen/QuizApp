using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.ViewModels;
using QuizApp.Services;

namespace QuizApp.Controllers;

public class FillInTheBlankController : Controller
{
    private readonly IFillInTheBlankRepository _fillInTheBlankRepository;
    private readonly IFillInTheBlankAttemptRepository _fillInTheBlankAttemptRepository;
    private readonly QuizService _quizService;
    private readonly ILogger<FillInTheBlankController> _logger;

    public FillInTheBlankController(IFillInTheBlankRepository fillInTheBlankRepository, IFillInTheBlankAttemptRepository fillInTheBlankAttemptRepository, QuizService quizService, ILogger<FillInTheBlankController> logger)
    {
        _fillInTheBlankRepository = fillInTheBlankRepository;
        _fillInTheBlankAttemptRepository = fillInTheBlankAttemptRepository;
        _quizService = quizService;
        _logger = logger;
    }

    // [HttpGet]
    // public async Task<IActionResult> Questions()
    // {
    //     var questions = await _fillInTheBlankRepository.GetAll();
    //     if (questions == null)
    //     {
    //         _logger.LogError("[FillInTheBlankController] Questions list not found while executing _fillInTheBlankRepository.GetAll()");
    //         return NotFound("Questions not found");
    //     }
    //     var questionsViewModelList = new QuestionsViewModel(questions);
    //     return View(questionsViewModelList);
    // }

    // [HttpPost]
    // public async Task<IActionResult> Questions(QuestionsViewModel model)
    // {
    //     foreach (var question in model.Questions)
    //     {
    //         if (!ModelState.IsValid) return View(model);   // Check if the model state is valid

    //         var questionFromDb = await _fillInTheBlankRepository.GetQuestionById(question.FillInTheBlankId);
    //         if (questionFromDb == null)
    //         {
    //             _logger.LogError("[FillInTheBlankController - Post Question] FillInTheBlank question not found for the Id {Id: 0000}", question.FillInTheBlankId);
    //             return NotFound("FillInTheBlank question not found.");
    //         }

    //         question.IsAnswerCorrect = _quizService.CheckAnswer(questionFromDb.Answer, question.UserAnswer);
    //         question.CorrectAnswer = questionFromDb.Answer;
    //     }
    //     return View(model);
    // }

    [HttpGet]
    public IActionResult Question(QuizViewModel model)
    {      
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Question(FillInTheBlankViewModel model)
    {
        if (!ModelState.IsValid) return View(model); // Check if the model is correct

        // Get the question from the database
        var questionFromDb = await _fillInTheBlankRepository.GetQuestionById(model.FillInTheBlankId);
        if (questionFromDb == null)
        {
            _logger.LogError("[FillInTheBlankController - Post Question] FillInTheBlank question not found for the Id {Id: 0000}", model.FillInTheBlankId);
            return NotFound("FillInTheBlank question not found.");
        }

        // Set the answer to correct or false in the model
        model.IsAnswerCorrect = _quizService.CheckAnswer(questionFromDb.CorrectAnswer, model.UserAnswer);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitQuestion(int quizId, int quizAttemptId, int quizQuestionId, int quizQuestionNum, int numOfQuestions, string userAnswer)
    {
        var fillInTheBlank = await _fillInTheBlankRepository.GetQuestionById(quizQuestionId);
        if (fillInTheBlank == null)
        {
            _logger.LogError("[FillInTheBlankController - Submit question] FillInTheBlank question not found for the Id {Id: 0000}", quizQuestionId);
            return NotFound("FillInTheBlank question not found.");
        }

        var fillInTheBlankAttempt = new FillInTheBlankAttempt
        {
            FillInTheBlankId = fillInTheBlank.FillInTheBlankId,
            QuizAttemptId = quizAttemptId,
            UserAnswer = userAnswer
        };

        var returnOk = await _fillInTheBlankAttemptRepository.CreateFillInTheBlankAttempt(fillInTheBlankAttempt);
        if (!returnOk)
        {
            _logger.LogError("[FillInTheBlankController] Question attempt creation failed {@attempt}", fillInTheBlankAttempt);
            return RedirectToAction("Quizzes", "Quiz");
        }

        if (fillInTheBlank.QuizQuestionNum == numOfQuestions)
            return RedirectToAction("Results", "Quiz", new { quizAttemptId = quizAttemptId });

        return RedirectToAction("NextQuestion", "Quiz", new
        {
            quizId = quizId,
            quizAttemptId = quizAttemptId,
            quizQuestionNum = quizQuestionNum
        });
    }

    // [HttpGet]
    // public IActionResult Create()
    // {
    //     return View();
    // }

    // [HttpPost]
    // public async Task<IActionResult> Create(FillInTheBlank fillQuestion)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         bool returnOk = await _fillInTheBlankRepository.CreateQuestion(fillQuestion);
    //         if (returnOk)
    //             return RedirectToAction(nameof(Questions));
    //     }
    //     _logger.LogError("[FillInTheBlankController] Question creation failed {@question}", fillQuestion);
    //     return View(fillQuestion);
    // }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var question = await _fillInTheBlankRepository.GetQuestionById(id);
        if (question == null)
        {
            _logger.LogError("[FillInTheBlankController] Question not found when updating QuestionId {QuestionId: 0000}", id);
            return BadRequest("Question not found for the QuestionId");
        }
        return View(question);
    }

    // [HttpPost]
    // public async Task<IActionResult> Edit(FillInTheBlank fillQuestion)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         bool returnOk = await _fillInTheBlankRepository.UpdateQuestion(fillQuestion);
    //         if (returnOk)
    //             return RedirectToAction(nameof(Questions));
    //     }
    //     _logger.LogError("[FillInTheBlankController] Question update failed {@question}", fillQuestion);
    //     return View(fillQuestion);
    // }

    // [HttpGet]
    // public async Task<IActionResult> Delete(int id)
    // {
    //     var question = await _fillInTheBlankRepository.GetQuestionById(id);
    //     if (question == null)
    //     {
    //         _logger.LogError("[FillInTheBlankController] Question deletion failed for the QuestionId {QuestionId:0000}", id);
    //         return BadRequest("Question not found for the QuestionId");
    //     }
    //     return View(question);
    // }

//     [HttpPost]
//     public async Task<IActionResult> DeleteConfirmed(int id)
//     {
//         bool returnOk = await _fillInTheBlankRepository.DeleteQuestion(id);
//         if (!returnOk)
//         {
//             _logger.LogError("[FillInTheBlankController] Question deletion failed for QuestionId {QuestionId:0000}", id);
//             return BadRequest("Question deletion failed");
//         }
//         return RedirectToAction(nameof(Questions));
//     }
}