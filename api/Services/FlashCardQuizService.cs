using QuizApp.Models;
using QuizApp.DAL;
using QuizApp.Controllers;

namespace QuizApp.Services;

public class FlashCardQuizService : IFlashCardQuizService
{
    private readonly IRepository<FlashCardQuiz> _flashCardQuizRepository;
    private readonly IFlashCardRepository _flashCardRepository;
    private readonly ILogger<FlashCardQuizService> _logger;

    public FlashCardQuizService(IRepository<FlashCardQuiz> flashCardQuizRepository, IFlashCardRepository flashCardRepository, ILogger<FlashCardQuizService> logger)
    {
        _flashCardQuizRepository = flashCardQuizRepository;
        _flashCardRepository = flashCardRepository;
        _logger = logger;
    }

    public async Task ChangeQuestionCount(int quizId, bool increment)
    {
        var quiz = await _flashCardQuizRepository.GetById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[FlashCardQuizService] FlashCardQuiz not found for the Id {Id: 0000}", quizId);
            return;
        }

        if (increment) quiz.NumOfQuestions += 1;
        else quiz.NumOfQuestions -= 1;

        bool returnOk = await _flashCardQuizRepository.Update(quiz);
        if (!returnOk)
        {
            _logger.LogError("[FlashCardQuizService] FlashCardQuiz update failed for {@quiz}", quiz);
        }
    }

    public async Task UpdateFlashCardQuestionNumbers(int qNum, int quizId)
    {
        var quiz = await _flashCardQuizRepository.GetById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[FlashCardQuizService] FlashCardQuiz not found for the Id {Id: 0000}", quizId);
            return;
        }

        foreach (var flashCard in quiz.FlashCards!)
        {
            if (flashCard.QuizQuestionNum < qNum) continue;
            flashCard.QuizQuestionNum -= 1;
            bool returnOk = await _flashCardRepository.Update(flashCard);
            if (!returnOk) break;
        }
    }

    // Flashcard dectoration
    public string PickRandomFlashCardColor()
    {
        var flashCardColors = new List<string>{
            "#FFF9C4", "#FFE0B2", "#F8BBD0",
            "#FFCCBC", "#E1BEE7", "#B3E5FC",
            "#C8E6C9", "#FFF3E0", "#FFDAB9"
        };

        var random = new Random();
        int randomIndex = random.Next(flashCardColors.Count);
        return flashCardColors[randomIndex];
    }
}