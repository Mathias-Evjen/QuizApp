using QuizApp.Models;

namespace QuizApp.DAL
{
    public interface ITrueFalseRepository
    {
        Task<IEnumerable<TrueFalse>?> GetAll();
        Task<TrueFalse?> GetById(int id);
        Task<bool> Create(TrueFalse question);
        Task<bool> Update(TrueFalse question);
        Task<bool> Delete(int id);
    }
}
