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
                    Question = "Make an offer,Get pre-approved,Sign the purchase agreement,Find a home,Secure financing",
                    QuestionText = "Order the following steps of applying for a home loan from first to last:",
                    CorrectAnswer = "Get pre-approved,Find a home,Make an offer,Sign the purchase agreement,Secure financing"
                },
                new Sequence {
                    Question = "1801,1345,1814,2009,1918",
                    QuestionText = "Sequence these years from earliest to latest:",
                    CorrectAnswer = "1345,1801,1814,1918,2009"
                }
            };
            context.AddRange(questions);
            context.SaveChanges();
        }
        if (!context.RankingQuestions.Any())
        {
            var questions = new List<Ranking>
            {
                new Ranking {
                    Question = "Service speed,Price,Ambiance,Food quality",
                    QuestionText = "Rank these factors from most to least important for a dining experience:",
                    CorrectAnswer = "Food quality,Service speed,Ambiance,Price"
                },
                new Ranking {
                    Question = "Location,Cleanliness,Price,Amenities",
                    QuestionText = "Rank these features from most to least important when choosing a hotel:",
                    CorrectAnswer = "Cleanliness,Location,Price,Amenities"
                }
            };
            context.AddRange(questions);
            context.SaveChanges();
        }
        context.SaveChanges();
    }
}