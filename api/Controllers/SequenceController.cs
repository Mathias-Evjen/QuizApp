using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Services;
using QuizApp.DTOs;

namespace QuizApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SequenceAPIController : ControllerBase
{
    private readonly IQuestionRepository<Sequence> _sequenceRepository;
    private readonly IAttemptRepository<SequenceAttempt> _sequenceAttemptRepository;
    private readonly QuizService _quizService;
    private readonly ILogger<SequenceAPIController> _logger;

    public SequenceAPIController(
        IQuestionRepository<Sequence> sequenceRepository,
        IAttemptRepository<SequenceAttempt> sequenceAttemptRepository,
        QuizService quizService,
        ILogger<SequenceAPIController> logger)
    {
        _sequenceRepository = sequenceRepository;
        _sequenceAttemptRepository = sequenceAttemptRepository;
        _quizService = quizService;
        _logger = logger;
    }

    [HttpGet("getQuestions/{quizId}")]
    public async Task<IActionResult> GetQuestions(int quizId)
    {
        var questions = await _sequenceRepository.GetAll(m => m.QuizId == quizId);
        if (questions == null)
        {
            _logger.LogError("[SequenceAPIController] Questions list not found while executing _sequenceRepository.GetAll()");
            return NotFound("Sequence questions not found");
        }
        var questionDtos = questions.Select(question => {        
            return new SequenceDto
            {
                SequenceId = question.SequenceId,
                QuestionText = question.QuestionText,
                Question = question.Question,
                QuizQuestionNum = question.QuizQuestionNum,
                QuizId = question.QuizId
            };
        });

        return Ok(questionDtos);
    }

    [HttpPost("submitAttempts/{quizAttemptId}")]
    public async Task<IActionResult> SubmitAttempt(int quizAttemptId, [FromBody] SequenceAttemptDto sequenceAttemptDto)
    {
        var sequence = await _sequenceRepository.GetById(sequenceAttemptDto.SequenceId);
        if (sequence == null)
        {
            _logger.LogError("[SequenceAPIController - Submit question] Sequence question not found for the Id {Id: 0000}", sequenceAttemptDto.SequenceId);
            return NotFound("Sequence question not found.");
        }

        var sequenceAttempt = new SequenceAttempt
        {
            SequenceId = sequence.SequenceId,
            QuizAttemptId = quizAttemptId,
            UserAnswer = sequenceAttemptDto.UserAnswer,
            QuizQuestionNum = sequenceAttemptDto.QuizQuestionNum
        };

        var returnOk = await _sequenceAttemptRepository.Create(sequenceAttempt);
        if (!returnOk)
        {
            _logger.LogError("[SequenceAPIController] Question attempt creation failed {@attempt}", sequenceAttempt);
            return StatusCode(500, "Internal server error");
        }

        return Ok(sequenceAttempt);
    }


    [HttpPost("create")]
    public async Task<IActionResult> CreateSequenceQuestion([FromBody] SequenceDto sequenceDto)
    {
        var sequenceQuestion = new Sequence
        {
            QuizId = sequenceDto.QuizId,
            QuizQuestionNum = sequenceDto.QuizQuestionNum,
            Question = sequenceDto.Question,
            QuestionText = sequenceDto.QuestionText,
            CorrectAnswer = sequenceDto.CorrectAnswer
        };
        bool returnOk = await _sequenceRepository.Create(sequenceQuestion);
        if (returnOk)
        {
            await _quizService.ChangeQuestionCount(sequenceQuestion.QuizId, true);
            return CreatedAtAction(nameof(GetQuestions), new { quizId = sequenceQuestion.QuizId }, sequenceQuestion);
        }
        _logger.LogError("[SequenceAPIController] Question creation failed {@question}", sequenceQuestion);
        return StatusCode(500, "Internal server error");
    }

    [HttpPut("update/{sequenceId}")]
    public async Task<IActionResult> Update([FromBody] SequenceDto sequenceDto)
    {
        Sequence updatetSequence = new()
        {
            SequenceId = sequenceDto.SequenceId,
            QuestionText = sequenceDto.QuestionText,
            QuizId = sequenceDto.QuizId,
            QuizQuestionNum = sequenceDto.QuizQuestionNum,
            Question = sequenceDto.Question,
            CorrectAnswer = sequenceDto.CorrectAnswer
        };

        bool returnOk = await _sequenceRepository.Update(updatetSequence);
        if (returnOk) {
            return Ok(updatetSequence);
        }
        _logger.LogError("[SequenceAPIController] Question update failed {@question}", updatetSequence);
        return StatusCode(500, "Internal server error");
    }

    [HttpDelete("delete/{sequenceId}")]
    public async Task<IActionResult> Delete(int sequenceId, [FromQuery] int qNum, [FromQuery] int quizId)
    {
        bool returnOk = await _sequenceRepository.Delete(sequenceId);
        if (!returnOk)
        {
            _logger.LogError("[SequenceAPIController] Question deletion failed for QuestionId {QuestionId:0000}", sequenceId);
            return BadRequest("Question deletion failed");
        }
        await _quizService.ChangeQuestionCount(quizId, false);
        await _quizService.UpdateQuestionNumbers(qNum, quizId);
        return NoContent();
    }
}
