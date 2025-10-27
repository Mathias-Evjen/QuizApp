using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class FlashCardsViewModel
    {
        public int CurrentFlashCardNum { get; set; } = 0;
        public IEnumerable<FlashCard> FlashCards { get; set; } = [];

        public string FlashCardQuizName { get; set; } = string.Empty;
        public string FlashCardQuizDescription { get; set; } = string.Empty;

        public FlashCardsViewModel() { }

        public FlashCardsViewModel(IEnumerable<FlashCard> flashCards, string flashCardquizName, string flashCardQuizDescription)
        {
            FlashCards = [.. flashCards];
            FlashCardQuizName = flashCardquizName;
            FlashCardQuizDescription = flashCardQuizDescription;
        }
    }
}