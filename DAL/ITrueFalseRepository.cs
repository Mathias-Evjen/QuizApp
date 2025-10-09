using QuizApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizApp.DAL
{
    public interface ITrueFalseRepository
    {
        Task<List<TrueFalseQuestion>> GetAllAsync();
        Task<TrueFalseQuestion?> GetByIdAsync(int id);
        Task AddAsync(TrueFalseQuestion q);
        Task UpdateAsync(TrueFalseQuestion q);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
