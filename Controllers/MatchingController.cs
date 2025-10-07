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
        var testQuestion = new Matching
        {
            Id = 4,
            Question = "elefant,ost,melk,blomst,grønnsak,tiger",
            CorrectAnswer = "elefant,tiger,melk,ost,grønnsak,blomst"
        };
        _matchingRepository.CreateMatching(testQuestion);
        Matching matchingJustMade = await _matchingRepository.GetMatchingById(4);
        Console.WriteLine($"Correct Answer: {matchingJustMade.CorrectAnswer}, Question: {matchingJustMade.Question}");

        var viewModel = new MatchingViewModel(testQuestion);

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
        string questionAnswer = matchingObject.AssembleAnswer(keys, values);
        Console.WriteLine($"Answer: {questionAnswer}, Correct Answer: {correctAnswer}");
        if (questionAnswer == correctAnswer)
        {
            Console.WriteLine("Answer is correct!");
        }
        else
        {
            Console.WriteLine($"Answer is wrong! Correct answer is: {correctAnswer}");
        }

        return View();
    }
}