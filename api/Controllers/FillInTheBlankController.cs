using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;
using QuizApp.Services;
using QuizApp.DTOs;

namespace QuizApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FillInTheBlankAPIController(IQuestionRepository<FillInTheBlank> fillInTheBlankRepository, IAttemptRepository<FillInTheBlankAttempt> fillInTheBlankAttemptRepository, QuizService quizService, ILogger<FillInTheBlankAPIController> logger) : ControllerBase
    {
        private readonly IQuestionRepository<FillInTheBlank> _fillInTheBlankRepository = fillInTheBlankRepository;
        private readonly IAttemptRepository<FillInTheBlankAttempt> _fillInTheBlankAttemptRepository = fillInTheBlankAttemptRepository;
        private readonly QuizService _quizService = quizService;
        private readonly ILogger<FillInTheBlankAPIController> _logger = logger;

        [HttpGet("getQuestions/{quizId}")]
        public async Task<IActionResult> GetQuestions(int quizId)
        {
            var questions = await _fillInTheBlankRepository.GetAll(fib => fib.QuizId == quizId);
            if (questions == null)
            {
                _logger.LogError("[FillInTheBlankAPIController] FillInTheBlank question list not found while executing _fillInTheBlankREpository.GetAll()");
                return NotFound("Questions not found");
            }

            var questionDtos = questions.Select(question => new FillInTheBlankDto
            {
                FillInTheBlankId = question.FillInTheBlankId,
                Question = question.Question,
                QuizQuestionNum = question.QuizQuestionNum,
                QuizId = question.QuizId
            });

            return Ok(questionDtos);
        }

        [HttpGet("getAttempts/{quizAttemptId}")]
        public async Task<IActionResult> GetAttempts(int quizAttemptId)
        {
            var attempts = await _fillInTheBlankAttemptRepository.GetAll(fiba => fiba.QuizAttemptId == quizAttemptId);
            if (attempts == null)
            {
                _logger.LogError("[FillInTheBlankAPIController] FillInTheBlank attempt list not found while executing _fillInTheBlankAttemptRepository.GetAll()");
                return NotFound("Attempts not found");
            }

            var attemptDtos = attempts.Select(attempt => new FillInTheBlankAttemptdto
            {
                FillInTheBlankAttemptId = attempt.FillInTheBlankId,
                UserAnswer = attempt.UserAnswer,
                QuizQuestionNum = attempt.QuizQuestionNum,
                QuizAttemptId = attempt.QuizAttemptId
            });

            return Ok(attemptDtos);
        }

        [HttpPost("submitAttempts/{quizAttemptId}")]
        public async Task<IActionResult> SubmitAttempt(int quizAttemptId, [FromBody] FillInTheBlankAttemptdto fillInTheBlankAttemptDto)
        {
            var fillInTheBlank = await _fillInTheBlankRepository.GetById(fillInTheBlankAttemptDto.FillInTheBlankId);
            if (fillInTheBlank == null)
            {
                _logger.LogError("[FillInTheBlankAPIController - Submit question] FillInTheBlank question not found for the Id {Id: 0000}", fillInTheBlankAttemptDto.FillInTheBlankId);
                return NotFound("FillInTheBlank question not found.");
            }

            var fillInTheBlankAttempt = new FillInTheBlankAttempt
            {
                FillInTheBlankId = fillInTheBlank.FillInTheBlankId,
                QuizAttemptId = fillInTheBlankAttemptDto.QuizAttemptId,
                UserAnswer = fillInTheBlankAttemptDto.UserAnswer,
                QuizQuestionNum = fillInTheBlankAttemptDto.QuizQuestionNum
            };

            var returnOk = await _fillInTheBlankAttemptRepository.Create(fillInTheBlankAttempt);
            if (!returnOk)
            {
                _logger.LogError("[FillInTheBlankAPIController] Question attempt creation failed {@attempt}", fillInTheBlankAttempt);
                return StatusCode(500, "Internal server error");
            }

            return Ok(fillInTheBlankAttempt);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] FillInTheBlankDto fillInTheBlankDto)
        {
            if (fillInTheBlankDto == null)
                return BadRequest("Question cannot be null");

            var newQuestion = new FillInTheBlank
            {
                Question = fillInTheBlankDto.Question,
                CorrectAnswer = fillInTheBlankDto.CorrectAnswer!,
                QuizId = fillInTheBlankDto.QuizId,
                QuizQuestionNum = fillInTheBlankDto.QuizQuestionNum
            };

            bool returnOk = await _fillInTheBlankRepository.Create(newQuestion);
            if (returnOk)
            {
                await _quizService.ChangeQuestionCount(newQuestion.QuizId, true);
                return CreatedAtAction(nameof(GetQuestions), new { quizId = newQuestion.QuizId }, newQuestion);
            }

            _logger.LogError("[FillInTheBlankAPIController] Question creation failed {@question}", newQuestion);
            return StatusCode(500, "Internal server error");
        }
    
        [HttpPut("update/{fillInTheBlankId}")]
        public async Task<IActionResult> Edit(int fillInTheBlankId, [FromBody] FillInTheBlankDto fillQuestionDto)
        {
            if (fillQuestionDto == null)
                return BadRequest("Question cannot be null");

            var existingQuestion = await _fillInTheBlankRepository.GetById(fillInTheBlankId);
            if (existingQuestion == null)
            {
                _logger.LogError("[FillInTheBlankAPIcontroller] Question not found for the id {Id: 0000}", fillInTheBlankId);
                return NotFound("Question not found for the FillInTheBlankId");
            }

            existingQuestion.Question = fillQuestionDto.Question;
            existingQuestion.CorrectAnswer = fillQuestionDto.CorrectAnswer!;
            existingQuestion.QuizQuestionNum = fillQuestionDto.QuizQuestionNum;
        
            bool returnOk = await _fillInTheBlankRepository.Update(existingQuestion);
            if (returnOk) {
                return Ok(existingQuestion);
            }
            _logger.LogError("[FillInTheBlankAPIController] Question update failed {@question}", existingQuestion);
            return StatusCode(500, "Internal server error");
        }

        [HttpDelete("delete/{fillInTheBlankId}")]
        public async Task<IActionResult> Delete(int fillInTheBlankId, [FromQuery] int qNum, [FromQuery] int quizId)
        {
            bool returnOk = await _fillInTheBlankRepository.Delete(fillInTheBlankId);
            if (!returnOk)
            {
                _logger.LogError("[FillInTheBlankAPIController] Question deletion failed for FillInTheBlankId {fillInTheBlankId:0000}", fillInTheBlankId);
                return BadRequest("Question deletion failed");
            }
            await _quizService.ChangeQuestionCount(quizId, false);
            await _quizService.UpdateQuestionNumbers(qNum, quizId);
            return NoContent();
        }
    }
}