using QuizApp.Models;

namespace QuizApp.DAL
{
    public interface IQuestionTextRepository
    {
        Task<List<QuestionText>> GetAllAsync();
        Task<QuestionText?> GetByIdAsync(int id);
        Task AddAsync(QuestionText q);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}