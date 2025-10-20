using QuizApp.Models;
using QuizApp.DAL;

namespace QuizApp.Services;

public class QuizService
{
    private readonly IQuizRepository _quizRepository;
    private readonly IFillInTheBlankRepository _fillInTheBlankRepository;
    private readonly IMatchingRepository _matchingRepository;
    private readonly IRankingRepository _rankingRepository;
    private readonly ISequenceRepository _sequenceRepository;
    private readonly ILogger<QuizService> _logger;

    public QuizService(
        IQuizRepository quizRepository,
        IFillInTheBlankRepository fillInTheBlankRepository,
        IMatchingRepository matchingRepository,
        IRankingRepository rankingRepository,
        ISequenceRepository sequenceRepository,
        ILogger<QuizService> logger)
    {
        _quizRepository = quizRepository;
        _fillInTheBlankRepository = fillInTheBlankRepository;
        _matchingRepository = matchingRepository;
        _rankingRepository = rankingRepository;
        _sequenceRepository = sequenceRepository;
        _logger = logger;
    }

    // Checks the user's answer wit the correct answer in the database
    public bool CheckAnswer(string correctAnswer, string userAnswer)
    {
        return string.Equals(
            userAnswer?.Trim(),
            correctAnswer,
            StringComparison.OrdinalIgnoreCase
            );
    }

    public bool CheckAnswer(bool correctAnswer, bool userAnswer)
    {
        return correctAnswer == userAnswer;
    }

    public async Task ChangeQuestionCount(int quizId, bool increment)
    {
        var quiz = await _quizRepository.GetQuizById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[FlashCardQuizService] FlashCardQuiz not found for the Id {Id: 0000}", quizId);
            return;
        }

        if (increment) quiz.NumOfQuestions += 1;
        else quiz.NumOfQuestions -= 1;

        bool returnOk = await _quizRepository.UpdateQuiz(quiz);
        if (!returnOk)
        {
            _logger.LogError("[FlashCardQuizService] FlashCardQuiz update failed for {@quiz}", quiz);
        }
    }

    public async Task UpdateQuestionNumbers(int qNum, int quizId)
    {
        var quiz = await _quizRepository.GetQuizById(quizId);
        if (quiz == null)
        {
            _logger.LogError("[FlashCardQuizService] FlashCardQuiz not found for the Id {Id: 0000}", quizId);
            return;
        }

        foreach (var question in quiz.AllQuestions.OrderByDescending(q => q.QuizQuestionNum))
        {
            if (question.QuizQuestionNum < qNum) continue;
            var returnOk = await UpdateQuizQuestionNumbers(question);
            if (!returnOk) break;
        }
    }

    public async Task<bool> UpdateQuizQuestionNumbers(Question question)
    {
        // Finds the question-model, decrements QuizQuestionNum, and updates the database

        if (question is FillInTheBlank fib)
        {
            fib.QuizQuestionNum -= 1;
            return await _fillInTheBlankRepository.UpdateQuestion(fib);
        }

        if (question is Matching m)
        {
            m.QuizQuestionNum -= 1;
            return await _matchingRepository.UpdateMatching(m);
        }

        if (question is Sequence sq)
        {
            sq.QuizQuestionNum -= 1;
            return await _sequenceRepository.UpdateSequence(sq);
        }

        if (question is Ranking r)
        {
            r.QuizQuestionNum -= 1;
            return await _rankingRepository.UpdateRanking(r);
        }
        return false;
    }
}