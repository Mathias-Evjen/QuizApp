using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Services;

namespace QuizApp.Controllers;

public class RankingController : Controller
{
    private readonly IRepository<Ranking> _rankingRepository;
    private readonly IAttemptRepository<RankingAttempt> _rankingAttemptRepository;
    private readonly QuizService _quizService;
    private readonly ILogger<RankingController> _logger;

    public RankingController(
        IRepository<Ranking> rankingRepository,
        IAttemptRepository<RankingAttempt> rankingAttemptRepository,
        QuizService quizService,
        ILogger<RankingController> logger)
    {
        _rankingRepository = rankingRepository;
        _rankingAttemptRepository = rankingAttemptRepository;
        _quizService = quizService;
        _logger = logger;
    }

    public async Task<IActionResult> RankingQuestion()
    {
        var ranking = await _rankingRepository.GetAll();
        if (ranking == null)
        {
            _logger.LogError("[RankingController] Questions list not found while executing _rankingRepository.GetAll()");
            return NotFound("Questions not found");
        }

        var viewModel = new RankingViewModel(ranking.ElementAt(0));

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitRankingQuestion(int id, List<string> values, int quizId, int quizQuestionNum, int quizAttemptId, int numOfQuestions)
    {
        var rankingObject = await _rankingRepository.GetById(id);
        if (rankingObject == null)
        {
            _logger.LogError("[RankingController - Get Question] Ranking question not found for the Id {Id: 0000}", id);
            return NotFound("Ranking question not found.");
        }

        string answer = rankingObject.Assemble(values, 2);

        if (!CheckAttempt(quizAttemptId))
        {
            var rankingAttempt = new RankingAttempt();
            rankingAttempt.RankingId = rankingObject.Id;
            rankingAttempt.QuizAttemptId = quizAttemptId;
            rankingAttempt.UserAnswer = answer;
            if (answer == rankingObject.CorrectAnswer) { rankingAttempt.AnsweredCorrectly = true; }
            else{ rankingAttempt.AnsweredCorrectly = false; }

            var returnOk = await _rankingAttemptRepository.Create(rankingAttempt);
            if (!returnOk)
            {
                _logger.LogError("[RankingController] Question attempt creation failed {@attempt}", rankingAttempt);
                return RedirectToAction("Quizzes", "Quiz");
            }
        }
        else
        {
            var rankingAttempt = await _rankingAttemptRepository.GetById(quizAttemptId);
            if (rankingAttempt == null)
            {
                _logger.LogError("[RankingController - Get Attempt] Ranking attempt not found for the Id {Id: 0000}", id);
                return NotFound("Ranking attempt not found.");
            }
            rankingAttempt.RankingId = rankingObject.Id;
            rankingAttempt.QuizAttemptId = quizAttemptId;
            rankingAttempt.UserAnswer = answer;
            if (answer == rankingObject.CorrectAnswer) { rankingAttempt.AnsweredCorrectly = true; }
            else{ rankingAttempt.AnsweredCorrectly = false; }

            var returnOk = await _rankingAttemptRepository.Create(rankingAttempt);
            if (!returnOk)
            {
                _logger.LogError("[RankingController] Question attempt creation failed {@attempt}", rankingAttempt);
                return RedirectToAction("Quizzes", "Quiz");
            }
        }
        if (rankingObject.QuizQuestionNum == numOfQuestions)
            return RedirectToAction("Results", "Quiz", new { quizAttemptId = quizAttemptId });
            
        return RedirectToAction("NextQuestion", "Quiz", new
        {
            quizId = quizId,
            quizAttemptId = quizAttemptId,
            quizQuestionNum = quizQuestionNum
        });
    }

    public bool CheckAttempt(int quizAttemptId)
    {
        if(quizAttemptId <= 0){ return false; }
        var attempt =  _rankingAttemptRepository.Exists(quizAttemptId);
        if (attempt)
        {
            return true;
        }
        else
        {
            return false; 
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateRankingQuestion(string questionText, List<string> Values)
    {
        if (Values == null)
        {
            ModelState.AddModelError("", "Values can not be empty.");
            return View();
        }
        var rankingQuestion = new Ranking
        {
            QuestionText = questionText
        };
        rankingQuestion.Assemble(Values, 1);
        rankingQuestion.ShuffleQuestion(Values);
        await _rankingRepository.Create(rankingQuestion);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult CreateRankingQuestion(int id)
    {
        return View();
    }

    // [HttpGet]
    // public async Task<IActionResult> ShowRankings()
    // {
    //     var rankings = await _rankingRepository.GetAll();

    //     var viewModel = new RankingViewModel(rankings);
    //     return View(viewModel);
    // }

    public async Task<IActionResult> UpdateRankingPage(int id)
    {
        var ranking = await _rankingRepository.GetById(id);
        return View(ranking);
    }

    [HttpPost]
    public IActionResult UpdateRanking(int id, string questionText, List<string> question, List<string> correctAnswer)
    {
        Ranking updatetRanking = new Ranking
        {
            Id = id,
            QuestionText = questionText
        };
        updatetRanking.Assemble(question, 3);
        updatetRanking.Assemble(correctAnswer, 1);

        _rankingRepository.Update(updatetRanking);
        return RedirectToAction("ShowRankings");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var question = await _rankingRepository.GetById(id);
        if (question == null)
        {
            _logger.LogError("[RankingController] Question deletion failed for the QuestionId {QuestionId:0000}", id);
            return BadRequest("Question not found for the QuestionId");
        }
        return View(question);
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int questionId, int qNum, int quizId)
    {
        bool returnOk = await _rankingRepository.Delete(questionId);
        if (!returnOk)
        {
            _logger.LogError("[RankingController] Question deletion failed for QuestionId {QuestionId:0000}", questionId);
            return BadRequest("Question deletion failed");
        }
        await _quizService.ChangeQuestionCount(quizId, false);
        await _quizService.UpdateQuestionNumbers(qNum, quizId);
        return RedirectToAction("ManageQuiz", "Quiz", new { quizId });
    }
}