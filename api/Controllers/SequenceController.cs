using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Services;

namespace QuizApp.Controllers;

public class SequenceController : Controller
{
    private readonly IQuestionRepository<Sequence> _sequenceRepository;
    private readonly IAttemptRepository<SequenceAttempt> _sequenceAttemptRepository;
    private readonly QuizService _quizService;
    private readonly ILogger<SequenceController> _logger;

    public SequenceController(
        IQuestionRepository<Sequence> sequenceRepository,
        IAttemptRepository<SequenceAttempt> sequenceAttemptRepository,
        QuizService quizService,
        ILogger<SequenceController> logger)
    {
        _sequenceRepository = sequenceRepository;
        _sequenceAttemptRepository = sequenceAttemptRepository;
        _quizService = quizService;
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
    public async Task<IActionResult> SubmitSequenceQuestion(int id, List<string> values, int quizId, int quizQuestionNum, int quizAttemptId, int numOfQuestions)
    {
        var sequenceObject = await _sequenceRepository.GetById(id);
        if (sequenceObject == null)
        {
            _logger.LogError("[SequenceController - Get Question] Sequence question not found for the Id {Id: 0000}", id);
            return NotFound("Sequence question not found.");
        }

        string answer = sequenceObject.Assemble(values, 2);

        if (!CheckAttempt(quizAttemptId))
        {
            var sequenceAttempt = new SequenceAttempt();
            sequenceAttempt.SequenceId = sequenceObject.Id;
            sequenceAttempt.QuizAttemptId = quizAttemptId;
            sequenceAttempt.UserAnswer = answer;
            if (answer == sequenceObject.CorrectAnswer) { sequenceAttempt.AnsweredCorrectly = true; }
            else{ sequenceAttempt.AnsweredCorrectly = false; }

            var returnOk = await _sequenceAttemptRepository.Create(sequenceAttempt);
            if (!returnOk)
            {
                _logger.LogError("[SequenceController] Question attempt creation failed {@attempt}", sequenceAttempt);
                return RedirectToAction("Quizzes", "Quiz");
            }
        }
        else
        {
            var sequenceAttempt = await _sequenceAttemptRepository.GetById(quizAttemptId);
            if (sequenceAttempt == null)
            {
                _logger.LogError("[SequenceController - Get Attempt] Sequence attempt not found for the Id {Id: 0000}", id);
                return NotFound("Sequence attempt not found.");
            }
            sequenceAttempt.SequenceId = sequenceObject.Id;
            sequenceAttempt.QuizAttemptId = quizAttemptId;
            sequenceAttempt.UserAnswer = answer;
            if (answer == sequenceObject.CorrectAnswer) { sequenceAttempt.AnsweredCorrectly = true; }
            else{ sequenceAttempt.AnsweredCorrectly = false; }

            var returnOk = await _sequenceAttemptRepository.Create(sequenceAttempt);
            if (!returnOk)
            {
                _logger.LogError("[SequenceController] Question attempt creation failed {@attempt}", sequenceAttempt);
                return RedirectToAction("Quizzes", "Quiz");
            }
        }

        if (sequenceObject.QuizQuestionNum == numOfQuestions)
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
        var attempt =  _sequenceAttemptRepository.Exists(sAttempt => sAttempt.SequenceAttemptId == quizAttemptId);
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
    public async Task<IActionResult> CreateSequenceQuestion(string questionText, List<string> Values, int quizId, int quizQuestionNum)
    {
        if (Values == null)
        {
            ModelState.AddModelError("", "Ugyldige inndata: Må ha values.");
            return View();
        }
        var sequenceQuestion = new Sequence
        {
            QuizId = quizId,
            QuizQuestionNum = quizQuestionNum,
            QuestionText = questionText
        };
        sequenceQuestion.Assemble(Values, 1);
        sequenceQuestion.ShuffleQuestion(Values);
        bool returnOk = await _sequenceRepository.Create(sequenceQuestion);
        if (returnOk)
        {
            await _quizService.ChangeQuestionCount(sequenceQuestion.QuizId, true);
            return RedirectToAction("ManageQuiz", "Quiz", new { quizId = sequenceQuestion.QuizId});
        }

        _logger.LogError("[MatchingController] Question creation failed {@question}", sequenceQuestion);
        return View();
    }

    [HttpGet]
    public IActionResult CreateSequenceQuestion(Quiz quiz)
    {
        return View(quiz);
    }

    // [HttpGet]
    // public async Task<IActionResult> ShowSequences()
    // {
    //     var sequences = await _sequenceRepository.GetAll();

    //     var viewModel = new SequenceViewModel(sequences);
    //     return View(viewModel);
    // }

    public async Task<IActionResult> Edit(int id)
    {
        var sequence = await _sequenceRepository.GetById(id);
        return View("UpdateSequencePage", sequence);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, string questionText, List<string> question, List<string> correctAnswer, int quizId, int quizQuestionNum)
    {
        Sequence updatetSequence = new Sequence
        {
            Id = id,
            QuestionText = questionText,
            QuizId = quizId,
            QuizQuestionNum = quizQuestionNum
        };
        updatetSequence.Assemble(question, 3);
        updatetSequence.Assemble(correctAnswer, 1);

        if (ModelState.IsValid)
        {
            bool returnOk = await _sequenceRepository.Update(updatetSequence);
            if (returnOk)
                return RedirectToAction("ManageQuiz", "Quiz", new { quizId = updatetSequence.QuizId });
        }
        _logger.LogError("[SequenceController] Question update failed {@question}", updatetSequence);
        return View(updatetSequence);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var question = await _sequenceRepository.GetById(id);
        if (question == null)
        {
            _logger.LogError("[SequenceController] Question deletion failed for the QuestionId {QuestionId:0000}", id);
            return BadRequest("Question not found for the QuestionId");
        }
        return View(question);
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int questionId, int qNum, int quizId)
    {
        bool returnOk = await _sequenceRepository.Delete(questionId);
        if (!returnOk)
        {
            _logger.LogError("[SequenceController] Question deletion failed for QuestionId {QuestionId:0000}", questionId);
            return BadRequest("Question deletion failed");
        }
        await _quizService.ChangeQuestionCount(quizId, false);
        await _quizService.UpdateQuestionNumbers(qNum, quizId);
        return RedirectToAction("ManageQuiz", "Quiz", new { quizId });
    }
}
