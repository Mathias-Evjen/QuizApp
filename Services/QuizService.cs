using QuizApp.Models;
using QuizApp.DAL;
using QuizApp.Controllers;

namespace QuizApp.Services;

public class QuizService
{
    private readonly IFillInTheBlankRepository _fillInTheBlankRepository;
    private readonly ILogger<QuizService> _logger;

    public QuizService(IFillInTheBlankRepository fillInTheBlankRepository, ILogger<QuizService> logger)
    {
        _fillInTheBlankRepository = fillInTheBlankRepository;
        _logger = logger;
    }

    public async Task<bool> CheckAnswer(FillInTheBlank question, string userAnswer)
    {
        return string.Equals(
            userAnswer?.Trim(),
            question.Answer,
            StringComparison.OrdinalIgnoreCase
        );
    }
}