using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.DAL;
using QuizApp.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace QuizApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlashCardQuizAPIController(IQuizRepository<FlashCardQuiz> flashCardQuizRepository, ILogger<FlashCardQuizAPIController> logger) : ControllerBase
    {
        private readonly IQuizRepository<FlashCardQuiz> _flashCardQuizRepository = flashCardQuizRepository;
        private readonly ILogger<FlashCardQuizAPIController> _logger = logger;

        [HttpGet("getQuizzes")]
        public async Task<IActionResult> GetQuizzes()
        {
            var quizzes = await _flashCardQuizRepository.GetAll();
            if (quizzes == null)
            {
                _logger.LogError("[FlashCardQuizAPIcontroller] FlashCardQuizzes list not found while executing _flashCardQuizRepository.GetAll()");
                return NotFound("FlashCardQuizzes not found");
            }

            var quizDtos = quizzes.Select(quiz => new FlashCardQuizDto
            {
                FlashCardQuizId = quiz.FlashCardQuizId,
                Name = quiz.Name,
                Description = quiz.Description,
                NumOfQuestions = quiz.NumOfQuestions
            });

            return Ok(quizDtos);
        }

        [HttpGet("getQuiz/{id}")]
        public async Task<IActionResult> GetQuiz(int id)
        {
            var quiz = await _flashCardQuizRepository.GetById(id);
            if (quiz == null)
            {
                _logger.LogError("[FlashCardQuizAPIController] FlashCardQuiz not found for the Id {Id: 0000}", id);
                return NotFound("FlashCardQuiz not found");
            }
            return Ok(quiz);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] FlashCardQuizDto flashCardQuizDto)
        {
            if (flashCardQuizDto == null)
            {
                return BadRequest("Flash card quiz cannot be null");
            }

            var newQuiz = new FlashCardQuiz
            {
                Name = flashCardQuizDto.Name,
                Description = flashCardQuizDto.Description
            };

            bool returnOk = await _flashCardQuizRepository.Create(newQuiz);
            if (returnOk)
                return CreatedAtAction(nameof(GetQuizzes), newQuiz);

            _logger.LogError("[FlashCardQuizAPIController] FlashCardQuiz creation failed {@quiz}", newQuiz);
            return StatusCode(500, "Internal server error");
        }

        [Authorize]
        [HttpPut("update/{flashCardQuizId}")]
        public async Task<IActionResult> Update(int flashCardQuizId, [FromBody] FlashCardQuizDto flashCardQuizDto)
        {
            if (flashCardQuizDto == null)
                return BadRequest("Flash card quiz cannot be null");

            if (flashCardQuizId != flashCardQuizDto.FlashCardQuizId)
                return BadRequest("Ids must match");

            var existingQuiz = await _flashCardQuizRepository.GetById(flashCardQuizId);
            if (existingQuiz == null)
            {
                _logger.LogError("[FlashCardQuizAPIcontroller] Exisitng quiz not found for the Id {Id: 0000}", flashCardQuizId);
                return NotFound("FlashCardQuiz not found for the FlashCardQuizId");
            }

            existingQuiz.Name = flashCardQuizDto.Name;
            existingQuiz.Description = flashCardQuizDto.Description;

            bool returnOk = await _flashCardQuizRepository.Update(existingQuiz);
            if (returnOk)
                return Ok(existingQuiz);
        
            _logger.LogError("[FlashCardQuizAPIController] FlashCardQuiz update failed {@quiz}", existingQuiz);
            return StatusCode(500, "Internal server error");
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool returnOk = await _flashCardQuizRepository.Delete(id);
            if (!returnOk)
            {
                _logger.LogError("[FlashCardQuizAPIController] FlashCardQuiz deletion failed for FlashCardQuizId {Id:0000}", id);
                return BadRequest("FlashCardQuiz deletion failed");
            }
            return NoContent();
        }
    }
}