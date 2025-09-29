using QuizApp.Models;

namespace QuizApp.DAL;

public interface IFillInTheBlankRepository
{
    Task<IEnumerable<FillInTheBlank>?> GetAll();
    Task<FillInTheBlank?> GetQuestionById(int id);
    Task<bool> Create(FillInTheBlank fillInTheBlank);
    Task<bool> Update(FillInTheBlank fillInTheBlank);
    Task<bool> Delete(int id);

}