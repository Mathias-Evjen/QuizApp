using QuizApp.Models;

namespace QuizApp.DAL
{
    public interface IMultipleChoiceRepository
    {
        Task<List<MultipleChoice>> GetAllWithOptionsAsync();
        Task<MultipleChoice?> GetWithOptionsAsync(int id);
        Task AddAsync(MultipleChoice q);
        Task UpdateAsync(MultipleChoice q);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}