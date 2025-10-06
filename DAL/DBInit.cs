using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL;

public static class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        QuizDbContext context = serviceScope.ServiceProvider.GetRequiredService<QuizDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if (!context.FillInTheBlankQuestions.Any())
        {
            var questions = new List<FillInTheBlank>
            {
                new FillInTheBlank {
                    Question = "What is the capital of Norway?",
                    Answer = "Oslo"
                },
                new FillInTheBlank {
                    Question = "What is the capital of Sweden?",
                    Answer = "Stockholm"
                }
            };
            context.AddRange(questions);
            context.SaveChanges();
        }

        
        if (!context.FlashCardQuizzes.Any())
        {
            var quiz = new List<FlashCardQuiz>
            {
                new FlashCardQuiz {Name = "Capitals of Scandinavia", Description = "Flashcards with questions about the captial cities of Scandinavia"}
            };
            context.AddRange(quiz);
            context.SaveChanges();
        }

        if (!context.FlashCards.Any())
        {
            var quiz = context.FlashCardQuizzes.Find(1);
            var questions = new List<FlashCard>
            {
                new FlashCard {
                    Question = "What is the capital of Norway?",
                    Answer = "Oslo",
                    Quiz = quiz!
                },
                new FlashCard {
                    Question = "What is the capital of Sweden?",
                    Answer = "Stockholm",
                    Quiz = quiz!
                },
                new FlashCard {
                    Question = "What is the capital of Denmark?",
                    Answer = "Copenhagen",
                    Quiz = quiz!
                }
            };
            context.AddRange(questions);
            context.SaveChanges();
        }
        var quizzes = context.FlashCardQuizzes.Include(fc => fc.FlashCards);
        foreach(var quiz in quizzes)
        {
            quiz.NumOfQuestions = quiz.FlashCards != null ? quiz.FlashCards.Count : 0;
        }
        context.SaveChanges();
    }
}