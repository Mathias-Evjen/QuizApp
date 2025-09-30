using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class FlashCardsViewModel
    {
        public IEnumerable<FlashCard> FlashCards;

        public FlashCardsViewModel(IEnumerable<FlashCard> flashCards)
        {
            FlashCards = flashCards;
        }
    }
}