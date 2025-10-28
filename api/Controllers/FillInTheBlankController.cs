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

        [HttpPost]
        public async Task<IActionResult> SubmitQuestion(int quizId, int quizAttemptId, int quizQuestionId, int quizQuestionNum, int numOfQuestions, string userAnswer)
        {
            Console.WriteLine(quizAttemptId);
            var fillInTheBlank = await _fillInTheBlankRepository.GetById(quizQuestionId);
            if (fillInTheBlank == null)
            {
                _logger.LogError("[FillInTheBlankAPIController - Submit question] FillInTheBlank question not found for the Id {Id: 0000}", quizQuestionId);
                return NotFound("FillInTheBlank question not found.");
            }

            if (userAnswer != null)
            {
                _logger.LogError("Skal ikke komme hit");
                var fillInTheBlankAttempt = new FillInTheBlankAttempt
                {
                    FillInTheBlankId = fillInTheBlank.FillInTheBlankId,
                    QuizAttemptId = quizAttemptId,
                    UserAnswer = userAnswer
                };

                var returnOk = await _fillInTheBlankAttemptRepository.Create(fillInTheBlankAttempt);
                if (!returnOk)
                {
                    _logger.LogError("[FillInTheBlankAPIController] Question attempt creation failed {@attempt}", fillInTheBlankAttempt);
                    return RedirectToAction("Quizzes", "Quiz");
                }
            }


            if (fillInTheBlank.QuizQuestionNum == numOfQuestions)
                return RedirectToAction("Results", "Quiz", new { quizAttemptId = quizAttemptId });

            return RedirectToAction("NextQuestion", "Quiz", new
            {
                quizId = quizId,
                quizAttemptId = quizAttemptId,
                quizQuestionNum = quizQuestionNum
            });
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

        [HttpDelete("delete/{questionId}")]
        public async Task<IActionResult> Delete(int questionId, [FromQuery] int qNum, [FromQuery] int quizId)
        {
            bool returnOk = await _fillInTheBlankRepository.Delete(questionId);
            if (!returnOk)
            {
                _logger.LogError("[FillInTheBlankAPIController] Question deletion failed for QuestionId {QuestionId:0000}", questionId);
                return BadRequest("Question deletion failed");
            }
            await _quizService.ChangeQuestionCount(quizId, false);
            await _quizService.UpdateQuestionNumbers(qNum, quizId);
            return NoContent();
        }
    }
}