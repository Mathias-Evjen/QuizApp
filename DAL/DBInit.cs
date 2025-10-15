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

        if (!context.Quizzes.Any())
        {
            var quiz = new List<Quiz>
            {
                new Quiz {Name = "Example quiz", Description = "Quiz with examples"},
                new Quiz {Name = "Also example quiz", Description = "This is also a quiz with examples"}
            };
            context.AddRange(quiz);
            context.SaveChanges();
        }

        // if (!context.MatchingQuestions.Any())
        // {
        //     var questions = new List<Matching>
        //     {
        //         new Matching {
        //             Question = "fotball,kake,hjul,knotter,bake,hav,bade,bil",
        //             CorrectAnswer = "fotball,knotter,hjul,bil,bake,kake,bade,hav",
        //             QuizId = 1
        //         },
        //         new Matching {
        //             Question = "sol,snø,tre,planet,vinter,blader",
        //             CorrectAnswer = "sol,planet,tre,blader,vinter,snø",
        //             QuizId = 2
        //         }
        //     };
        //     context.AddRange(questions);
        //     context.SaveChanges();
        // }
        if (!context.FillInTheBlankQuestions.Any())
        {
            var quiz1 = context.Quizzes.Find(1);
            var questions = new List<FillInTheBlank>
            {
                new FillInTheBlank {
                    Question = "What is the capital of Norway?",
                    Answer = "Oslo",
                    QuizId = 1,
                    Quiz = quiz1,
                    QuizQuestionNum = 1
                },
                new FillInTheBlank {
                    Question = "What is the capital of Sweden?",
                    Answer = "Stockholm",
                    QuizId = 1,
                    Quiz = quiz1,
                    QuizQuestionNum = 2
                }
            };
            context.AddRange(questions);
            context.SaveChanges();
        }
        // if (!context.SequenceQuestions.Any())
        // {
        //     var questions = new List<Sequence>
        //     {
        //         new Sequence {
        //             Question = "Make an offer,Get pre-approved,Sign the purchase agreement,Find a home,Secure financing",
        //             QuestionText = "Order the following steps of applying for a home loan from first to last:",
        //             CorrectAnswer = "Get pre-approved,Find a home,Make an offer,Sign the purchase agreement,Secure financing",
        //             QuizId = 1
        //         },
        //         new Sequence {
        //             Question = "1801,1345,1814,2009,1918",
        //             QuestionText = "Sequence these years from earliest to latest:",
        //             CorrectAnswer = "1345,1801,1814,1918,2009",
        //             QuizId = 1
        //         }
        //     };
        //     context.AddRange(questions);
        //     context.SaveChanges();
        // }
        // if (!context.RankingQuestions.Any())
        // {
        //     var questions = new List<Ranking>
        //     {
        //         new Ranking {
        //             Question = "Service speed,Price,Ambiance,Food quality",
        //             QuestionText = "Rank these factors from most to least important for a dining experience:",
        //             CorrectAnswer = "Food quality,Service speed,Ambiance,Price",
        //             QuizId = 1
        //         },
        //         new Ranking {
        //             Question = "Location,Cleanliness,Price,Amenities",
        //             QuestionText = "Rank these features from most to least important when choosing a hotel:",
        //             CorrectAnswer = "Cleanliness,Location,Price,Amenities",
        //             QuizId = 1
        //         }
        //     };
        //     context.AddRange(questions);
        //     context.SaveChanges();
        // }

        
        if (!context.FlashCardQuizzes.Any())
        {
            var quiz = new List<FlashCardQuiz>
            {
                new FlashCardQuiz {Name = "Capitals of Scandinavia", Description = "Flashcards with questions about the captial cities of Scandinavia"},
                new FlashCardQuiz {Name = "Which band?", Description = "Which band wrote the song on the card?"}
            };
            context.AddRange(quiz);
            context.SaveChanges();
        }

        if (!context.FlashCards.Any())
        {
            var quiz1 = context.FlashCardQuizzes.Find(1);
            var quiz2 = context.FlashCardQuizzes.Find(2);
            var questions = new List<FlashCard>
            {
                new FlashCard {
                    Question = "What is the capital of Norway?",
                    Answer = "Oslo",
                    Quiz = quiz1!,
                    QuizQuestionNum = 1
                },
                new FlashCard {
                    Question = "What is the capital of Sweden?",
                    Answer = "Stockholm",
                    Quiz = quiz1!,
                    QuizQuestionNum = 2
                },
                new FlashCard {
                    Question = "What is the capital of Denmark?",
                    Answer = "Copenhagen",
                    Quiz = quiz1!,
                    QuizQuestionNum = 3
                },
                new FlashCard {
                    Question = "Who wrote the song 'Supermassive Black Hole'?",
                    Answer = "Muse",
                    Quiz = quiz2!,
                    QuizQuestionNum = 1
                },
                new FlashCard {
                    Question = "Who wrote the song 'Paranoid Android'?",
                    Answer = "Radiohead",
                    Quiz = quiz2!,
                    QuizQuestionNum = 2
                },
                new FlashCard {
                    Question = "Who wrote the song 'Somewhere Only We know'?",
                    Answer = "Keane",
                    Quiz = quiz2!,
                    QuizQuestionNum = 3
                },
                new FlashCard {
                    Question = "Who wrote the song 'Do I Wanna Know'?",
                    Answer = "Arctic Monkeys",
                    Quiz = quiz2!,
                    QuizQuestionNum = 4
                },
                new FlashCard {
                    Question = "Who wrote the song 'Ode To The Mets'?",
                    Answer = "The Strokes",
                    Quiz = quiz2!,
                    QuizQuestionNum = 5
                }
            };

            context.AddRange(questions);
            context.SaveChanges();
        }
        var fcQuizzes = context.FlashCardQuizzes.Include(fc => fc.FlashCards);
        foreach(var quiz in fcQuizzes)
        {
            quiz.NumOfQuestions = quiz.FlashCards != null ? quiz.FlashCards.Count : 0;
        }
        context.SaveChanges();

        var quizzes = context.Quizzes.Include(q => q.FillInTheBlankQuestions).ToList();
        foreach (var quiz in quizzes)
        {
            quiz.NumOfQuestions = quiz.AllQuestions != null ? quiz.AllQuestions.Count() : 0;
        }
        context.SaveChanges();
    }
}
