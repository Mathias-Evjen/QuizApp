using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.ViewModels;

namespace QuizApp.Controllers;

public class MatchingController : Controller
{

    public IActionResult MatchingQuestion()
    {
        var testQuestion = new Matching
        {
            Id = 1,
            Question = "elefant,ost,melk,blomst,grønnsak,tiger"
        };

        var viewModel = new MatchingViewModel(testQuestion);

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult SubmitQuestion(int id, List<string> keys, List<string> values)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            Console.WriteLine($"Key: {keys[i]}, Answer: {values[i]}");
        }
        return View();
    }
}