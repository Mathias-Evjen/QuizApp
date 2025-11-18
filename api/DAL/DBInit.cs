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
                new() {Name = "Example quiz", Description = "Quiz with examples"},
                new() {Name = "Also example quiz", Description = "This is also a quiz with examples"},
                new() {Name = "True/False and MultipleChoice test", Description = "Test for True/False og MultipleChoice questions"},
                new() {Name = "QuizResults test", Description = "Bla bla bla"}
            };
            context.AddRange(quiz);
            context.SaveChanges();
        }

        if (!context.TrueFalseQuestions.Any())
        {
            var quiz = context.Quizzes.Find(3);
            var qustions = new List<TrueFalse>
            {
                new() {
                    Question = "Norge ligger i Europa",
                    CorrectAnswer = true,
                    QuizId = 3,
                    Quiz = quiz!,
                    QuizQuestionNum = 1
                },
                new() {
                    Question = "Jorda går i bane rundt månen",
                    CorrectAnswer = false,
                    QuizId = 3,
                    Quiz = quiz!,
                    QuizQuestionNum = 2
                }
            };
            context.AddRange(qustions);
            context.SaveChanges();
        }

        if (!context.MultipleChoiceQuestions.Any())
        {
            var quiz = context.Quizzes.Find(3);
            var questions = new List<MultipleChoice>
            {
                new() {
                    Question = "Which is the biggest planet in our solar system?",
                    CorrectAnswer = "Jupiter",
                    QuizId = 3,
                    Quiz = quiz!,
                    QuizQuestionNum = 3
                },
                new() {
                    Question = "Which season is warmest?",
                    CorrectAnswer = "Summer",
                    QuizId = 3,
                    Quiz = quiz!,
                    QuizQuestionNum = 4
                }
            };
            context.AddRange(questions);
            context.SaveChanges();
        }

        if (!context.Options.Any())
        {
            var options = new List<Option>
            {
                new() {
                    Text = "Earth",
                    IsCorrect = false,
                    MultipleChoiceId = 1
                },
                new() {
                    Text = "Saturn",
                    IsCorrect = false,
                    MultipleChoiceId = 1
                },
                new() {
                    Text = "Jupiter",
                    IsCorrect = true,
                    MultipleChoiceId = 1
                },
                new() {
                    Text = "Uranus",
                    IsCorrect = false,
                    MultipleChoiceId = 1
                },
                new() {
                    Text = "Spring",
                    IsCorrect = false,
                    MultipleChoiceId = 2
                },
                new() {
                    Text = "Summer",
                    IsCorrect = true,
                    MultipleChoiceId = 2
                },
                new() {
                    Text = "Autumn",
                    IsCorrect = false,
                    MultipleChoiceId = 2
                },
                new() {
                    Text = "Winter",
                    IsCorrect = false,
                    MultipleChoiceId = 2
                },
            };
            context.AddRange(options);
            context.SaveChanges();
        }

        if (!context.MatchingQuestions.Any())
        {
            var quiz1 = context.Quizzes.Find(1);
            var quiz2 = context.Quizzes.Find(2);
            var questions = new List<Matching>
            {
                new() {
                    Question = "fotball,kake,hjul,knotter,bake,hav,bade,bil",
                    QuestionText = "Match the rows",
                    CorrectAnswer = "fotball,knotter,hjul,bil,bake,kake,bade,hav",
                    QuizId = 1,
                    Quiz = quiz1,
                    QuizQuestionNum = 3
                },
                new() {
                    Question = "sol,snø,tre,planet,vinter,blader",
                    QuestionText = "Match the rows",
                    CorrectAnswer = "sol,planet,tre,blader,vinter,snø",
                    QuizId = 2,
                    Quiz = quiz2,
                    QuizQuestionNum = 1
                }
            };
            context.AddRange(questions);
            context.SaveChanges();
        }
        if (!context.FillInTheBlankQuestions.Any())
        {
            var quiz1 = context.Quizzes.Find(1);
            var quiz4 = context.Quizzes.Find(4);
            var questions = new List<FillInTheBlank>
            {
                new() {
                    Question = "What is the capital of Norway?",
                    CorrectAnswer = "Oslo",
                    QuizId = 1,
                    Quiz = quiz1,
                    QuizQuestionNum = 1
                },
                new() {
                    Question = "What is the capital of Sweden?",
                    CorrectAnswer = "Stockholm",
                    QuizId = 1,
                    Quiz = quiz1,
                    QuizQuestionNum = 2
                },
                new() {
                    Question = "What is the capital of Norway?",
                    CorrectAnswer = "Oslo",
                    QuizId = 4,
                    Quiz = quiz4,
                    QuizQuestionNum = 1
                },
                new() {
                    Question = "What is the capital of Sweden?",
                    CorrectAnswer = "Stockholm",
                    QuizId = 4,
                    Quiz = quiz4,
                    QuizQuestionNum = 2
                },
            };
            context.AddRange(questions);
            context.SaveChanges();
        }
        if (!context.SequenceQuestions.Any())
        {
            var quiz1 = context.Quizzes.Find(1);
            var quiz2 = context.Quizzes.Find(2);
            var questions = new List<Sequence>
            {
                new() {
                    Question = "Make an offer,Get pre-approved,Sign the purchase agreement,Find a home,Secure financing",
                    QuestionText = "Order the following steps of applying for a home loan from first to last:",
                    CorrectAnswer = "Get pre-approved,Find a home,Make an offer,Sign the purchase agreement,Secure financing",
                    QuizId = 1,
                    Quiz = quiz1,
                    QuizQuestionNum = 4
                },
                new() {
                    Question = "1801,1345,1814,2009,1918",
                    QuestionText = "Sequence these years from earliest to latest:",
                    CorrectAnswer = "1345,1801,1814,1918,2009",
                    QuizId = 2,
                    Quiz = quiz2,
                    QuizQuestionNum = 2
                }
            };
            context.AddRange(questions);
            context.SaveChanges();
        }
        if (!context.RankingQuestions.Any())
        {
            var quiz1 = context.Quizzes.Find(1);
            var quiz2 = context.Quizzes.Find(2);
            var questions = new List<Ranking>
            {
                new() {
                    Question = "Service speed,Price,Ambiance,Food quality",
                    QuestionText = "Rank these factors from most to least important for a dining experience:",
                    CorrectAnswer = "Food quality,Service speed,Ambiance,Price",
                    QuizId = 1,
                    Quiz = quiz1,
                    QuizQuestionNum = 5
                },
                new() {
                    Question = "Location,Cleanliness,Price,Amenities",
                    QuestionText = "Rank these features from most to least important when choosing a hotel:",
                    CorrectAnswer = "Cleanliness,Location,Price,Amenities",
                    QuizId = 2,
                    Quiz = quiz2,
                    QuizQuestionNum = 3
                }
            };
            context.AddRange(questions);
            context.SaveChanges();
        }

        
        if (!context.FlashCardQuizzes.Any())
        {
            var quiz = new List<FlashCardQuiz>
            {
                new() {Name = "Capitals of Scandinavia", Description = "Flashcards with questions about the captial cities of Scandinavia"},
                new() {Name = "Which band?", Description = "Which band wrote the song on the card?"}
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
                new() {
                    Question = "What is the capital of Norway?",
                    Answer = "Oslo",
                    Quiz = quiz1!,
                    QuizQuestionNum = 1
                },
                new() {
                    Question = "What is the capital of Sweden?",
                    Answer = "Stockholm",
                    Quiz = quiz1!,
                    QuizQuestionNum = 2
                },
                new() {
                    Question = "What is the capital of Denmark?",
                    Answer = "Copenhagen",
                    Quiz = quiz1!,
                    QuizQuestionNum = 3
                },
                new() {
                    Question = "Who wrote the song 'Supermassive Black Hole'?",
                    Answer = "Muse",
                    Quiz = quiz2!,
                    QuizQuestionNum = 1
                },
                new() {
                    Question = "Who wrote the song 'Paranoid Android'?",
                    Answer = "Radiohead",
                    Quiz = quiz2!,
                    QuizQuestionNum = 2
                },
                new() {
                    Question = "Who wrote the song 'Somewhere Only We know'?",
                    Answer = "Keane",
                    Quiz = quiz2!,
                    QuizQuestionNum = 3
                },
                new() {
                    Question = "Who wrote the song 'Do I Wanna Know'?",
                    Answer = "Arctic Monkeys",
                    Quiz = quiz2!,
                    QuizQuestionNum = 4
                },
                new() {
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

        var quizzes = context.Quizzes
                                .Include(q => q.FillInTheBlankQuestions)
                                .Include(q => q.MatchingQuestions)
                                .Include(q => q.RankingQuestions)
                                .Include(q => q.SequenceQuestions)
                                .Include(q => q.TrueFalseQuestions)
                                .Include(q => q.MultipleChoiceQuestions)
                                .ToList();
        foreach (var quiz in quizzes)
        {
            quiz.NumOfQuestions = quiz.AllQuestions != null ? quiz.AllQuestions.Count() : 0;
        }
        context.SaveChanges();
    }
}
