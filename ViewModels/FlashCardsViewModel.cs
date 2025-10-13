using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class FlashCardsViewModel
    {
        public int CurrentFlashCardNum { get; set; } = 0;
        public IEnumerable<FlashCard> FlashCards { get; set; } = new List<FlashCard>();

        public string FlashCardQuizName { get; set; } = string.Empty;
        public string FlashCardQuizDescription { get; set; } = string.Empty;

        public FlashCardsViewModel() { }

        public FlashCardsViewModel(IEnumerable<FlashCard> flashCards, string flashCardquizName, string flashCardQuizDescription)
        {
            FlashCards = flashCards.ToList();
            FlashCardQuizName = flashCardquizName;
            FlashCardQuizDescription = flashCardQuizDescription;
        }
    }
}