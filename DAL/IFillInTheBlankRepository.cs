using QuizApp.Models;

namespace QuizApp.DAL;

public interface IFillInTheBlankRepository
{
    Task<IEnumerable<FillInTheBlank>?> GetAll();
    Task<FillInTheBlank?> GetQuestionById(int id);
    Task<bool> CreateQuestion(FillInTheBlank fillInTheBlank);
    Task<bool> UpdateQuestion(FillInTheBlank fillInTheBlank);
    Task<bool> DeleteQuestion(int id);

}