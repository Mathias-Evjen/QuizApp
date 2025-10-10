using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace QuizApp.Controllers;

public class SequenceController : Controller
{
    private readonly ISequenceRepository _sequenceRepository;

    private readonly ILogger<SequenceController> _logger;

    public SequenceController(ISequenceRepository sequenceRepository, ILogger<SequenceController> logger)
    {
        _sequenceRepository = sequenceRepository;
        _logger = logger;
    }

    public async Task<IActionResult> SequenceQuestion()
    {
        List<Sequence> sequence = (await _sequenceRepository.GetAll()).ToList();
        var viewModel = new SequenceViewModel(sequence[0]);

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitSequenceQuestion(int id, List<string> values)
    {
        var sequenceObject = await _sequenceRepository.GetSequenceById(id);
        string answer = sequenceObject.Assemble(values, 2);
        Console.WriteLine("Answer: " + answer);
        if (answer == sequenceObject.CorrectAnswer)
        {
            Console.WriteLine("Answer is correct!");
        }
        else
        {
            Console.WriteLine($"Answer is wrong, correct answer: {sequenceObject.CorrectAnswer}");
        }
        _sequenceRepository.UpdateSequence(sequenceObject);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> CreateSequenceQuestion(string questionText, List<string> Values)
    {
        if (Values == null)
        {
            ModelState.AddModelError("", "Ugyldige inndata: Må ha values.");
            return View();
        }
        var sequenceQuestion = new Sequence();
        sequenceQuestion.QuestionText = questionText;
        sequenceQuestion.Assemble(Values, 1);
        sequenceQuestion.ShuffleQuestion(Values);
        _sequenceRepository.CreateSequence(sequenceQuestion);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult CreateSequenceQuestion(int id)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ShowSequences()
    {
        IEnumerable<Sequence> sequences = await _sequenceRepository.GetAll();

        var viewModel = new SequenceViewModel(sequences);
        return View(viewModel);
    }

    public async Task<IActionResult> UpdateSequencePage(int id)
    {
        Sequence sequence = await _sequenceRepository.GetSequenceById(id);
        return View(sequence);
    }

    [HttpPost]
    public IActionResult UpdateSequence(int id, string questionText, List<string> question, List<string> correctAnswer)
    {
        Sequence updatetSequence = new Sequence();
        updatetSequence.Id = id;
        updatetSequence.QuestionText = questionText;
        updatetSequence.Assemble(question, 3);
        updatetSequence.Assemble(correctAnswer, 1);

        _sequenceRepository.UpdateSequence(updatetSequence);
        return RedirectToAction("ShowSequences");
    }
    
    public IActionResult DeleteSequence(int id)
    {
        _sequenceRepository.DeleteSequence(id);
        return RedirectToAction("ShowSequences");
    }
}