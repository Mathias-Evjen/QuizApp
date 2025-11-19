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
                MatchingId = question.MatchingId,
                QuestionText = question.QuestionText,
                Question = question.Question,
                QuizQuestionNum = question.QuizQuestionNum,
                QuizId = question.QuizId,
            };
        });

        return Ok(questionDtos);
    }

    [HttpPost("submitAttempts/{quizAttemptId}")]
    public async Task<IActionResult> SubmitAttempt(int quizAttemptId, [FromBody] MatchingAttemptDto matchingAttemptDto)
    {
        var matching = await _matchingRepository.GetById(matchingAttemptDto.MatchingId);
        if (matching == null)
        {
            _logger.LogError("[MatchingAPIController - Submit question] Matching question not found for the Id {Id: 0000}", matchingAttemptDto.MatchingId);
            return NotFound("Matching question not found.");
        }

        var matchingAttempt = new MatchingAttempt
        {
            MatchingId = matching.MatchingId,
            QuizAttemptId = quizAttemptId,
            UserAnswer = matchingAttemptDto.UserAnswer,
            QuizQuestionNum = matchingAttemptDto.QuizQuestionNum
        };

        var returnOk = await _matchingAttemptRepository.Create(matchingAttempt);
        if (!returnOk)
        {
            _logger.LogError("[MatchingAPIController] Question attempt creation failed {@attempt}", matchingAttempt);
            return StatusCode(500, "Internal server error");
        }

        return Ok(matchingAttempt);
    }


    [HttpPost("create")]
    public async Task<IActionResult> CreateMatchingQuestion([FromBody] MatchingDto matchingDto)
    {
        var matchingQuestion = new Matching
        {
            QuizId = matchingDto.QuizId,
            QuizQuestionNum = matchingDto.QuizQuestionNum,
            Question = matchingDto.Question,
            QuestionText = matchingDto.QuestionText,
            CorrectAnswer = matchingDto.CorrectAnswer
        };
        bool returnOk = await _matchingRepository.Create(matchingQuestion);
        if (returnOk)
        {
            await _quizService.ChangeQuestionCount(matchingQuestion.QuizId, true);
            return CreatedAtAction(nameof(GetQuestions), new { quizId = matchingQuestion.QuizId }, matchingQuestion);
        }
        _logger.LogError("[MatchingAPIController] Question creation failed {@question}", matchingQuestion);
        return StatusCode(500, "Internal server error");
    }


    [HttpPut("update/{matchingId}")]
    public async Task<IActionResult> Update([FromBody] MatchingDto matchingDto)
    {
        Matching updatetMatching = new()
        {
            MatchingId = matchingDto.MatchingId,
            QuestionText = matchingDto.QuestionText,
            QuizId = matchingDto.QuizId,
            QuizQuestionNum = matchingDto.QuizQuestionNum,
            CorrectAnswer = matchingDto.CorrectAnswer
        };
        KeyValuePair<string, string>[] splitQuestion = updatetMatching.SplitCorrectAnswer();
        List<string> keys = splitQuestion.Select(kv => kv.Key).ToList();
        List<string> values = splitQuestion.Select(kv => kv.Value).ToList();
        updatetMatching.ShuffleQuestion(keys, values);
        bool returnOk = await _matchingRepository.Update(updatetMatching);
        if (returnOk) {
            return Ok(updatetMatching);
        }
        _logger.LogError("[MatchingAPIController] Question update failed {@question}", updatetMatching);
        return StatusCode(500, "Internal server error");
    }


    [HttpDelete("delete/{matchingId}")]
    public async Task<IActionResult> Delete(int matchingId, [FromQuery] int quizQuestionNum, [FromQuery] int quizId)
    {
        bool returnOk = await _matchingRepository.Delete(matchingId);
        if (!returnOk)
        {
            _logger.LogError("[MatchingAPIController] Question deletion failed for QuestionId {QuestionId:0000}", matchingId);
            return BadRequest("Question deletion failed");
        }
        await _quizService.ChangeQuestionCount(quizId, false);
        await _quizService.UpdateQuestionNumbers(quizQuestionNum, quizId);
        return NoContent();
    }
}
