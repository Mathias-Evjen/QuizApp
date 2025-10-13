using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.ViewModels;

namespace QuizApp.Controllers;

public class MatchingController : Controller
{

    private readonly IMatchingRepository _matchingRepository;

    private readonly ILogger<MatchingController> _logger;

    public MatchingController(IMatchingRepository matchingRepository, ILogger<MatchingController> logger)
    {
        _matchingRepository = matchingRepository;
        _logger = logger;
    }

    public async Task<IActionResult> MatchingQuestion()
    {
        var matching = await _matchingRepository.GetAll();
        if (matching == null)
        {
            _logger.LogError("[MatchingController] Questions list not found while executing _matchingRepository.GetAll()");
            return NotFound("Matching questions not found");
        }

        var viewModel = new MatchingViewModel(matching.ElementAt(0));

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitMatchingQuestion(int id, List<string> keys, List<string> values)
    {
        var matchingObject = await _matchingRepository.GetMatchingById(id);
        if (matchingObject == null)
        {
            _logger.LogError("[MatchingController - Get Question] Matching question not found for the Id {Id: 0000}", id);
            return NotFound("Matching question not found.");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            Console.WriteLine($"Key: {keys[i]}, Answer: {values[i]}, Id: {id}");
        }
        string correctAnswer = matchingObject.CorrectAnswer;
        string questionAnswer = matchingObject.Assemble(keys, values, 2);
        matchingObject.Answer = questionAnswer;
        Console.WriteLine($"Answer: {questionAnswer}, Correct Answer: {correctAnswer}");
        if (questionAnswer == correctAnswer)
        {
            Console.WriteLine("Answer is correct!");
        }
        else
        {
            Console.WriteLine($"Answer is wrong! Correct answer is: {correctAnswer}");
        }
        int correctCounter = 0;
        KeyValuePair<string, string>[] corretAnswerSplit = matchingObject.SplitCorrectAnswer();
        for (int i = 0; i < corretAnswerSplit.Length; i++)
        {
            if (corretAnswerSplit[i].Value == values[i])
            {
                correctCounter++;
            }
        }
        matchingObject.AmountCorrect = correctCounter;
        await _matchingRepository.UpdateMatching(matchingObject);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> CreateMatchingQuestion(List<string> Keys, List<string> Values)
    {
        if (Keys == null || Values == null || Keys.Count != Values.Count)
        {
            ModelState.AddModelError("", "Ugyldige inndata: Keys og Values må være like lange.");
            return View();
        }

        var matchingQuestion = new Matching();
        matchingQuestion.Assemble(Keys, Values, 1);
        matchingQuestion.ShuffleQuestion(Keys, Values);
        await _matchingRepository.CreateMatching(matchingQuestion);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult CreateMatchingQuestion(int id)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ShowMatchings()
    {
        var matchings = await _matchingRepository.GetAll();

        var viewModel = new MatchingViewModel(matchings);
        return View(viewModel);
    }

    public async Task<IActionResult> UpdateMatchingPage(int id)
    {
        var matching = await _matchingRepository.GetMatchingById(id);
        return View(matching);
    }

    [HttpPost]
    public IActionResult UpdateMatching(int id, List<string> keysQuestion, List<string> valuesQuestion, List<string> keysCorrectAnswer, List<string> valuesCorrectAnswer)
    {
        Matching updatetMatching = new Matching();
        updatetMatching.Id = id;
        updatetMatching.Assemble(keysQuestion, valuesQuestion, 3);
        updatetMatching.Assemble(keysCorrectAnswer, valuesCorrectAnswer, 1);
        _matchingRepository.UpdateMatching(updatetMatching);
        return RedirectToAction("ShowMatchings");
    }
    
    public IActionResult DeleteMatching(int id)
    {
        _matchingRepository.DeleteMatching(id);
        return RedirectToAction("ShowMatchings");
    }
}