

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
 public class Sequence
    {
        [Key] // Marker dette som primærnøkkel
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
    }   
}