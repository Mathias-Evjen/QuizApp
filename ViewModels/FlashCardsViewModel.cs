using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class FlashCardsViewModel
    {
        public int CurrentFlashCardNum { get; set; } = 0;
        public IEnumerable<FlashCard> FlashCards { get; set; } = new List<FlashCard>();

        public FlashCardsViewModel() { }

        public FlashCardsViewModel(IEnumerable<FlashCard> flashCards)
        {
            FlashCards = flashCards;
        }
    }
}