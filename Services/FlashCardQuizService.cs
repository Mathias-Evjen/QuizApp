using QuizApp.Models;
using QuizApp.DAL;
using QuizApp.Controllers;

namespace QuizApp.Services;

public class FlashCardQuizService : IFlashCardQuizService
{
    private readonly IFlashCardQuizRepository _flashCardQuizRepository;
    private readonly IFlashCardRepository _flashCardRepository;
    private readonly ILogger<FlashCardQuizService> _logger;

    public FlashCardQuizService(IFlashCardQuizRepository flashCardQuizRepository, IFlashCardRepository flashCardRepository, ILogger<FlashCardQuizService> logger)
    {
        _flashCardQuizRepository = flashCardQuizRepository;
        _flashCardRepository = flashCardRepository;
        _logger = logger;
    }

    public async Task IncQuestionCounter(int quizId)
    {
        var quiz = await _flashCardQuizRepository.GetFlashCardQuizById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[FlashCardQuizService] FlashCardQuiz not found for the Id {Id: 0000}", quizId);
            return;
        }

        quiz.NumOfQuestions += 1;
        bool returnOk = await _flashCardQuizRepository.UpdateFlashCardQuiz(quiz);
        if (!returnOk)
        {
            _logger.LogError("[FlashCardQuizService] FlashCardQuiz update failed for {@quiz}", quiz);
        }
    }

    public async Task DecQuestionCounter(int quizId)
    {
        var quiz = await _flashCardQuizRepository.GetFlashCardQuizById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[FlashCardQuizService] FlashCardQuiz not found for the Id {Id: 0000}", quizId);
            return;
        }

        quiz.NumOfQuestions -= 1;
        bool returnOk = await _flashCardQuizRepository.UpdateFlashCardQuiz(quiz);
        if (!returnOk)
        {
            _logger.LogError("[FlashCardQuizService] FlashCardQuiz update failed for {@quiz}", quiz);
        }
    }

    public async Task UpdateFlashCardQuestionNumbers(int qNum, int quizId)
    {
        var quiz = await _flashCardQuizRepository.GetFlashCardQuizById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[FlashCardQuizService] FlashCardQuiz not found for the Id {Id: 0000}", quizId);
            return;
        }

        foreach (var flashCard in quiz.FlashCards!)
        {
            if (flashCard.QuizQuestionNum < qNum) continue;
            flashCard.QuizQuestionNum -= 1;
            bool returnOk = await _flashCardRepository.UpdateFlashCard(flashCard);
            if (!returnOk) break;
        }
    }
}