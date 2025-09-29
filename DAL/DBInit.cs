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
    }
}