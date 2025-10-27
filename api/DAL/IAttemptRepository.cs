namespace QuizApp.DAL
{
    public interface IAttemptRepository<T> where T : class
    {
        Task<T?> GetById(int id);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);
        bool Exists(int id);
    }
}