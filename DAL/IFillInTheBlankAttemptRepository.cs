using QuizApp.Models;

namespace QuizApp.DAL;

public interface IFillInTheBlankAttemptRepository
{
    Task<FillInTheBlankAttempt?> GetFillInTheBlankAttemptById(int id);
    Task<bool> CreateFillInTheBlankAttempt(FillInTheBlankAttempt fillInTheBlankAttempt);
    Task<bool> UpdateFillInTheBlankAttempt(FillInTheBlankAttempt fillInTheBlankAttempt);
    Task<bool> DeleteFillInTheBlankAttempt(int id);
}