using QuizApp.Models;

namespace QuizApp.DAL
{
    public interface IMultipleChoiceRepository
    {
        Task<IEnumerable<MultipleChoice>?> GetAll();
        Task<MultipleChoice?> GetById(int id);
        Task<bool> Create(MultipleChoice question);
        Task<bool> Update(MultipleChoice question);
        Task<bool> Delete(int id);
    }
}
