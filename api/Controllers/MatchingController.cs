using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.Services;
using QuizApp.ViewModels;

namespace QuizApp.Controllers;

public class MatchingController : Controller
{

    private readonly IRepository<Matching> _matchingRepository;
    private readonly IAttemptRepository<MatchingAttempt> _matchingAttemptRepository;
    private readonly QuizService _quizService;
    private readonly ILogger<MatchingController> _logger;

    public MatchingController(
        IRepository<Matching> matchingRepository,
        IAttemptRepository<MatchingAttempt> matchingAttemptRepository,
        QuizService quizService,
        ILogger<MatchingController> logger)
    {
        _matchingRepository = matchingRepository;
        _matchingAttemptRepository = matchingAttemptRepository;
        _quizService = quizService;
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
    public async Task<IActionResult> SubmitMatchingQuestion(int id, List<string> keys, List<string> values, int quizId, int quizQuestionNum, int quizAttemptId, int numOfQuestions)
    {
        var matchingObject = await _matchingRepository.GetById(id);
        if (matchingObject == null)
        {
            _logger.LogError("[MatchingController - Get Question] Matching question not found for the Id {Id: 0000}", id);
            return NotFound("Matching question not found.");
        }
        string questionAnswer = matchingObject.Assemble(keys, values, 2);
        matchingObject.TotalRows = keys.Count;
        int correctCounter = 0;
        KeyValuePair<string, string>[] correctAnswerSplit = matchingObject.SplitCorrectAnswer();
        for (int i = 0; i < correctAnswerSplit.Length; i++)
        {
            if (correctAnswerSplit[i].Value == values[i])
            {
                correctCounter++;
            }
        }
        if (!CheckAttempt(quizAttemptId))
        {
            var matchingAttempt = new MatchingAttempt();
            matchingAttempt.MatchingId = matchingObject.Id;
            matchingAttempt.QuizAttemptId = quizAttemptId;
            matchingAttempt.UserAnswer = questionAnswer;
            matchingAttempt.AmountCorrect = correctCounter;
            if (correctCounter == matchingObject.TotalRows) { matchingAttempt.AnsweredCorrectly = true; }
            else { matchingAttempt.AnsweredCorrectly = false; }

            var returnOk = await _matchingAttemptRepository.Create(matchingAttempt);
            if (!returnOk)
            {
                _logger.LogError("[MatchingController] Question attempt creation failed {@attempt}", matchingAttempt);
                return RedirectToAction("Quizzes", "Quiz");
            }
        }
        else
        {
            var matchingAttempt = await _matchingAttemptRepository.GetById(quizAttemptId);
            if (matchingAttempt == null)
            {
                _logger.LogError("[MatchingController - Get Attempt] Matching attempt not found for the Id {Id: 0000}", id);
                return NotFound("Matching attempt not found.");
            }
            matchingAttempt.MatchingId = matchingObject.Id;
            matchingAttempt.QuizAttemptId = quizAttemptId;
            matchingAttempt.UserAnswer = questionAnswer;
            matchingAttempt.AmountCorrect = correctCounter;
            if (correctCounter == matchingObject.TotalRows) { matchingAttempt.AnsweredCorrectly = true; }
            else { matchingAttempt.AnsweredCorrectly = false; }

            var returnOk = await _matchingAttemptRepository.Update(matchingAttempt);
            if (!returnOk)
            {
                _logger.LogError("[MatchingController] Question attempt creation failed {@attempt}", matchingAttempt);
                return RedirectToAction("Quizzes", "Quiz");
            }
        }
        
        if (matchingObject.QuizQuestionNum == numOfQuestions)
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

        if (quizAttemptId <= 0) { return false; }
        var attempt = _matchingAttemptRepository.Exists(quizAttemptId);
        if (!attempt)
        {
            Console.WriteLine("denne er false");
            return false;
        }
        else
        {
            Console.WriteLine("Denne er true");
            return true;
        }
    }
    
    public IActionResult CreateMatchingQuestion(int quizId, int numOfQuestions)
    {
        var question = new Matching
        {
            QuizId = quizId,
            QuizQuestionNum = numOfQuestions + 1
        };
        return View(question);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMatchingQuestion(List<string> Keys, List<string> Values, string questionText, int quizId, int quizQuestionNum)
    {
        Console.WriteLine(quizId + ", " + quizQuestionNum);
        if (Keys == null || Values == null || Keys.Count != Values.Count)
        {
            ModelState.AddModelError("", "Ugyldige inndata: Keys og Values må være like lange.");
            return View();
        }

        var matchingQuestion = new Matching
        {
            QuizId = quizId,
            QuizQuestionNum = quizQuestionNum
        };
        matchingQuestion.Assemble(Keys, Values, 1);
        matchingQuestion.Assemble(Keys, Values, 3);
        matchingQuestion.ShuffleQuestion(Keys, Values);
        matchingQuestion.TotalRows = Keys.Count;
        matchingQuestion.QuestionText = questionText;
        bool returnOk = await _matchingRepository.Create(matchingQuestion);
        if (returnOk)
        {
            await _quizService.ChangeQuestionCount(matchingQuestion.QuizId, true);
            return RedirectToAction("ManageQuiz", "Quiz", new { quizId = matchingQuestion.QuizId});
        }

        _logger.LogError("[MatchingController] Question creation failed {@question}", matchingQuestion);
        return View();
    }

    [HttpGet]
    public IActionResult CreateMatchingQuestion(Quiz quiz)
    {
        Console.WriteLine(quiz.QuizId + ", " + quiz.NumOfQuestions);
        return View(quiz);
    }
/*
    [HttpGet]
    public async Task<IActionResult> ShowMatchings()
    {
        var matchings = await _matchingRepository.GetAll();

        var viewModel = new MatchingViewModel(matchings);
        return View(viewModel);
    }
*/
    public async Task<IActionResult> Edit(int id)
    {
        var matching = await _matchingRepository.GetById(id);
        return View("UpdateMatchingPage", matching);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, List<string> keysQuestion, List<string> valuesQuestion, List<string> keysCorrectAnswer, List<string> valuesCorrectAnswer, string questionText, int quizId, int quizQuestionNum)
    {
        Matching updatetMatching = new()
        {
            Id = id,
            QuestionText = questionText,
            QuizId = quizId,
            QuizQuestionNum = quizQuestionNum
        };
        updatetMatching.Assemble(keysQuestion, valuesQuestion, 3);
        updatetMatching.Assemble(keysCorrectAnswer, valuesCorrectAnswer, 1);
        if (ModelState.IsValid)
        {
            bool returnOk = await _matchingRepository.Update(updatetMatching);
            if (returnOk)
                return RedirectToAction("ManageQuiz", "Quiz", new { quizId = updatetMatching.QuizId });
        }
        _logger.LogError("[MatchingController] Question update failed {@question}", updatetMatching);
        return View(updatetMatching);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var question = await _matchingRepository.GetById(id);
        if (question == null)
        {
            _logger.LogError("[MatchingController] Question deletion failed for the QuestionId {QuestionId:0000}", id);
            return BadRequest("Question not found for the QuestionId");
        }
        return View(question);
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int questionId, int qNum, int quizId)
    {
        bool returnOk = await _matchingRepository.Delete(questionId);
        if (!returnOk)
        {
            _logger.LogError("[MatchingController] Question deletion failed for QuestionId {QuestionId:0000}", questionId);
            return BadRequest("Question deletion failed");
        }
        await _quizService.ChangeQuestionCount(quizId, false);
        await _quizService.UpdateQuestionNumbers(qNum, quizId);
        return RedirectToAction("ManageQuiz", "Quiz", new { quizId });
    }
}