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

        if (!context.MatchingQuestions.Any())
        {
            var questions = new List<Matching>
            {
                new Matching {
                    Question = "fotball,kake,hjul,knotter,bake,hav,bade,bil",
                    CorrectAnswer = "fotball,knotter,hjul,bil,bake,kake,bade,hav"
                },
                new Matching {
                    Question = "sol,snø,tre,planet,vinter,blader",
                    CorrectAnswer = "sol,planet,tre,blader,vinter,snø"
                }
            };
            context.AddRange(questions);
            context.SaveChanges();
        }
        if (!context.SequenceQuestions.Any())
        {
            var questions = new List<Sequence>
            {
                new Sequence {
                    Question = "1945,1911,1970,2011",
                    CorrectAnswer = "1911,1945,1970,2011"
                },
                new Sequence {
                    Question = "1801,1345,1814,2009,1918",
                    CorrectAnswer = "1345,1801,1814,1918,2009"
                }
            };
            context.AddRange(questions);
            context.SaveChanges();
        }
        context.SaveChanges();
    }
}