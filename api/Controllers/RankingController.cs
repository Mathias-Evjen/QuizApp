using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Services;
using QuizApp.DTOs;

namespace QuizApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RankingAPIController : ControllerBase
{
    private readonly IQuestionRepository<Ranking> _rankingRepository;
    private readonly IAttemptRepository<RankingAttempt> _rankingAttemptRepository;
    private readonly QuizService _quizService;
    private readonly ILogger<RankingAPIController> _logger;

    public RankingAPIController(
        IQuestionRepository<Ranking> rankingRepository,
        IAttemptRepository<RankingAttempt> rankingAttemptRepository,
        QuizService quizService,
        ILogger<RankingAPIController> logger)
    {
        _rankingRepository = rankingRepository;
        _rankingAttemptRepository = rankingAttemptRepository;
        _quizService = quizService;
        _logger = logger;
    }

    [HttpGet("getQuestions/{quizId}")]
    public async Task<IActionResult> GetQuestions(int quizId)
    {
        var questions = await _rankingRepository.GetAll(m => m.QuizId == quizId);
        if (questions == null)
        {
            _logger.LogError("[RankingAPIController] Questions list not found while executing _rankingRepository.GetAll()");
            return NotFound("Ranking questions not found");
        }
        var questionDtos = questions.Select(question => {        
            return new RankingDto
            {
                RankingId = question.RankingId,
                QuestionText = question.QuestionText,
                Question = question.Question,
                QuizQuestionNum = question.QuizQuestionNum,
                QuizId = question.QuizId
            };
        });

        return Ok(questionDtos);
    }

    [HttpPost("submitAttempts/{quizAttemptId}")]
    public async Task<IActionResult> SubmitAttempt(int quizAttemptId, [FromBody] RankingAttemptDto rankingAttemptDto)
    {
        var ranking = await _rankingRepository.GetById(rankingAttemptDto.RankingId);
        if (ranking == null)
        {
            _logger.LogError("[RankingAPIController - Submit question] Ranking question not found for the Id {Id: 0000}", rankingAttemptDto.RankingId);
            return NotFound("Ranking question not found.");
        }

        var rankingAttempt = new RankingAttempt
        {
            RankingId = ranking.RankingId,
            QuizAttemptId = quizAttemptId,
            UserAnswer = rankingAttemptDto.UserAnswer,
            QuizQuestionNum = rankingAttemptDto.QuizQuestionNum
        };

        var returnOk = await _rankingAttemptRepository.Create(rankingAttempt);
        if (!returnOk)
        {
            _logger.LogError("[RankingAPIController] Question attempt creation failed {@attempt}", rankingAttempt);
            return StatusCode(500, "Internal server error");
        }

        return Ok(rankingAttempt);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateRankingQuestion([FromBody] RankingDto rankingDto)
    {
        var rankingQuestion = new Ranking
        {
            QuizId = rankingDto.QuizId,
            QuizQuestionNum = rankingDto.QuizQuestionNum,
            QuestionText = rankingDto.QuestionText,
            CorrectAnswer = rankingDto.CorrectAnswer
        };
        rankingQuestion.Question = rankingQuestion.ShuffleQuestion(rankingDto.CorrectAnswer.Split(",").ToList());
        bool returnOk = await _rankingRepository.Create(rankingQuestion);
        if (returnOk)
        {
            await _quizService.ChangeQuestionCount(rankingQuestion.QuizId, true);
            return CreatedAtAction(nameof(GetQuestions), new { quizId = rankingQuestion.QuizId }, rankingQuestion);
        }
        _logger.LogError("[RankingAPIController] Question creation failed {@question}", rankingQuestion);
        return StatusCode(500, "Internal server error");
    }

    [HttpPut("update/{rankingId}")]
    public async Task<IActionResult> Update([FromBody] RankingDto rankingDto)
    {
        Ranking updatetRanking = new()
        {
            RankingId = rankingDto.RankingId,
            QuestionText = rankingDto.QuestionText,
            QuizId = rankingDto.QuizId,
            QuizQuestionNum = rankingDto.QuizQuestionNum,
            Question = rankingDto.Question,
            CorrectAnswer = rankingDto.CorrectAnswer
        };

        bool returnOk = await _rankingRepository.Update(updatetRanking);
        if (returnOk) {
            return Ok(updatetRanking);
        }
        _logger.LogError("[RankingAPIController] Question update failed {@question}", updatetRanking);
        return StatusCode(500, "Internal server error");
    }

    [HttpDelete("delete/{rankingId}")]
    public async Task<IActionResult> Delete(int rankingId, [FromQuery] int quizQuestionNum, [FromQuery] int quizId)
    {
        bool returnOk = await _rankingRepository.Delete(rankingId);
        if (!returnOk)
        {
            _logger.LogError("[RankingAPIController] Question deletion failed for QuestionId {QuestionId:0000}", rankingId);
            return BadRequest("Question deletion failed");
        }
        await _quizService.ChangeQuestionCount(quizId, false);
        await _quizService.UpdateQuestionNumbers(quizQuestionNum, quizId);
        return NoContent();
    }
}