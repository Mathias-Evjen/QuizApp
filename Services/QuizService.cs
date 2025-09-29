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

    // Checks the user's answer wit the correct answer in the database
    public bool CheckAnswer(FillInTheBlank question, string userAnswer)
    {
        return string.Equals(
            userAnswer?.Trim(),
            question.Answer,
            StringComparison.OrdinalIgnoreCase
        );
    }
}