using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace QuizApp.Controllers;

public class RankingController : Controller
{
    private readonly IRankingRepository _rankingRepository;

    private readonly ILogger<RankingController> _logger;

    public RankingController(IRankingRepository rankingRepository, ILogger<RankingController> logger)
    {
        _rankingRepository = rankingRepository;
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
    public async Task<IActionResult> SubmitRankingQuestion(int id, List<string> values, int quizId, int quizQuestionNum)
    {
        var rankingObject = await _rankingRepository.GetRankingById(id);
        if (rankingObject == null)
        {
            _logger.LogError("[RankingController - Get Question] Ranking question not found for the Id {Id: 0000}", id);
            return NotFound("Ranking question not found.");
        }

        string answer = rankingObject.Assemble(values, 2);
        Console.WriteLine("Answer: " + answer);
        if (answer == rankingObject.CorrectAnswer)
        {
            Console.WriteLine("Answer is correct!");
        }
        else
        {
            Console.WriteLine($"Answer is wrong, correct answer: {rankingObject.CorrectAnswer}");
        }
        await _rankingRepository.UpdateRanking(rankingObject);
        return RedirectToAction("NextQuestion", "Quiz", new
        {
            quizId = quizId,
            quizQuestionNum = quizQuestionNum
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateRankingQuestion(string questionText, List<string> Values)
    {
        if (Values == null)
        {
            ModelState.AddModelError("", "Values can not be empty.");
            return View();
        }
        var rankingQuestion = new Ranking();
        rankingQuestion.QuestionText = questionText;
        rankingQuestion.Assemble(Values, 1);
        rankingQuestion.ShuffleQuestion(Values);
        await _rankingRepository.CreateRanking(rankingQuestion);

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
        var ranking = await _rankingRepository.GetRankingById(id);
        return View(ranking);
    }

    [HttpPost]
    public IActionResult UpdateRanking(int id, string questionText, List<string> question, List<string> correctAnswer)
    {
        Ranking updatetRanking = new Ranking();
        updatetRanking.Id = id;
        updatetRanking.QuestionText = questionText;
        updatetRanking.Assemble(question, 3);
        updatetRanking.Assemble(correctAnswer, 1);

        _rankingRepository.UpdateRanking(updatetRanking);
        return RedirectToAction("ShowRankings");
    }
    
    public IActionResult DeleteRanking(int id)
    {
        _rankingRepository.DeleteRanking(id);
        return RedirectToAction("ShowRankings");
    }
}