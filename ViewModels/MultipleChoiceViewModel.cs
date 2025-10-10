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

        // For å vise/endre eksisterende alternativer
        public List<Option> Options { get; set; } = new();

        // For å legge til nye alternativer i Create/Edit
        public List<string> NewOptions { get; set; } = new();

        // For å markere riktige alternativer
        public List<int> CorrectOptionIndexes { get; set; } = new();
    }
}
