using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.ViewModels;
using QuizApp.DAL;
using QuizApp.Services;

namespace QuizApp.Controllers;

public class QuizController : Controller
{
    private readonly IRepository<Quiz> _quizRepository;
    private readonly IAttemptRepository<QuizAttempt> _quizAttemptRepository;
    private readonly QuizService _quizService;
    private readonly ILogger<QuizController> _logger;
    
    public QuizController(IRepository<Quiz> quizRepository,
                          IAttemptRepository<QuizAttempt> quizAttemptRepository,
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
        var quiz = await _quizRepository.GetById(id);
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
        var quiz = await _quizRepository.GetById(id);
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
        var quiz = await _quizRepository.GetById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[QuizController - Get Quiz By Id] Quiz not found for the Id {Id: 0000}", quizId);
            return NotFound("Quiz not found.");
        }

        var quizAttempt = await _quizAttemptRepository.GetById(quizAttemptId);
        if (quizAttempt == null)
        {
            _logger.LogError("[QuizController - PrevQuestion] Quiz attempt not found for the Id {id: 0000}", quizAttemptId);
            return NotFound("QuizAttempt not found");
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

        if (currentQuestionVM is FillInTheBlankViewModel fib)
        {
            var fibAttempt = quizAttempt.FillInTheBlankAttempts
                .FirstOrDefault(a => a.FillInTheBlankId == fib.FillInTheBlankId);
            if (fibAttempt != null)
                fib.UserAnswer = fibAttempt.UserAnswer;
            return View("~/Views/FillInTheBlank/Question.cshtml", model);
        }
        else if (currentQuestionVM is MatchingViewModel m)
        {
            var mAttempt = quizAttempt.MatchingAttempts
                .FirstOrDefault(a => a.MatchingId == m.Id);
            if (mAttempt != null)
                m.UserAnswer = mAttempt.UserAnswer;
            return View("~/Views/Matching/MatchingQuestion.cshtml", model);
        }
        else if (currentQuestionVM is SequenceViewModel sq)
        {
            var sqAttempt = quizAttempt.SequenceAttempts
                .FirstOrDefault(a => a.SequenceId == sq.Id);
            if (sqAttempt != null)
                sq.UserAnswer = sqAttempt.UserAnswer;
            return View("~/Views/Sequence/SequenceQuestion.cshtml", model);
        }
        else if (currentQuestionVM is RankingViewModel r)
        {
            var rAttempt = quizAttempt.RankingAttempts
                .FirstOrDefault(a => a.RankingId == r.Id);
            if (rAttempt != null)
                r.UserAnswer = rAttempt.UserAnswer;
            return View("~/Views/Ranking/RankingQuestion.cshtml", model);
        }
        else if (currentQuestionVM is TrueFalseViewModel tf)
        {
            var tfAttempt = quizAttempt.TrueFalseQuestionAttempts
                .FirstOrDefault(a => a.TrueFalseId == tf.TrueFalseId);
            if (tfAttempt != null)
                tf.UserAnswer = tfAttempt.UserAnswer;
            return View("~/Views/TrueFalse/Question.cshtml", model);
        }
        else if (currentQuestionVM is MultipleChoiceViewModel mc)
        {
            var mcAttempt = quizAttempt.MultipleChoiceAttempts
                .FirstOrDefault(a => a.MultiplechoiceId == mc.MultipleChoiceId);

            if (mcAttempt != null)
                mc.UserAnswer = mcAttempt.UserAnswer;
            return View("~/Views/MultipleChoice/Question.cshtml", model);
        }

        _logger.LogError("[QuizController - NextQuestion] Something went wrong.");
        return RedirectToAction(nameof(Quizzes));
    }


    [HttpPost]
    public async Task<IActionResult> PrevQuestion(int quizId, int quizAttemptId, int quizQuestionNum)
    {
        Console.WriteLine(quizId + ", " + quizQuestionNum);
        var quiz = await _quizRepository.GetById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[QuizController - Get Quiz By Id] Quiz not found for the Id {Id: 0000}", quizId);
            return NotFound("Quiz not found.");
        }

        var quizAttempt = await _quizAttemptRepository.GetById(quizAttemptId);
        if (quizAttempt == null)
        {
            _logger.LogError("[QuizController - PrevQuestion] Quiz attempt not found for the Id {id: 0000}", quizAttemptId);
            return NotFound("QuizAttempt not found");
        }

        var model = new QuizViewModel(quiz, quizAttemptId)
        {
            CurrentQuestionNum = quizQuestionNum
        };

        if (model.CurrentQuestionNum > 0)
        {
            model.CurrentQuestionNum -= 1;
        }

        var currentQuestionVM = model.QuestionViewModels.ElementAt(model.CurrentQuestionNum);

        if (currentQuestionVM is FillInTheBlankViewModel fib)
        {
            var fibAttempt = quizAttempt.FillInTheBlankAttempts
                .FirstOrDefault(a => a.FillInTheBlankId == fib.FillInTheBlankId);
            if (fibAttempt != null)
                fib.UserAnswer = fibAttempt.UserAnswer;
            return View("~/Views/FillInTheBlank/Question.cshtml", model);
        }
        else if (currentQuestionVM is MatchingViewModel m)
        {
            var mAttempt = quizAttempt.MatchingAttempts
                .FirstOrDefault(a => a.MatchingId == m.Id);
            if (mAttempt != null)
                m.UserAnswer = mAttempt.UserAnswer;
            return View("~/Views/Matching/MatchingQuestion.cshtml", model);
        }
        else if (currentQuestionVM is SequenceViewModel sq)
        {
            var sqAttempt = quizAttempt.SequenceAttempts
                .FirstOrDefault(a => a.SequenceId == sq.Id);
            if (sqAttempt != null)
                sq.UserAnswer = sqAttempt.UserAnswer;
            return View("~/Views/Sequence/SequenceQuestion.cshtml", model);
        }
        else if (currentQuestionVM is RankingViewModel r)
        {
            var rAttempt = quizAttempt.RankingAttempts
                .FirstOrDefault(a => a.RankingId == r.Id);
            if (rAttempt != null)
                r.UserAnswer = rAttempt.UserAnswer;
            return View("~/Views/Ranking/RankingQuestion.cshtml", model);
        }
        else if (currentQuestionVM is TrueFalseViewModel tf)
        {
            var tfAttempt = quizAttempt.TrueFalseQuestionAttempts
                .FirstOrDefault(a => a.TrueFalseId == tf.TrueFalseId);
            if (tfAttempt != null)
                tf.UserAnswer = tfAttempt.UserAnswer;
            return View("~/Views/TrueFalse/Question.cshtml", model);
        }
        else if (currentQuestionVM is MultipleChoiceViewModel mc)
        {
            var mcAttempt = quizAttempt.MultipleChoiceAttempts
                .FirstOrDefault(a => a.MultiplechoiceId == mc.MultipleChoiceId);

            if (mcAttempt != null)
                mc.UserAnswer = mcAttempt.UserAnswer;
            return View("~/Views/MultipleChoice/Question.cshtml", model);
        }

        _logger.LogError("[QuizController - PrevQuestion] Something went wrong.");
        return RedirectToAction(nameof(Quizzes));
    }

    [HttpPost]
    public IActionResult RedirectToCreate(string questionType, int quizId, int numOfQuestions)
    {
        Console.WriteLine(questionType);
        if (questionType == "FillInTheBlank")
            return RedirectToAction("Create", questionType, new { quizId, numOfQuestions });
        if (questionType == "TrueFalse")
            return RedirectToAction("Create", questionType, new { quizId, numOfQuestions });
        if (questionType == "MultipleChoice")
            return RedirectToAction("Create", questionType, new { quizId, numOfQuestions });
        if (questionType == "Matching")
            return RedirectToAction("CreateMatchingQuestion", questionType, new { quizId, numOfQuestions });
        if (questionType == "Ranking")
            return RedirectToAction("CreateRankingQuestion", questionType, new { quizId, numOfQuestions });
        if (questionType == "Sequence")
            return RedirectToAction("CreateSequenceQuestion", questionType, new { quizId, numOfQuestions });
        _logger.LogError("[QuizController - RedirectToCreate] Something went wrong.");
        return View("ManageQuiz", quizId);
    }

    [HttpGet]
    public async Task<IActionResult> Results(int quizAttemptId)
    {
        var quizAttempt = await _quizAttemptRepository.GetById(quizAttemptId);
        if (quizAttempt == null)
        {
            _logger.LogError("[QuizController - GetQuizAttemptById] Quiz attempt not found for the Id {Id: 0000}", quizAttemptId);
            return NotFound("Quiz not found.");
        }

        var quiz = await _quizRepository.GetById(quizAttempt.QuizId);
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
                mcAttempt.AnsweredCorrectly = _quizService.CheckAnswer(mc.CorrectAnswer!, mcAttempt.UserAnswer);
            }
        }

        var quizResultViewModel = new QuizResultViewModel(quiz, quizAttempt);

        return View(quizResultViewModel);
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
            bool returnOk = await _quizRepository.Create(quiz);
            if (returnOk)
                return View("ManageQuiz", quiz);
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

        bool returnOk = await _quizAttemptRepository.Create(quizAttempt);
        if (returnOk)
            return quizAttempt.QuizAttemptId;

        _logger.LogError("QuizController] QuizAttempt creation failed {@quizAttempt}", quizAttempt);
        return 0;
    }

    [HttpGet]
    public async Task<IActionResult> ManageQuiz(int quizId)
    {
        var quiz = await _quizRepository.GetById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[QuizController] ManageQuiz not found for the Id {Id: 0000}", quizId);
            return NotFound("Quiz not found for the QuizId");
        }
        return View(quiz);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int quizId)
    {
        var quiz = await _quizRepository.GetById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[Quizcontroller] ManageQuiz not found for the Id {Id: 0000}", quizId);
            return NotFound("Quiz not found for the QuizId");
        }
        return View(quiz);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int quizId, string name, string description)
    {
        var quiz = await _quizRepository.GetById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[Quizcontroller] ManageQuiz not found for the Id {Id: 0000}", quizId);
            return NotFound("Quiz not found for the QuizId");
        }

        quiz.Name = name;
        quiz.Description = description;

        if (ModelState.IsValid)
        {
            bool returnOk = await _quizRepository.Update(quiz);
            if (returnOk)
                return RedirectToAction("ManageQuiz", new { quizId = quiz.QuizId });
        }
        _logger.LogError("[QuizController] Quiz update failed {@quiz}", quiz);
        return RedirectToAction("ManageQuiz", new { quizId = quiz.QuizId });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var quiz = await _quizRepository.GetById(id);
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
        bool returnOk = await _quizRepository.Delete(id);
        if (!returnOk)
        {
            _logger.LogError("[QuizController] Quiz deletion failed for QuizId {Id:0000}", id);
            return BadRequest("Quiz deletion failed");
        }
        return RedirectToAction(nameof(Quizzes));
    }
}