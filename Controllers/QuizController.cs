using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.ViewModels;
using QuizApp.DAL;
using Serilog;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using QuizApp.Services;

namespace QuizApp.Controllers;

public class QuizController : Controller
{
    private readonly IQuizRepository _quizRepository;
    private readonly IQuizAttemptRepository _quizAttemptRepository;
    private readonly QuizService _quizService;
    private readonly ILogger<QuizController> _logger;
    
    public QuizController(IQuizRepository quizRepository,
                          IQuizAttemptRepository quizAttemptRepository,
                          QuizService quizService,
                          ILogger<QuizController> logger)
    {
        _quizRepository = quizRepository;
        _quizAttemptRepository = quizAttemptRepository;
        _quizService = quizService;
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

        var currentQuestion = quizViewModel.QuestionViewModels.ElementAt(quizViewModel.CurrentQuestionNum);
        if (currentQuestion is FillInTheBlankViewModel)
        {
            return View("~/Views/FillInTheBlank/Question.cshtml", quizViewModel);
        }
        else if (currentQuestion is MatchingViewModel)
        {
            return View("~/Views/Matching/MatchingQuestion.cshtml", quizViewModel);
        }
        else if (currentQuestion is SequenceViewModel)
        {
            return View("~/Views/Sequence/SequenceQuestion.cshtml", quizViewModel);
        }
        else if (currentQuestion is RankingViewModel)
        {
            return View("~/Views/Ranking/RankingQuestion.cshtml", quizViewModel);
        }
        else if (currentQuestion is TrueFalseViewModel)
        {
            return View("~/Views/TrueFalse/Question.cshtml", quizViewModel);
        }
        else if (currentQuestion is MultipleChoiceViewModel)
        {
            return View("~/Views/MultipleChoice/Question.cshtml", quizViewModel);
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

        var model = new QuizViewModel(quiz, quizAttemptId)
        {
            CurrentQuestionNum = quizQuestionNum
        };

        if (model.CurrentQuestionNum + 1 < model.QuestionViewModels.Count())
        {
            model.CurrentQuestionNum += 1;
        }

        var currentQuestionVM = model.QuestionViewModels.ElementAt(model.CurrentQuestionNum);
        if (currentQuestionVM is FillInTheBlankViewModel)
        {
            return View("~/Views/FillInTheBlank/Question.cshtml", model);
        }
        else if (currentQuestionVM is MatchingViewModel)
        {
            return View("~/Views/Matching/MatchingQuestion.cshtml", model);
        }
        else if (currentQuestionVM is SequenceViewModel)
        {
            return View("~/Views/Sequence/SequenceQuestion.cshtml", model);
        }
        else if (currentQuestionVM is RankingViewModel)
        {
            return View("~/Views/Ranking/RankingQuestion.cshtml", model);
        }
        else if (currentQuestionVM is TrueFalseViewModel)
        {
            return View("~/Views/TrueFalse/Question.cshtml", model);
        }
        else if (currentQuestionVM is MultipleChoiceViewModel)
        {
            return View("~/Views/MultipleChoice/Question.cshtml", model);
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

    [HttpGet]
    public async Task<IActionResult> Results(int quizAttemptId)
    {
        var quizAttempt = await _quizAttemptRepository.GetQuizAttemptById(quizAttemptId);
        if (quizAttempt == null)
        {
            _logger.LogError("[QuizController - GetQuizAttemptById] Quiz attempt not found for the Id {Id: 0000}", quizAttemptId);
            return NotFound("Quiz not found.");
        }

        var quiz = await _quizRepository.GetQuizById(quizAttempt.QuizId);
        if (quiz == null)
        {
            _logger.LogError("[QuizController - Get Quiz By Id] Quiz not found for the Id {Id: 0000}", quizAttempt.QuizId);
            return NotFound("Quiz not found.");
        }

        for (int i = 0; i < quiz.AllQuestions.Count(); i++)
        {
            var question = quiz.AllQuestions.ElementAt(i);
            var questionAttempt = quizAttempt.AllQuestionAttempts.ElementAt(i);
            if (question is FillInTheBlank fib && questionAttempt is FillInTheBlankAttempt fibAttempt)
            {
                fibAttempt.AnsweredCorrectly = _quizService.CheckAnswer(fib.CorrectAnswer, fibAttempt.UserAnswer);
            }
            if (question is TrueFalse tf && questionAttempt is TrueFalseAttempt tfAttempt)
            {
                tfAttempt.AnsweredCorrectly = _quizService.CheckAnswer(tf.CorrectAnswer, tfAttempt.UserAnswer);
            }
            if (question is MultipleChoice mc && questionAttempt is MultipleChoiceAttempt mcAttempt)
            {
                mcAttempt.AnsweredCorrectly = _quizService.CheckAnswer(mc.CorrectAnswer, mcAttempt.UserAnswer);   
            }
        }

        var quizResultViewModel = new QuizResultViewModel(quiz, quizAttempt);

        return View(quizResultViewModel);
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
        var quizAttempt = new QuizAttempt
        {
            QuizId = quiz.QuizId
        };

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