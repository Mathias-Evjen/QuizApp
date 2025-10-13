using QuizApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizApp.DAL
{
    public interface ITrueFalseRepository
    {
        Task<List<TrueFalseQuestion>> GetAllAsync();
        Task<TrueFalseQuestion?> GetByIdAsync(int id);
        Task<TrueFalseQuestion?> GetDetailedAsync(int id);
        Task AddAsync(TrueFalseQuestion question);
        Task UpdateAsync(TrueFalseQuestion question);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
