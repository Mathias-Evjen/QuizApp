using QuizApp.Models;

namespace QuizApp.DAL;

public interface IQuizAttemptRepository
{
    Task<IEnumerable<QuizAttempt>?> GetAll();
    Task<QuizAttempt?> GetQuizAttemptById(int id);
    Task<bool> CreateQuizAttempt(QuizAttempt QuizAttempt);
    Task<bool> UpdateQuizAttempt(QuizAttempt QuizAttempt);
    Task<bool> DeleteQuizAttempt(int id);
}