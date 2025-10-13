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
        List<Matching> matching = (await _matchingRepository.GetAll()).ToList();
        var viewModel = new MatchingViewModel(matching[0]);

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitMatchingQuestion(int id, List<string> keys, List<string> values)
    {
        var matchingObject = await _matchingRepository.GetMatchingById(id);
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
        _matchingRepository.UpdateMatching(matchingObject);
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
        _matchingRepository.CreateMatching(matchingQuestion);

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
        IEnumerable<Matching> matchings = await _matchingRepository.GetAll();

        var viewModel = new MatchingViewModel(matchings);
        return View(viewModel);
    }

    public async Task<IActionResult> UpdateMatchingPage(int id)
    {
        Matching matching = await _matchingRepository.GetMatchingById(id);
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