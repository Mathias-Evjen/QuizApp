using QuizApp.Models;

namespace QuizApp.DAL
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetAllAsync();
        Task<Question?> GetByIdAsync(int id);
        Task AddAsync(Question q);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}