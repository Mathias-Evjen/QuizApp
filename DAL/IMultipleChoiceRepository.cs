using QuizApp.Models;

namespace QuizApp.DAL
{
    public interface IMultipleChoiceRepository
    {
        Task<List<MultipleChoice>> GetAllAsync();
        Task<MultipleChoice?> GetDetailedAsync(int id);
        Task AddAsync(MultipleChoice question);
        Task UpdateAsync(MultipleChoice question);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
