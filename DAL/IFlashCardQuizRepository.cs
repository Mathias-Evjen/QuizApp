using QuizApp.Models;

namespace QuizApp.DAL;

public interface IFlashCardQuizRepository 
{
    Task<IEnumerable<FlashCardQuiz>?> GetAll();
    Task<FlashCardQuiz?> GetFlashCardQuizById(int id);
    Task<bool> CreateFlashCardQuiz(FlashCardQuiz quiz);
    Task<bool> UpdateFlashCardQuiz(FlashCardQuiz quiz);
    Task<bool> DeleteFlashCardQuiz(int id);
}