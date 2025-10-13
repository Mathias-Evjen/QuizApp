using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    public class MultipleChoiceViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        public string QuestionText { get; set; } = string.Empty;

        public List<Option> Options { get; set; } = new();

        public List<string> NewOptions { get; set; } = new();

        public List<int> CorrectOptionIndexes { get; set; } = new();
    }
}
