using QuizApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizApp.DAL
{
    public interface IMultipleChoiceRepository
    {
        Task<List<MultipleChoice>> GetAllAsync();
        Task<MultipleChoice?> GetByIdAsync(int id);
        Task<MultipleChoice?> GetDetailedAsync(int id);
        Task AddAsync(MultipleChoice question);
        Task UpdateAsync(MultipleChoice question);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
