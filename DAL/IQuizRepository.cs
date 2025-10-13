using QuizApp.Models;

namespace QuizApp.DAL;

public interface IQuizRepository
{
    Task<IEnumerable<Quiz>?> GetAll();
    Task<Quiz?> GetQuizById(int id);
    Task<bool> CreateQuiz(Quiz Quiz);
    Task<bool> UpdateQuiz(Quiz Quiz);
    Task<bool> DeleteQuiz(int id);
}