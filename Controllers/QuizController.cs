using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.ViewModels;
using QuizApp.DAL;
using Serilog;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace QuizApp.Controllers;

public class QuizController : Controller
{
    private readonly IQuizRepository _quizRepository;
    private readonly IQuizAttemptRepository _quizAttemptRepository;
    private readonly ILogger<QuizController> _logger;
    
    public QuizController(IQuizRepository quizRepository, IQuizAttemptRepository quizAttemptRepository, ILogger<QuizController> logger)
    {
        _quizRepository = quizRepository;
        _quizAttemptRepository = quizAttemptRepository;
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
    public async Task<IActionResult> OpenQuiz(int id)
    {
        Console.WriteLine(id);
        var quiz = await _quizRepository.GetQuizById(id);
        if (quiz == null)
        {
            _logger.LogError("[QuizController - Get Quiz By Id] Quiz not found for the Id {Id: 0000}", id);
            return NotFound("Quiz not found.");
        }

        var quizAttemptId = await CreateAttempt(quiz);
        if (quizAttemptId == 0)
            return RedirectToAction(nameof(Quizzes));

        var quizViewModel = new QuizViewModel(quiz, quizAttemptId);

        Console.WriteLine(quizViewModel.QuizId);
        if (quizViewModel.QuestionViewModels.ElementAt(quizViewModel.CurrentQuestionNum) is FillInTheBlankViewModel)
        {
            return View("~/Views/FillInTheBlank/Question.cshtml", quizViewModel);
        }
        else if (quizViewModel.QuestionViewModels.ElementAt(quizViewModel.CurrentQuestionNum) is MatchingViewModel)
        {
            return View("~/Views/Matching/MatchingQuestion.cshtml", quizViewModel);
        }
        else if (quizViewModel.QuestionViewModels.ElementAt(quizViewModel.CurrentQuestionNum) is SequenceViewModel)
        {
            return View("~/Views/Sequence/SequenceQuestion.cshtml", quizViewModel);
        }
        else if (quizViewModel.QuestionViewModels.ElementAt(quizViewModel.CurrentQuestionNum) is RankingViewModel)
        {
            return View("~/Views/Ranking/RankingQuestion.cshtml", quizViewModel);
        }
        else if (quizViewModel.QuestionViewModels.ElementAt(quizViewModel.CurrentQuestionNum) is TrueFalseViewModel)
        {
            return View("~/Views/TrueFalse/Question.cshtml", quizViewModel);
        }
        
        return RedirectToAction(nameof(Quizzes));
    }

    [HttpGet]
    public async Task<IActionResult> NextQuestion(int quizId, int quizAttemptId, int quizQuestionNum)
    {
        var quiz = await _quizRepository.GetQuizById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[QuizController - Get Quiz By Id] Quiz not found for the Id {Id: 0000}", quizId);
            return NotFound("Quiz not found.");
        }

        var model = new QuizViewModel(quiz, quizAttemptId);
        model.CurrentQuestionNum = quizQuestionNum;

        if (model.CurrentQuestionNum + 1 < model.QuestionViewModels.Count())
        {
            model.CurrentQuestionNum += 1;
        }
        if (model.QuestionViewModels.ElementAt(model.CurrentQuestionNum) is FillInTheBlankViewModel)
        {
            return View("~/Views/FillInTheBlank/Question.cshtml", model);
        }
        else if (model.QuestionViewModels.ElementAt(model.CurrentQuestionNum) is MatchingViewModel)
        {
            return View("~/Views/Matching/MatchingQuestion.cshtml", model);
        }
        else if (model.QuestionViewModels.ElementAt(model.CurrentQuestionNum) is SequenceViewModel)
        {
            return View("~/Views/Sequence/SequenceQuestion.cshtml", model);
        }
        else if (model.QuestionViewModels.ElementAt(model.CurrentQuestionNum) is RankingViewModel)
        {
            return View("~/Views/Ranking/RankingQuestion.cshtml", model);
        }
        else if (model.QuestionViewModels.ElementAt(model.CurrentQuestionNum) is TrueFalseViewModel)
        {
            return View("~/Views/TrueFalse/Question.cshtml", model);
        }

        return View("FlashCards", model);
    }


    [HttpPost]
    public IActionResult PrevQuestion(QuizViewModel model)
    {
        if (model.CurrentQuestionNum - 1 >= 0)
            model.CurrentQuestionNum -= 1;
        return View("FlashCards", model);
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
        _logger.LogError("[QuizController] Quiz creation failed {@quiz}", quiz);
        return View(quiz);
    }

    public async Task<int> CreateAttempt(Quiz quiz)
    {
        var quizAttempt = new QuizAttempt();
        quizAttempt.QuizId = quiz.QuizId;

        bool returnOk = await _quizAttemptRepository.CreateQuizAttempt(quizAttempt);
        if (returnOk)
            return quizAttempt.QuizAttemptId;

        _logger.LogError("QuizController] QuizAttempt creation failed {@quizAttempt}", quizAttempt);
        return 0;
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