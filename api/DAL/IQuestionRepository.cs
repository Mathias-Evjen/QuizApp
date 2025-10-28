using System.Linq.Expressions;

namespace QuizApp.DAL
{
    public interface IQuestionRepository<T> where T : class
    {
        Task<IEnumerable<T>?> GetAll();
        Task<IEnumerable<T>?> GetAll(Expression<Func<T, bool>> predicate);
        Task<T?> GetById(int id);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);
    }
}