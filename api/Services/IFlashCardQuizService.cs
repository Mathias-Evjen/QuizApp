namespace QuizApp.Services;

public interface IFlashCardQuizService
{
    Task ChangeQuestionCount(int quizId, bool increment);
    Task UpdateFlashCardQuestionNumbers(int qNum, int quizId);
    string PickRandomFlashCardColor();
}