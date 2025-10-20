using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace QuizApp.Controllers;

public class SequenceController : Controller
{
    private readonly ISequenceRepository _sequenceRepository;
    private readonly ISequenceAttemptRepository _sequenceAttemptRepository;

    private readonly ILogger<SequenceController> _logger;

    public SequenceController(ISequenceRepository sequenceRepository, ISequenceAttemptRepository sequenceAttemptRepository, ILogger<SequenceController> logger)
    {
        _sequenceRepository = sequenceRepository;
        _sequenceAttemptRepository = sequenceAttemptRepository;
        _logger = logger;
    }

    public async Task<IActionResult> SequenceQuestion()
    {
        var sequence = await _sequenceRepository.GetAll();
        if (sequence == null)
        {
            _logger.LogError("[SequenceController] Questions list not found while executing _SequenceRepository.GetAll()");
            return NotFound("Sequence question not found");
        }

        var viewModel = new SequenceViewModel(sequence.ElementAt(0));

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitSequenceQuestion(int id, List<string> values, int quizId, int quizQuestionNum, int quizAttemptId)
    {
        var sequenceObject = await _sequenceRepository.GetSequenceById(id);
        if (sequenceObject == null)
        {
            _logger.LogError("[SequenceController - Get Question] Sequence question not found for the Id {Id: 0000}", id);
            return NotFound("Sequence question not found.");
        }

        string answer = sequenceObject.Assemble(values, 2);
        var sequenceAttempt = new SequenceAttempt
        {
            SequenceId = sequenceObject.Id,
            QuizAttemptId = quizAttemptId,
            UserAnswer = answer
        };

        var returnOk = await _sequenceAttemptRepository.CreateSequenceAttempt(sequenceAttempt);
        if (!returnOk)
        {
            _logger.LogError("[SequenceController] Question attempt creation failed {@attempt}", sequenceAttempt);
            return RedirectToAction("Quizzes", "Quiz");
        }
        return RedirectToAction("NextQuestion", "Quiz", new
        {
            quizId = quizId,
            quizAttemptId = quizAttemptId,
            quizQuestionNum = quizQuestionNum
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateSequenceQuestion(string questionText, List<string> Values)
    {
        if (Values == null)
        {
            ModelState.AddModelError("", "Ugyldige inndata: Må ha values.");
            return View();
        }
        var sequenceQuestion = new Sequence
        {
            QuestionText = questionText
        };
        sequenceQuestion.Assemble(Values, 1);
        sequenceQuestion.ShuffleQuestion(Values);
        await _sequenceRepository.CreateSequence(sequenceQuestion);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult CreateSequenceQuestion(int id)
    {
        return View();
    }

    // [HttpGet]
    // public async Task<IActionResult> ShowSequences()
    // {
    //     var sequences = await _sequenceRepository.GetAll();

    //     var viewModel = new SequenceViewModel(sequences);
    //     return View(viewModel);
    // }

    public async Task<IActionResult> UpdateSequencePage(int id)
    {
        var sequence = await _sequenceRepository.GetSequenceById(id);
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