public class TrueFalseDto
{
    public int TrueFalseId { get; set; }
    public string Question { get; set; } = string.Empty;
    public bool CorrectAnswer { get; set; }
    public int QuizId { get; set; }
    public int QuizQuestionNum { get; set; }
}
