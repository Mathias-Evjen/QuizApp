using QuizApp.Models;
using QuizApp.DAL;

namespace QuizApp.Services
{
    public class QuizService(
        IQuizRepository<Quiz> quizRepository,
        IQuestionRepository<FillInTheBlank> fillInTheBlankRepository,
        IQuestionRepository<Matching> matchingRepository,
        IQuestionRepository<Ranking> rankingRepository,
        IQuestionRepository<Sequence> sequenceRepository,
        IQuestionRepository<MultipleChoice> multipleChoiceRepository,
        IQuestionRepository<TrueFalse> trueFalseRepository,
        ILogger<QuizService> logger)
    {
        private readonly IQuizRepository<Quiz> _quizRepository = quizRepository;
        private readonly IQuestionRepository<FillInTheBlank> _fillInTheBlankRepository = fillInTheBlankRepository;
        private readonly IQuestionRepository<Matching> _matchingRepository = matchingRepository;
        private readonly IQuestionRepository<Ranking> _rankingRepository = rankingRepository;
        private readonly IQuestionRepository<Sequence> _sequenceRepository = sequenceRepository;
        private readonly IQuestionRepository<MultipleChoice> _multipleChoiceRepository = multipleChoiceRepository;
        private readonly IQuestionRepository<TrueFalse> _trueFalseRepository = trueFalseRepository;
        private readonly ILogger<QuizService> _logger = logger;

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
            var quiz = await _quizRepository.GetById(quizId);
            if (quiz == null)
            {
                _logger.LogError("[QuizService] Quiz not found for the Id {Id: 0000}", quizId);
                return;
            }

            if (increment) quiz.NumOfQuestions += 1;
            else quiz.NumOfQuestions -= 1;

            bool returnOk = await _quizRepository.Update(quiz);
            if (!returnOk)
            {
                _logger.LogError("[QuizService] Quiz update failed for {@quiz}", quiz);
            }
        }

        public async Task UpdateQuestionNumbers(int qNum, int quizId)
        {
            var quiz = await _quizRepository.GetById(quizId);
            if (quiz == null)
            {
                _logger.LogError("[QuizService] Quiz not found for the Id {Id: 0000}", quizId);
                return;
            }

            foreach (var question in quiz.AllQuestions.OrderByDescending(q => q.QuizQuestionNum))
            {
                if (question.QuizQuestionNum < qNum) continue;
                var returnOk = await UpdateQuizQuestionNumbers(question);
                if (!returnOk) break;
            }
        }

        private async Task<bool> UpdateQuizQuestionNumbers(Question question)
        {
            // Finds the question-model, decrements QuizQuestionNum, and updates the database

            if (question is FillInTheBlank fib)
            {
                fib.QuizQuestionNum -= 1;
                return await _fillInTheBlankRepository.Update(fib);
            }

            if (question is Matching m)
            {
                m.QuizQuestionNum -= 1;
                return await _matchingRepository.Update(m);
            }

            if (question is Sequence sq)
            {
                sq.QuizQuestionNum -= 1;
                return await _sequenceRepository.Update(sq);
            }

            if (question is Ranking r)
            {
                r.QuizQuestionNum -= 1;
                return await _rankingRepository.Update(r);
            }

            if (question is MultipleChoice mc)
            {
                mc.QuizQuestionNum -= 1;
                return await _multipleChoiceRepository.Update(mc);
            }

            if (question is TrueFalse tf)
            {
                tf.QuizQuestionNum -= 1;
                return await _trueFalseRepository.Update(tf);
            }
            return false;
        }
    }
}