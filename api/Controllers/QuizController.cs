using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.ViewModels;
using QuizApp.DAL;
using QuizApp.Services;
using QuizApp.DTOs;

namespace QuizApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizAPIController : ControllerBase
{
    private readonly IQuizRepository<Quiz> _quizRepository;
    private readonly IAttemptRepository<QuizAttempt> _quizAttemptRepository;
    private readonly QuizService _quizService;
    private readonly ILogger<QuizAPIController> _logger;
    
    public QuizAPIController(IQuizRepository<Quiz> quizRepository,
                          IAttemptRepository<QuizAttempt> quizAttemptRepository,
                          QuizService quizService,
                          ILogger<QuizAPIController> logger)
    {
        _quizRepository = quizRepository;
        _quizAttemptRepository = quizAttemptRepository;
        _quizService = quizService;
        _logger = logger;
    }

    [HttpGet("getQuizzes")]
    public async Task<IActionResult> GetQuizzes()
    {
        var quizzes = await _quizRepository.GetAll();
        if (quizzes == null)
        {
            _logger.LogError("[Quizcontroller] Quizzes list not found while executing _quizRepository.GetAll()");
            return NotFound("Quizzes not found");
        }
        var quizDto = quizzes.Select(quiz => new QuizDto
        {
            QuizId = quiz.QuizId,
            Name = quiz.Name,
            Description = quiz.Description,
            NumOfQuestions = quiz.NumOfQuestions,
            FillInTheBlankQuestions = quiz.FillInTheBlankQuestions,
            MatchingQuestions = quiz.MatchingQuestions,
            SequenceQuestions = quiz.SequenceQuestions,
            RankingQuestions = quiz.RankingQuestions,
            TrueFalseQuestions = quiz.TrueFalseQuestions,
            MultipleChoiceQuestions = quiz.MultipleChoiceQuestions
        });
        return Ok(quizDto);
    }

    [HttpGet("getQuiz/{id}")]
    public async Task<IActionResult> GetQuiz(int id)
    {
        var quiz = await _quizRepository.GetById(id);
        if (quiz == null)
        {
            _logger.LogError("[QuizAPIController] Quiz not found for the Id {Id: 0000}", id);
            return NotFound("Quiz not found");
        }
        return Ok(quiz);
    }

    [HttpGet("getAttempt/{id}")]
    public async Task<IActionResult> GetAttempt(int id)
    {
        var quizAttempt = await _quizAttemptRepository.GetById(id);
        if (quizAttempt == null)
        {
            _logger.LogError("[QuizAPIController] Quiz attempt not found for the Id {Id: 0000}", id);
            return NotFound("Quiz attempt not found");
        }
        return Ok(quizAttempt);
    }

    // [HttpGet]
    // public async Task<IActionResult> Results(int quizAttemptId)
    // {
    //     var quizAttempt = await _quizAttemptRepository.GetById(quizAttemptId);
    //     if (quizAttempt == null)
    //     {
    //         _logger.LogError("[QuizAPIController - GetQuizAttemptById] Quiz attempt not found for the Id {Id: 0000}", quizAttemptId);
    //         return NotFound("Quiz not found.");
    //     }

    //     var quiz = await _quizRepository.GetById(quizAttempt.QuizId);
    //     if (quiz == null)
    //     {
    //         _logger.LogError("[QuizAPIController - Get Quiz By Id] Quiz not found for the Id {Id: 0000}", quizAttempt.QuizId);
    //         return NotFound("Quiz not found.");
    //     }

    //     for (int i = 0; i < quiz.AllQuestions.Count(); i++)
    //     {
    //         var question = quiz.AllQuestions.ElementAt(i);
    //         var questionAttempt = quizAttempt.AllQuestionAttempts.ElementAt(i);
    //         if (question is FillInTheBlank fib && questionAttempt is FillInTheBlankAttempt fibAttempt)
    //         {
    //             fibAttempt.AnsweredCorrectly = _quizService.CheckAnswer(fib.CorrectAnswer, fibAttempt.UserAnswer);
    //         }
    //         if (question is TrueFalse tf && questionAttempt is TrueFalseAttempt tfAttempt)
    //         {
    //             tfAttempt.AnsweredCorrectly = _quizService.CheckAnswer(tf.CorrectAnswer, tfAttempt.UserAnswer);
    //         }
    //         if (question is MultipleChoice mc && questionAttempt is MultipleChoiceAttempt mcAttempt)
    //         {
    //             mcAttempt.AnsweredCorrectly = _quizService.CheckAnswer(mc.CorrectAnswer!, mcAttempt.UserAnswer);
    //         }
    //     }

    //     var quizResultViewModel = new QuizResultViewModel(quiz, quizAttempt);

    //     return View(quizResultViewModel);
    // }
    
    // [HttpGet]
    // public IActionResult Create()
    // {
    //     return View();
    // }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] QuizDto quizDto)
    {
        if (quizDto == null)
        {
            return BadRequest("Quiz cannot be null");
        }
        var quiz = new Quiz
        {
          Name = quizDto.Name,
          Description = quizDto.Description  
        };
        bool returnOk = await _quizRepository.Create(quiz);
        if (returnOk)
            return CreatedAtAction(nameof(GetQuizzes), quiz);
        _logger.LogError("[QuizAPIController] Quiz creation failed {@quiz}", quiz);
        return StatusCode(500, "Internal server error");
    }

    [HttpPost("createAttempt")]
    public async Task<IActionResult> CreateAttempt([FromBody] QuizAttemptDto quizAttemptDto)
    {   
        if (quizAttemptDto == null)
        {
            return BadRequest("Quiz cannot be null");
        }
        var quizAttempt = new QuizAttempt
        {
            QuizId = quizAttemptDto.QuizId
            //Legge til attempts for alle quiztyper
        };

        bool returnOk = await _quizAttemptRepository.Create(quizAttempt);
        if (returnOk)
            return CreatedAtAction(nameof(GetAttempt), new { id = quizAttempt.QuizAttemptId}, quizAttempt);

        _logger.LogError("QuizAPIController] QuizAttempt creation failed {@quizAttempt}", quizAttempt);
        return StatusCode(500, "Internal server error");
    }


    [HttpPut("update/{quizId}")]
    public async Task<IActionResult> Update(int quizId, [FromBody] QuizDto quizDto)
    {
        if (quizDto == null)
            return BadRequest("Quiz data cannot be null");
        var quiz = await _quizRepository.GetById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[QuizAPIController] Quiz not found for the Id {Id: 0000}", quizId);
            return NotFound("Quiz not found for the QuizId");
        }

        quiz.Name = quizDto.Name;
        quiz.Description = quizDto.Description;

        bool returnOk = await _quizRepository.Update(quiz);
        if (returnOk)
            return Ok(quiz);

        _logger.LogError("[QuizAPIController] Quiz update failed {@quiz}", quiz);
        return StatusCode(500, "Internal server error");
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool returnOk = await _quizRepository.Delete(id);
        if (!returnOk)
        {
            _logger.LogError("[QuizAPIController] Quiz deletion failed for QuizId {Id:0000}", id);
            return BadRequest("Quiz deletion failed");
        }
        return NoContent();
    }
}