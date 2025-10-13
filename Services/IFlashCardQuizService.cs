namespace QuizApp.Services;

public interface IFlashCardQuizService
{
    Task IncQuestionCounter(int quizId);
    Task DecQuestionCounter(int quizId);
    Task UpdateFlashCardQuestionNumbers(int qNum, int quizId);
}