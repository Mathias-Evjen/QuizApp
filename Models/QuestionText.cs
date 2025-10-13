using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public abstract class QuestionText
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        [StringLength(200, ErrorMessage = "Question must be under 200 characters long.")]
        public string QuestionTexts { get; set; } = string.Empty;
    }
}