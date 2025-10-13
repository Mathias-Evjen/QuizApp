using QuizApp.Models;

namespace QuizApp.DAL
{
    public static class DbInit
    {
        public static void Seed(QuizDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Questions.Any())
            {
                var fruitQ = new MultipleChoice
                {
                    QuestionTexts = "Hvilke av disse er frukt?",
                    Options = new List<Option>
                    {
                        new Option { Text = "Eple",  IsCorrect = true },
                        new Option { Text = "Banan", IsCorrect = true },
                        new Option { Text = "Potet", IsCorrect = false },
                        new Option { Text = "Tomat", IsCorrect = true }
                    }
                };

                var tfQ = new TrueFalseQuestion
                {
                    QuestionTexts = "C# er et programmeringsspråk.",
                    CorrectAnswer = true
                };

                context.MultipleChoices.Add(fruitQ);
                context.TrueFalseQuestions.Add(tfQ);

                context.SaveChanges();
            }
        }
    }
}
