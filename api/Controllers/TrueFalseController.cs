using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.DTOs;
using QuizApp.Models;
using QuizApp.Services;

namespace QuizApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrueFalseAPIController : ControllerBase
    {
        private readonly IQuestionRepository<TrueFalse> _trueFalseRepository;
        private readonly IAttemptRepository<TrueFalseAttempt> _trueFalseAttemptRepository;
        private readonly QuizService _quizService;
        private readonly ILogger<TrueFalseAPIController> _logger;

        public TrueFalseAPIController(
            IQuestionRepository<TrueFalse> trueFalseRepository,
            IAttemptRepository<TrueFalseAttempt> trueFalseAttemptRepository,
            QuizService quizService,
            ILogger<TrueFalseAPIController> logger)
        {
            _trueFalseRepository = trueFalseRepository;
            _trueFalseAttemptRepository = trueFalseAttemptRepository;
            _quizService = quizService;
            _logger = logger;
        }

        [HttpGet("getQuestions/{quizId}")]
        public async Task<IActionResult> GetQuestions(int quizId)
        {
            var questions = await _trueFalseRepository.GetAll(q => q.QuizId == quizId);

            if (questions == null || !questions.Any())
            {
                return NotFound("No True/False questions found for this quiz.");
            }

            var dtoList = questions.Select(q => new TrueFalseDto
            {
                TrueFalseId = q.TrueFalseId,
                Question = q.Question,
                CorrectAnswer = q.CorrectAnswer,
                QuizId = q.QuizId,
                QuizQuestionNum = q.QuizQuestionNum
            });

            return Ok(dtoList);
        }

        [HttpPost("submitQuestion")]
        public async Task<IActionResult> SubmitQuestion([FromBody] TrueFalseAttemptDto trueFalseAttemptDto)
        {
            var trueFalseAttempt = new TrueFalseAttempt
            {
                TrueFalseId = trueFalseAttemptDto.TrueFalseId,
                QuizAttemptId = trueFalseAttemptDto.QuizAttemptId,
                UserAnswer = trueFalseAttemptDto.UserAnswer,
                QuizQuestionNum = trueFalseAttemptDto.QuizQuestionNum
            };

            var returnOk = await _trueFalseAttemptRepository.Create(trueFalseAttempt);
            if (!returnOk)
            {
                _logger.LogError("[FillInTheBlankAPIController] Question attempt creation failed {@attempt}", trueFalseAttempt);
                return StatusCode(500, "Internal server error");
            }

            return Ok(trueFalseAttempt);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] TrueFalseDto dto)
        {
            if (dto == null)
                return BadRequest("Question cannot be null");

            var question = new TrueFalse
            {
                Question = dto.Question,
                CorrectAnswer = dto.CorrectAnswer,
                QuizId = dto.QuizId,
                QuizQuestionNum = dto.QuizQuestionNum
            };

            bool ok = await _trueFalseRepository.Create(question);
            if (ok)
            {
                await _quizService.ChangeQuestionCount(question.QuizId, true);
                return CreatedAtAction(nameof(GetQuestions), new { quizId = question.QuizId }, question);
            }

            _logger.LogError("[TrueFalseAPI] Creation failed {@question}", question);
            return StatusCode(500, "Internal server error");
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] TrueFalseDto dto)
        {
            if (dto == null)
                return BadRequest("Question cannot be null");

            var existing = await _trueFalseRepository.GetById(id);
            if (existing == null)
                return NotFound("Question not found");

            existing.Question = dto.Question;
            existing.CorrectAnswer = dto.CorrectAnswer;
            existing.QuizQuestionNum = dto.QuizQuestionNum;

            bool ok = await _trueFalseRepository.Update(existing);
            if (ok)
                return Ok(existing);

            return StatusCode(500, "Internal server error");
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int qNum, [FromQuery] int quizId)
        {
            bool ok = await _trueFalseRepository.Delete(id);
            if (!ok)
                return BadRequest("Question deletion failed");

            await _quizService.ChangeQuestionCount(quizId, false);
            await _quizService.UpdateQuestionNumbers(qNum, quizId);

            return NoContent();
        }
    }
}
