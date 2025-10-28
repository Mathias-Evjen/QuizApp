using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.ViewModels;
using QuizApp.Services;

namespace QuizApp.Controllers;

public class FillInTheBlankController : Controller
{
    private readonly IQuestionRepository<FillInTheBlank> _fillInTheBlankRepository;
    private readonly IAttemptRepository<FillInTheBlankAttempt> _fillInTheBlankAttemptRepository;
    private readonly QuizService _quizService;
    private readonly ILogger<FillInTheBlankController> _logger;

    public FillInTheBlankController(IQuestionRepository<FillInTheBlank> fillInTheBlankRepository, IAttemptRepository<FillInTheBlankAttempt> fillInTheBlankAttemptRepository, QuizService quizService, ILogger<FillInTheBlankController> logger)
    {
        _fillInTheBlankRepository = fillInTheBlankRepository;
        _fillInTheBlankAttemptRepository = fillInTheBlankAttemptRepository;
        _quizService = quizService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Question(QuizViewModel model)
    {      
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitQuestion(int quizId, int quizAttemptId, int quizQuestionId, int quizQuestionNum, int numOfQuestions, string userAnswer)
    {
        Console.WriteLine(quizAttemptId);
        var fillInTheBlank = await _fillInTheBlankRepository.GetById(quizQuestionId);
        if (fillInTheBlank == null)
        {
            _logger.LogError("[FillInTheBlankController - Submit question] FillInTheBlank question not found for the Id {Id: 0000}", quizQuestionId);
            return NotFound("FillInTheBlank question not found.");
        }

        if (userAnswer != null)
        {
            _logger.LogError("Skal ikke komme hit");
            var fillInTheBlankAttempt = new FillInTheBlankAttempt
            {
                FillInTheBlankId = fillInTheBlank.FillInTheBlankId,
                QuizAttemptId = quizAttemptId,
                UserAnswer = userAnswer
            };

            var returnOk = await _fillInTheBlankAttemptRepository.Create(fillInTheBlankAttempt);
            if (!returnOk)
            {
                _logger.LogError("[FillInTheBlankController] Question attempt creation failed {@attempt}", fillInTheBlankAttempt);
                return RedirectToAction("Quizzes", "Quiz");
            }
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

    [HttpGet]
    public IActionResult Create(int quizId, int numOfQuestions)
    {
        var question = new FillInTheBlank
        {
            QuizId = quizId,
            QuizQuestionNum = numOfQuestions + 1
        };
        return View(question);
    }

    [HttpPost]
    public async Task<IActionResult> Create(FillInTheBlank fillQuestion)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _fillInTheBlankRepository.Create(fillQuestion);
            if (returnOk)
                await _quizService.ChangeQuestionCount(fillQuestion.QuizId, true);
                return RedirectToAction("ManageQuiz", "Quiz", new { quizId = fillQuestion.QuizId});
        }
        _logger.LogError("[FillInTheBlankController] Question creation failed {@question}", fillQuestion);
        return View(fillQuestion);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var question = await _fillInTheBlankRepository.GetById(id);
        if (question == null)
        {
            _logger.LogError("[FillInTheBlankController] Question not found when updating QuestionId {QuestionId: 0000}", id);
            return BadRequest("Question not found for the QuestionId");
        }
        return View(question);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(FillInTheBlank fillQuestion)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _fillInTheBlankRepository.Update(fillQuestion);
            if (returnOk)
                return RedirectToAction("ManageQuiz", "Quiz", new { quizId = fillQuestion.QuizId });
        }
        _logger.LogError("[FillInTheBlankController] Question update failed {@question}", fillQuestion);
        return View(fillQuestion);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var question = await _fillInTheBlankRepository.GetById(id);
        if (question == null)
        {
            _logger.LogError("[FillInTheBlankController] Question deletion failed for the QuestionId {QuestionId:0000}", id);
            return BadRequest("Question not found for the QuestionId");
        }
        return View(question);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int questionId, int qNum, int quizId)
    {
        bool returnOk = await _fillInTheBlankRepository.Delete(questionId);
        if (!returnOk)
        {
            _logger.LogError("[FillInTheBlankController] Question deletion failed for QuestionId {QuestionId:0000}", questionId);
            return BadRequest("Question deletion failed");
        }
        await _quizService.ChangeQuestionCount(quizId, false);
        await _quizService.UpdateQuestionNumbers(qNum, quizId);
        return RedirectToAction("ManageQuiz", "Quiz", new { quizId });
    }
}