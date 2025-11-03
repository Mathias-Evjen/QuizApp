using System.Linq.Expressions;

namespace QuizApp.DAL
{
    public interface IAttemptRepository<T> where T : class
    {
        Task<IEnumerable<T>?> GetAll(Expression<Func<T, bool>> predicate);
        Task<T?> GetById(int id);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);
        bool Exists(Expression<Func<T, bool>> predicate);
    }
}