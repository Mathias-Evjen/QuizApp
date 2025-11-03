using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.DTOs;
using QuizApp.Models;
using QuizApp.Services;
using QuizApp.ViewModels;

namespace QuizApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchingAPIController : ControllerBase
{

    private readonly IQuestionRepository<Matching> _matchingRepository;
    private readonly IAttemptRepository<MatchingAttempt> _matchingAttemptRepository;
    private readonly QuizService _quizService;
    private readonly ILogger<MatchingAPIController> _logger;

    public MatchingAPIController(
        IQuestionRepository<Matching> matchingRepository,
        IAttemptRepository<MatchingAttempt> matchingAttemptRepository,
        QuizService quizService,
        ILogger<MatchingAPIController> logger)
    {
        _matchingRepository = matchingRepository;
        _matchingAttemptRepository = matchingAttemptRepository;
        _quizService = quizService;
        _logger = logger;
    }

    [HttpGet("getQuestions/{quizId}")]
    public async Task<IActionResult> GetQuestions(int quizId)
    {
        var questions = await _matchingRepository.GetAll(m => m.QuizId == quizId);
        if (questions == null)
        {
            _logger.LogError("[MatchingAPIController] Questions list not found while executing _matchingRepository.GetAll()");
            return NotFound("Matching questions not found");
        }
        var questionDtos = questions.Select(question => {
            var keyValuePairs = question.SplitQuestion();

            var keys = keyValuePairs.Select(kvp => kvp.Key).ToList();
            var values = keyValuePairs.Select(kvp => kvp.Value).ToList();
        
            return new MatchingDto
            {
                MatchingId = question.Id,
                QuestionText = question.QuestionText,
                Question = question.Question,
                QuizQuestionNum = question.QuizQuestionNum,
                QuizId = question.QuizId,
                Keys = keys,
                Values = values
            };
        });

        return Ok(questionDtos);
    }

    // [HttpPost]
    // public async Task<IActionResult> SubmitMatchingQuestion(int id, List<string> keys, List<string> values, int quizId, int quizQuestionNum, int quizAttemptId, int numOfQuestions)
    // {
    //     var matchingObject = await _matchingRepository.GetById(id);
    //     if (matchingObject == null)
    //     {
    //         _logger.LogError("[MatchingAPIController - Get Question] Matching question not found for the Id {Id: 0000}", id);
    //         return NotFound("Matching question not found.");
    //     }
    //     string questionAnswer = matchingObject.Assemble(keys, values, 2);
    //     matchingObject.TotalRows = keys.Count;
    //     int correctCounter = 0;
    //     KeyValuePair<string, string>[] correctAnswerSplit = matchingObject.SplitCorrectAnswer();
    //     for (int i = 0; i < correctAnswerSplit.Length; i++)
    //     {
    //         if (correctAnswerSplit[i].Value == values[i])
    //         {
    //             correctCounter++;
    //         }
    //     }
    //     if (!CheckAttempt(quizAttemptId))
    //     {
    //         var matchingAttempt = new MatchingAttempt
    //         {
    //             MatchingId = matchingObject.Id,
    //             QuizAttemptId = quizAttemptId,
    //             UserAnswer = questionAnswer,
    //             AmountCorrect = correctCounter
    //         };
    //         if (correctCounter == matchingObject.TotalRows) { matchingAttempt.AnsweredCorrectly = true; }
    //         else { matchingAttempt.AnsweredCorrectly = false; }
    //
    //         var returnOk = await _matchingAttemptRepository.Create(matchingAttempt);
    //         if (!returnOk)
    //         {
    //             _logger.LogError("[MatchingAPIController] Question attempt creation failed {@attempt}", matchingAttempt);
    //             return RedirectToAction("Quizzes", "Quiz");
    //         }
    //     }
    //     else
    //     {
    //         var matchingAttempt = await _matchingAttemptRepository.GetById(quizAttemptId);
    //         if (matchingAttempt == null)
    //         {
    //             _logger.LogError("[MatchingAPIController - Get Attempt] Matching attempt not found for the Id {Id: 0000}", id);
    //             return NotFound("Matching attempt not found.");
    //         }
    //         matchingAttempt.MatchingId = matchingObject.Id;
    //         matchingAttempt.QuizAttemptId = quizAttemptId;
    //         matchingAttempt.UserAnswer = questionAnswer;
    //         matchingAttempt.AmountCorrect = correctCounter;
    //         if (correctCounter == matchingObject.TotalRows) { matchingAttempt.AnsweredCorrectly = true; }
    //         else { matchingAttempt.AnsweredCorrectly = false; }
    //
    //         var returnOk = await _matchingAttemptRepository.Update(matchingAttempt);
    //         if (!returnOk)
    //         {
    //             _logger.LogError("[MatchingAPIController] Question attempt creation failed {@attempt}", matchingAttempt);
    //             return RedirectToAction("Quizzes", "Quiz");
    //         }
    //     }
    //
    //     if (matchingObject.QuizQuestionNum == numOfQuestions)
    //         return RedirectToAction("Results", "Quiz", new { quizAttemptId = quizAttemptId });
    //
    //     return RedirectToAction("NextQuestion", "Quiz", new
    //     {
    //         quizId = quizId,
    //         quizAttemptId = quizAttemptId,
    //         quizQuestionNum = quizQuestionNum
    //     });
    // }

    public bool CheckAttempt(int quizAttemptId)
    {

        if (quizAttemptId <= 0) { return false; }
        var attempt = _matchingAttemptRepository.Exists(mAttempt => mAttempt.MatchingAttemptId == quizAttemptId);
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

    
    // public IActionResult CreateMatchingQuestion(int quizId, int numOfQuestions)
    // {
    //     var question = new Matching
    //     {
    //         QuizId = quizId,
    //         QuizQuestionNum = numOfQuestions + 1
    //     };
    //     return View(question);
    // }

    [HttpPost("create")]
    public async Task<IActionResult> CreateMatchingQuestion([FromBody] MatchingDto matchingDto)
    {
        if (matchingDto.Keys == null || matchingDto.Values == null || matchingDto.Keys.Count != matchingDto.Values.Count)
        {
            ModelState.AddModelError("", "Ugyldige inndata: Keys og Values må være like lange.");
            return BadRequest("Keys and Value must not be null");
        }

        var matchingQuestion = new Matching
        {
            QuizId = matchingDto.QuizId,
            QuizQuestionNum = matchingDto.QuizQuestionNum
        };
        matchingQuestion.Assemble(matchingDto.Keys, matchingDto.Values, 1);
        matchingQuestion.Assemble(matchingDto.Keys, matchingDto.Values, 3);
        matchingQuestion.ShuffleQuestion(matchingDto.Keys, matchingDto.Values);
        matchingQuestion.TotalRows = matchingDto.Keys.Count;
        matchingQuestion.QuestionText = matchingDto.QuestionText;
        bool returnOk = await _matchingRepository.Create(matchingQuestion);
        if (returnOk)
        {
            await _quizService.ChangeQuestionCount(matchingQuestion.QuizId, true);
            return CreatedAtAction(nameof(GetQuestions), new { quizId = matchingQuestion.QuizId }, matchingQuestion);
        }
        _logger.LogError("[MatchingAPIController] Question creation failed {@question}", matchingQuestion);
        return StatusCode(500, "Internal server error");
    }

    // [HttpGet]
    // public IActionResult CreateMatchingQuestion(Quiz quiz)
    // {
    //     Console.WriteLine(quiz.QuizId + ", " + quiz.NumOfQuestions);
    //     return View(quiz);
    // }
    /*
        [HttpGet]
        public async Task<IActionResult> ShowMatchings()
        {
            var matchings = await _matchingRepository.GetAll();

            var viewModel = new MatchingViewModel(matchings);
            return View(viewModel);
        }
    */
    // public async Task<IActionResult> Edit(int id)
    // {
    //     var matching = await _matchingRepository.GetById(id);
    //     return View("UpdateMatchingPage", matching);
    // }

    [HttpPut("update/{matchingId}")]
    public async Task<IActionResult> Edit([FromBody] MatchingDto matchingDto)
    {
        Matching updatetMatching = new()
        {
            Id = matchingDto.MatchingId,
            QuestionText = matchingDto.QuestionText,
            QuizId = matchingDto.QuizId,
            QuizQuestionNum = matchingDto.QuizQuestionNum
        };
        //TODO finn måte å legge til keys/values for question og correctAnswer
        updatetMatching.Assemble(matchingDto.Keys, matchingDto.Values, 3);
        updatetMatching.Assemble(matchingDto.Keys, matchingDto.Values, 1);
        bool returnOk = await _matchingRepository.Update(updatetMatching);
        if (returnOk) {
            return Ok(updatetMatching);
        }
        _logger.LogError("[MatchingAPIController] Question update failed {@question}", updatetMatching);
        return StatusCode(500, "Internal server error");
    }


    // public async Task<IActionResult> Delete(int id)
    // {
    //     var question = await _matchingRepository.GetById(id);
    //     if (question == null)
    //     {
    //         _logger.LogError("[MatchingAPIController] Question deletion failed for the QuestionId {QuestionId:0000}", id);
    //         return BadRequest("Question not found for the QuestionId");
    //     }
    //     return View(question);
    // }

    [HttpDelete("delete/{questionId}")]
    public async Task<IActionResult> Delete([FromBody] MatchingDto matchingDto)
    {
        bool returnOk = await _matchingRepository.Delete(matchingDto.MatchingId);
        if (!returnOk)
        {
            _logger.LogError("[MatchingAPIController] Question deletion failed for QuestionId {QuestionId:0000}", matchingDto.MatchingId);
            return BadRequest("Question deletion failed");
        }
        await _quizService.ChangeQuestionCount(matchingDto.QuizId, false);
        await _quizService.UpdateQuestionNumbers(matchingDto.QuizQuestionNum, matchingDto.QuizId);
        return NoContent();
    }
}
