using System.ComponentModel.DataAnnotations;

namespace QuizApp.DTOs
{
    public class QuizAttemptDto
    {
        public int QuizAttemptId { get; set; }
        public int QuizId { get; set; }

        public int NumOfCorrectAnswers { get; set; }

        //TODO: Må kunne sende alle quiz typer attempts
    }
}