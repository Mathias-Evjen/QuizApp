using QuizApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizApp.DAL
{
    public interface ITrueFalseRepository
    {
        Task<List<TrueFalse>> GetAllAsync();
        Task<TrueFalse?> GetByIdAsync(int id);
        Task<TrueFalse?> GetDetailedAsync(int id);
        Task AddAsync(TrueFalse question);
        Task UpdateAsync(TrueFalse question);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
