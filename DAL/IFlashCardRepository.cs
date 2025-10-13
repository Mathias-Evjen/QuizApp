using QuizApp.Models;

namespace QuizApp.DAL;

public interface IFlashCardRepository
{
    Task<IEnumerable<FlashCard>?> GetAll(int id);
    Task<FlashCard?> GetFlashCardById(int id);
    Task<bool> CreateFlashCard(FlashCard flashCard);
    Task<bool> UpdateFlashCard(FlashCard flashCard);
    Task<bool> DeleteFlashCard(int id);
}