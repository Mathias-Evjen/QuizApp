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
                new() {Name = "Sports & Fitness", Description = "Quiz about sports and fitness."},
                new() {Name = "Food & Cooking", Description = "This is a quiz about food and cooking."},
            };
            context.AddRange(quiz);
            context.SaveChanges();
        }

        if (!context.TrueFalseQuestions.Any())
        {
            var quiz1 = context.Quizzes.Find(1);
            var quiz2 = context.Quizzes.Find(2);
            var qustions = new List<TrueFalse>
            {
                new() {
                    Question = "Swimming is a full-body workout",
                    CorrectAnswer = true,
                    QuizId = 1,
                    Quiz = quiz1,
                    QuizQuestionNum = 4
                },
                new() {
                    Question = "Boiling vegetables preserves all their nutrients",
                    CorrectAnswer = false,
                    QuizId = 2,
                    Quiz = quiz2,
                    QuizQuestionNum = 4
                }
            };
            context.AddRange(qustions);
            context.SaveChanges();
        }

        if (!context.MultipleChoiceQuestions.Any())
        {
            var quiz1 = context.Quizzes.Find(1);
            var quiz2 = context.Quizzes.Find(2);
            var questions = new List<MultipleChoice>
            {
                new() {
                    Question = "Which of these is an Olympic sport?",
                    CorrectAnswer = "Table Tennis",
                    QuizId = 1,
                    Quiz = quiz1,
                    QuizQuestionNum = 6
                },
                new() {
                    Question = "Which herb is commonly used in Italian cooking?",
                    CorrectAnswer = "Basil",
                    QuizId = 2,
                    Quiz = quiz2,
                    QuizQuestionNum = 6
                }
            };
            context.AddRange(questions);
            context.SaveChanges();
        }

        if (!context.Options.Any())
        {
            var options = new List<Option>
            {
                new() { Text = "Table Tennis", IsCorrect = true, MultipleChoiceId = 1 },
                new() { Text = "Chess", IsCorrect = false, MultipleChoiceId = 1 },
                new() { Text = "Bowling", IsCorrect = false, MultipleChoiceId = 1 },
                new() { Text = "Video Gaming", IsCorrect = false, MultipleChoiceId = 1 },
                new() { Text = "Basil", IsCorrect = true, MultipleChoiceId = 2 },
                new() { Text = "Cilantro", IsCorrect = false, MultipleChoiceId = 2 },
                new() { Text = "Mint", IsCorrect = false, MultipleChoiceId = 2 },
                new() { Text = "Dill", IsCorrect = false, MultipleChoiceId = 2 },
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
                    Question = "Match the sports with their equipment",
                    CorrectAnswer = "soccer,cleats,cycling,helmet,basketball,ball,swimming,swim cap",
                    QuizId = 1,
                    Quiz = quiz1,
                    QuizQuestionNum = 1
                },
                new() {
                    Question = "Match the food with its category",
                    CorrectAnswer = "apple,fruit,salmon,fish,cheese,dairy,pizza,fast food",
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
            var quiz2 = context.Quizzes.Find(2);
            var questions = new List<FillInTheBlank>
            {
                new() {
                    Question = "The sport known as 'the beautiful game' is ___",
                    CorrectAnswer = "soccer",
                    QuizId = 1,
                    Quiz = quiz1,
                    QuizQuestionNum = 5
                },
                new() {
                    Question = "The main ingredient in guacamole is ___",
                    CorrectAnswer = "avocado",
                    QuizId = 2,
                    Quiz = quiz2,
                    QuizQuestionNum = 5
                }
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
                    Question = "Order the steps of a typical workout from start to finish:",
                    CorrectAnswer = "Warm-up,Stretching,Cardio,Strength training,Cool down",
                    QuizId = 1,
                    Quiz = quiz1,
                    QuizQuestionNum = 2
                },
                new() {
                    Question = "Order the steps of baking a cake from start to finish:",
                    CorrectAnswer = "Preheat oven,Mix ingredients,Pour batter into pan,Bake,Cool,Decorate",
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
                    Question = "Rank these exercises from most to least calories burned per hour:",
                    CorrectAnswer = "Running,Swimming,Cycling,Yoga",
                    QuizId = 1,
                    Quiz = quiz1,
                    QuizQuestionNum = 3
                },
                new() {
                    Question = "Rank these cooking methods from healthiest to least healthy:",
                    CorrectAnswer = "Steaming,Grilling,Baking,Frying",
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
