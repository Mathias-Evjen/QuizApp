using QuizApp.Models;
using QuizApp.DAL;
using QuizApp.Controllers;

namespace QuizApp.Services;

public class FlashCardQuizService
{
    private readonly IFlashCardQuizRepository _flashCardQuizRepository;
    private readonly ILogger<FlashCardQuizService> _logger;

    public FlashCardQuizService(IFlashCardQuizRepository flashCardQuizRepository, ILogger<FlashCardQuizService> logger)
    {
        _flashCardQuizRepository = flashCardQuizRepository;
        _logger = logger;
    }

    public async Task<bool> UpdateQuestionCounter(int quizId)
    {
        var quiz = await _flashCardQuizRepository.GetFlashCardQuizById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[FlashCardQuizService] FlashCardQuiz not found for the Id {Id: 0000}", quizId);
            return false;
        }

        quiz.NumOfQuestions += 1;
        bool returnOk = await _flashCardQuizRepository.UpdateFlashCardQuiz(quiz);
        if (!returnOk)
        {
            _logger.LogError("[FlashCardQuizService] FlashCardQuiz update failed for {@quiz}", quiz);
            return false;
        }
        return true;       
    }
}