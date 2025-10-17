using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class MultipleChoiceViewModel : QuestionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        public string QuestionText { get; set; } = string.Empty;

        public List<Option> Options { get; set; } = new();

        public List<string> NewOptions { get; set; } = new();

        public List<int> CorrectOptionIndexes { get; set; } = new();

        public int QuizId { get; set; }
        public int QuizQuestionNum { get; set; }

        // Brukes når en bruker svarer på spørsmålet i quiz
        public List<int> SelectedAnswers { get; set; } = new();

        public MultipleChoiceViewModel() { }

        public MultipleChoiceViewModel(MultipleChoice question)
        {
            Id = question.Id;
            QuestionText = question.QuestionText;
            Options = question.Options ?? new List<Option>();
        }
    }
}
