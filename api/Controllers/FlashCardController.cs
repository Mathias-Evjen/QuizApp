using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.DAL;
using QuizApp.Services;
using QuizApp.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace QuizApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlashCardAPIController(IQuestionRepository<FlashCard> flashCardRepository, IFlashCardQuizService flashCardQuizService, ILogger<FlashCardAPIController> logger) : ControllerBase
    {
        private readonly IQuestionRepository<FlashCard> _flashCardRepository = flashCardRepository;
        private readonly IFlashCardQuizService _flashCardQuizService = flashCardQuizService;
        private readonly ILogger<FlashCardAPIController> _logger = logger;

        [HttpGet("getFlashCards/{quizId}")]
        public async Task<IActionResult> GetFlashCards(int quizId)
        {
            var flashCards = await _flashCardRepository.GetAll(fc => fc.QuizId == quizId);
            if (flashCards == null)
            {
                _logger.LogError("[FlashCardAPIController] FlashCards list not found while executing _flashCardRepository.GetAll()");
                return NotFound("FlashCards not found");
            }

            var flashCardDtos = flashCards.Select(card => new FlashCardDto
            {
                FlashCardId = card.FlashCardId,
                Question = card.Question,
                Answer = card.Answer,
                QuizQuestionNum = card.QuizQuestionNum,
                QuizId = card.QuizId,
                Color = _flashCardQuizService.PickRandomFlashCardColor()
            })
            .OrderBy(card => card.QuizQuestionNum);

            return Ok(flashCardDtos);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] FlashCardDto flashCardDto)
        {
            if (flashCardDto == null)
                return BadRequest("Flash card cannot be null");

            var newFlashCard = new FlashCard
            {
                Question = flashCardDto.Question,
                Answer = flashCardDto.Answer,
                QuizId = flashCardDto.QuizId,
                QuizQuestionNum = flashCardDto.QuizQuestionNum
            };

            bool returnOk = await _flashCardRepository.Create(newFlashCard);
            if (returnOk)
            {
                await _flashCardQuizService.ChangeQuestionCount(newFlashCard.QuizId, true);
                return CreatedAtAction(nameof(GetFlashCards), new { quizId = newFlashCard.QuizId }, newFlashCard);
            }
            
            _logger.LogError("[FlashCardAPIController] FlashCard creation failed {@flashCard}", newFlashCard);
            return StatusCode(500, "Internal server error");
        }

        [Authorize]
        [HttpPut("update/{flashCardId}")]
        public async Task<IActionResult> Edit(int flashCardId, [FromBody] FlashCardDto flashCardDto)
        {
            if (flashCardDto == null)
                return BadRequest("Flash card cannot be null");

            if (flashCardId != flashCardDto.FlashCardId)
                return BadRequest("Ids must match");

            var existingFlashCard = await _flashCardRepository.GetById(flashCardId);
            if (existingFlashCard == null)
            {
                _logger.LogError("[FlashCardAPIController] Existing card not found for the Id {Id: 0000}", flashCardId);
                return NotFound("FlashCard not found for the FlashCardId");
            }
            
            existingFlashCard.Question = flashCardDto.Question;
            existingFlashCard.Answer = flashCardDto.Answer;
            existingFlashCard.QuizQuestionNum = flashCardDto.QuizQuestionNum;
        
            bool returnOk = await _flashCardRepository.Update(existingFlashCard);
            if (returnOk)
                return Ok(existingFlashCard);

            _logger.LogError("[FlashCardAPIController] FlashCard update failed {@flashCard}", existingFlashCard);
            return StatusCode(500, "Internal server error");
        }

        [Authorize]
        [HttpDelete("delete/{flashCardId}")]
        public async Task<IActionResult> Delete(int flashCardId, [FromQuery] int qNum, [FromQuery] int quizId)
        {
            bool returnOk = await _flashCardRepository.Delete(flashCardId);
            if (!returnOk)
            {
                _logger.LogError("[FlashCardAPIController] FlashCard deletion failed for FlashCardId {FlashCardId:0000}", flashCardId);
                return BadRequest("FlashCard deletion failed");
            }
            await _flashCardQuizService.ChangeQuestionCount(quizId, false);
            await _flashCardQuizService.UpdateFlashCardQuestionNumbers(qNum, quizId);
            return NoContent();
        }
    }
}