using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.DAL;
using QuizApp.DTOs;

namespace QuizApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlashCardQuizAPIController : ControllerBase
{
    private readonly IRepository<FlashCardQuiz> _flashCardQuizRepository;
    private readonly ILogger<FlashCardQuizAPIController> _logger;

    public FlashCardQuizAPIController(IRepository<FlashCardQuiz> flashCardQuizRepository, ILogger<FlashCardQuizAPIController> logger)
    {
        _flashCardQuizRepository = flashCardQuizRepository;
        _logger = logger;
    }

    [HttpGet("getQuizzes")]
    public async Task<IActionResult> Quizzes()
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

    [HttpGet("getQuiz")]
    public async Task<IActionResult> GetFlashCardQuiz(int id)
    {
        var quiz = await _flashCardQuizRepository.GetById(id);
        if (quiz == null)
        {
            _logger.LogError("[FlashCardQuizAPIController] FlashCardQuiz not found for the Id {Id: 0000}", id);
            return NotFound("FlashCardQuiz not found");
        }
        return Ok(quiz);
    }

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
            return CreatedAtAction(nameof(Quizzes), newQuiz);

        _logger.LogError("[FlashCardQuizAPIController] FlashCardQuiz creation failed {@quiz}", newQuiz);
        return StatusCode(500, "Internal server error");
    }

    [HttpPut("update/{flashCardQuizId}")]
    public async Task<IActionResult> Update(int flashCardQuizId, [FromBody] FlashCardQuizDto flashCardQuizDto)
    {
        if (flashCardQuizDto == null)
            return BadRequest("Flash card quiz data cannot be null");

        var existingQuiz = await _flashCardQuizRepository.GetById(flashCardQuizId);
        if (existingQuiz == null)
        {
            _logger.LogError("[FlashCardQuizAPIcontroller] ManageQuiz not found for the Id {Id: 0000}", flashCardQuizId);
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

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
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