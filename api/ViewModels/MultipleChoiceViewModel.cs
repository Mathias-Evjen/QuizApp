using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class MultipleChoiceViewModel : QuestionViewModel
    {
        public int MultipleChoiceId { get; set; }
        public int QuizQuestionNum { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        public string QuestionText { get; set; } = string.Empty;

        public List<Option> Options { get; set; } = [];

        public List<string> OptionAnswers { get; set; } = [];

        public List<int> CorrectOptionIndexes { get; set; } = [];

        public string UserAnswer { get; set; } = string.Empty;

        public MultipleChoiceViewModel() { }

        public MultipleChoiceViewModel(MultipleChoice question)
        {
            MultipleChoiceId = question.MultipleChoiceId;
            QuestionText = question.QuestionText;
            QuizQuestionNum = question.QuizQuestionNum;
            Options = question.Options;
        }
    }
}
