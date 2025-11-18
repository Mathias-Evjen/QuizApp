using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.DTOs;
using QuizApp.Models;
using QuizApp.Services;
using QuizApp.ViewModels;

namespace QuizApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MultipleChoiceAPIController : ControllerBase
    {
        private readonly IQuestionRepository<MultipleChoice> _multipleChoiceRepository;
        private readonly IAttemptRepository<MultipleChoiceAttempt> _multipleChoiceAttemptRepository;
        private readonly QuizService _quizService;
        private readonly ILogger<MultipleChoiceAPIController> _logger;

        public MultipleChoiceAPIController(IQuestionRepository<MultipleChoice> multipleChoiceRepository,
                                        IAttemptRepository<MultipleChoiceAttempt> multipleChoiceAttemptRepository,
                                        QuizService quizService,
                                        ILogger<MultipleChoiceAPIController> logger)
        {
            _multipleChoiceRepository = multipleChoiceRepository;
            _multipleChoiceAttemptRepository = multipleChoiceAttemptRepository;
            _quizService = quizService;
            _logger = logger;
        }

        [HttpGet("getQuestions/{quizId}")]
        public async Task<IActionResult> GetQuestions(int quizId)
        {
            Console.WriteLine("Funker");
            var questions = await _multipleChoiceRepository.GetAll(q => q.QuizId == quizId);

            if (questions == null || !questions.Any())
            {
                _logger.LogWarning("[MultipleChoiceAPIController] No MultipleChoice questions found for QuizId={QuizId}", quizId);
                return NotFound("No multiple choice questions found for this quiz.");
            }

            var questionDtos = questions.Select(q => new MultipleChoiceDto
            {
                MultipleChoiceId = q.MultipleChoiceId,
                Question = q.Question,
                CorrectAnswer = q.CorrectAnswer,
                QuizId = q.QuizId,
                QuizQuestionNum = q.QuizQuestionNum,
                Options = q.Options.Select(o => new OptionDto
                {
                    Text = o.Text,
                    IsCorrect = o.IsCorrect
                }).ToList()
            });

            return Ok(questionDtos);
        }

        [HttpPost("submitQuestion")]
        public async Task<IActionResult> SubmitQuestion([FromBody] MultipleChoiceAttemptDto multipleChoiceAttemptDto)
        {
            var multipleChoiceAttempt = new MultipleChoiceAttempt
            {
                MultiplechoiceId = multipleChoiceAttemptDto.MultipleChoiceId,
                QuizAttemptId = multipleChoiceAttemptDto.QuizAttemptId,
                UserAnswer = multipleChoiceAttemptDto.UserAnswer,
                QuizQuestionNum = multipleChoiceAttemptDto.QuizQuestionNum
            };

            var returnOk = await _multipleChoiceAttemptRepository.Create(multipleChoiceAttempt);
            if (!returnOk)
            {
                _logger.LogError("[FillInTheBlankAPIController] Question attempt creation failed {@attempt}", multipleChoiceAttempt);
                return StatusCode(500, "Internal server error");
            }

            return Ok(multipleChoiceAttempt);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] MultipleChoiceDto multipleChoiceDto)
        {
            if (multipleChoiceDto == null)
                return BadRequest("Question cannot be null");

            var question = new MultipleChoice
            {
                Question = multipleChoiceDto.Question,
                QuizId = multipleChoiceDto.QuizId,
                QuizQuestionNum = multipleChoiceDto.QuizQuestionNum,
                Options = multipleChoiceDto.Options.Select(o => new Option
                {
                    Text = o.Text,
                    IsCorrect = o.IsCorrect
                }).ToList(),
                CorrectAnswer = multipleChoiceDto.Options.FirstOrDefault(o => o.IsCorrect)?.Text
            };

            bool created = await _multipleChoiceRepository.Create(question);
            if (created)
            {
                await _quizService.ChangeQuestionCount(question.QuizId, true);
                return CreatedAtAction(nameof(GetQuestions), new { quizId = question.QuizId }, question);
            }

            _logger.LogError("[MultipleChoiceAPIController] Creation failed {@question}", question);
            return StatusCode(500, "Internal server error");
        }

        [HttpPut("update/{multipleChoiceId}")]
        public async Task<IActionResult> Edit(int multipleChoiceId, [FromBody] MultipleChoiceDto multipleChoiceDto)
        {
            if (multipleChoiceDto == null)
                return BadRequest("Question cannot be null");

            var existingQuestion = await _multipleChoiceRepository.GetById(multipleChoiceId);
            if (existingQuestion == null)
            {
                _logger.LogError("[MultipleChoiceAPIController] Question not found for id {Id}", multipleChoiceId);
                return NotFound("Question not found for the MultipleChoiceId");
            }

            existingQuestion.Question = multipleChoiceDto.Question;
            existingQuestion.QuizQuestionNum = multipleChoiceDto.QuizQuestionNum;

            existingQuestion.Options = multipleChoiceDto.Options.Select(o => new Option
            {
                Text = o.Text,
                IsCorrect = o.IsCorrect
            }).ToList();

            existingQuestion.CorrectAnswer = multipleChoiceDto.Options.FirstOrDefault(o => o.IsCorrect)?.Text;

            bool returnOk = await _multipleChoiceRepository.Update(existingQuestion);
            if (returnOk)
                return Ok(existingQuestion);

            _logger.LogError("[MultipleChoiceAPIController] Update failed {@question}", existingQuestion);
            return StatusCode(500, "Internal server error");
        }

        [HttpDelete("delete/{questionId}")]
        public async Task<IActionResult> Delete(int questionId, [FromQuery] int qNum, [FromQuery] int quizId)
        {
            bool returnOk = await _multipleChoiceRepository.Delete(questionId);

            if (!returnOk)
            {
                _logger.LogError("[MultipleChoiceAPIController] Deletion failed for questionId {QuestionId:0000}", questionId);
                return BadRequest("Question deletion failed.");
            }

            await _quizService.ChangeQuestionCount(quizId, false);
            await _quizService.UpdateQuestionNumbers(qNum, quizId);

            _logger.LogInformation("[MultipleChoiceAPIController] Deleted question Id={Id}", questionId);
            return NoContent(); 
        }
    }
}