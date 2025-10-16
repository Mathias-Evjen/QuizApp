using QuizApp.Models;

namespace QuizApp.DAL;

public interface ITrueFalseRepository
{
    Task<IEnumerable<TrueFalseQuestion>?> GetAll();
    Task<TrueFalseQuestion?> GetById(int id);
    Task<bool> Create(TrueFalseQuestion question);
    Task<bool> Update(TrueFalseQuestion question);
    Task<bool> Delete(int id);
}

