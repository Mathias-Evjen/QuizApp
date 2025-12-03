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
                    Question = "Match the rows",
                    CorrectAnswer = "fotball,knotter,hjul,bil,bake,kake,bade,hav",
                    QuizId = 1,
                    Quiz = quiz1,
                    QuizQuestionNum = 3
                },
                new() {
                    Question = "Match the rows",
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
                    Question = "Order the following steps of applying for a home loan from first to last:",
                    CorrectAnswer = "Get pre-approved,Find a home,Make an offer,Sign the purchase agreement,Secure financing",
                    QuizId = 1,
                    Quiz = quiz1,
                    QuizQuestionNum = 4
                },
                new() {
                    Question = "Sequence these years from earliest to latest:",
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
                    Question = "Rank these factors from most to least important for a dining experience:",
                    CorrectAnswer = "Food quality,Service speed,Ambiance,Price",
                    QuizId = 1,
                    Quiz = quiz1,
                    QuizQuestionNum = 5
                },
                new() {
                    Question = "Rank these features from most to least important when choosing a hotel:",
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
                new() {Name = "Which band?", Description = "Which band wrote the song on the card?"},
                new() {Name = "Norwegian translations", Description = "What is this english word in norwegian?"},
                new() {Name = "Who was born on this day?", Description = "What famous person was born on this day?"},
                new() {Name = "Antonyms", Description = "What are the antonyms of the words?"}
            };
            context.AddRange(quiz);
            context.SaveChanges();
        }

        if (!context.FlashCards.Any())
        {
            var quiz1 = context.FlashCardQuizzes.Find(1);
            var quiz2 = context.FlashCardQuizzes.Find(2);
            var quiz3 = context.FlashCardQuizzes.Find(3);
            var quiz4 = context.FlashCardQuizzes.Find(4);
            var quiz5 = context.FlashCardQuizzes.Find(5);
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
                },
                new() {
                    Question = "Resilient",
                    Answer = "Robust",
                    Quiz = quiz3!,
                    QuizQuestionNum = 1
                },
                new() {
                    Question = "Ambiguous",
                    Answer = "Tvetydig",
                    Quiz = quiz3!,
                    QuizQuestionNum = 2
                },
                new() {
                    Question = "Inevitable",
                    Answer = "Uunngåelig",
                    Quiz = quiz3!,
                    QuizQuestionNum = 3
                },
                new() {
                    Question = "Meticulous",
                    Answer = "Nøye/Grundig",
                    Quiz = quiz3!,
                    QuizQuestionNum = 4
                },
                new() {
                    Question = "Vulnerable",
                    Answer = "Sårbar",
                    Quiz = quiz3!,
                    QuizQuestionNum = 5
                },
                new() {
                    Question = "Consequence",
                    Answer = "Konsekvens",
                    Quiz = quiz3!,
                    QuizQuestionNum = 6
                },
                new() {
                    Question = "Preliminary",
                    Answer = "Foreløpig/Innledende",
                    Quiz = quiz3!,
                    QuizQuestionNum = 7
                },
                new() {
                    Question = "Sustainable",
                    Answer = "Bærekraftig",
                    Quiz = quiz3!,
                    QuizQuestionNum = 8
                },
                new() {
                    Question = "Comprehensive",
                    Answer = "Omfattende",
                    Quiz = quiz3!,
                    QuizQuestionNum = 9
                },
                new() {
                    Question = "Reluctant",
                    Answer = "Motvillig",
                    Quiz = quiz3!,
                    QuizQuestionNum = 10
                },
                new() {
                    Question = "15 August 1769",
                    Answer = "Napoleon Bonaparte",
                    Quiz = quiz4!,
                    QuizQuestionNum = 1
                },
                new() {
                    Question = "14 March 1879",
                    Answer = "Albert Einstein",
                    Quiz = quiz4!,
                    QuizQuestionNum = 2
                },
                new() {
                    Question = "15 January 1929",
                    Answer = "Martin Luther Kind Jr.",
                    Quiz = quiz4!,
                    QuizQuestionNum = 3
                },
                new() {
                    Question = "15 April 1452",
                    Answer = "Leonardo da Vinci",
                    Quiz = quiz4!,
                    QuizQuestionNum = 4
                },
                new() {
                    Question = "4 January 1643",
                    Answer = "Isaac Newton",
                    Quiz = quiz4!,
                    QuizQuestionNum = 5
                },
                new() {
                    Question = "7 November 1867",
                    Answer = "Marie Curie",
                    Quiz = quiz4!,
                    QuizQuestionNum = 6
                },
                new() {
                    Question = "6 January 1412",
                    Answer = "Joan of Arc",
                    Quiz = quiz4!,
                    QuizQuestionNum = 7
                },
                new() {
                    Question = "69 BC",
                    Answer = "Cleopatra VII",
                    Quiz = quiz4!,
                    QuizQuestionNum = 8
                },
                new() {
                    Question = "Resilient",
                    Answer = "Fragile",
                    Quiz = quiz5!,
                    QuizQuestionNum = 1
                },
                new() {
                    Question = "Ambigious",
                    Answer = "Explicit",
                    Quiz = quiz5!,
                    QuizQuestionNum = 2
                },
                new() {
                    Question = "Inevitable",
                    Answer = "Avoidable",
                    Quiz = quiz5!,
                    QuizQuestionNum = 3
                },
                new() {
                    Question = "Meticulous",
                    Answer = "Careless",
                    Quiz = quiz5!,
                    QuizQuestionNum = 4
                },
                new() {
                    Question = "Vulnerable",
                    Answer = "Protected",
                    Quiz = quiz5!,
                    QuizQuestionNum = 5
                },
                new() {
                    Question = "Preliminary",
                    Answer = "Final",
                    Quiz = quiz5!,
                    QuizQuestionNum = 6
                },
                new() {
                    Question = "Sustainable",
                    Answer = "Unsustainable",
                    Quiz = quiz5!,
                    QuizQuestionNum = 7
                },
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
