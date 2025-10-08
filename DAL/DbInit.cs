using QuizApp.Models;

namespace QuizApp.DAL
{
    public static class DbInit
    {
        public static void Seed(AppDbContext context)
        {
            // Pass på at databasen og tabellene er opprettet
            context.Database.EnsureCreated();

            // Legg til data bare hvis det ikke finnes fra før
            if (!context.Questions.Any())
            {
                // Multiple-Choice-spørsmål
                var fruitQ = new MultipleChoice
                {
                    QuestionText = "Hvilke av disse er frukt?",
                    Options = new List<Option>
                    {
                        new Option { Text = "Eple",  IsCorrect = true },
                        new Option { Text = "Banan", IsCorrect = true },
                        new Option { Text = "Potet", IsCorrect = false },
                        new Option { Text = "Tomat", IsCorrect = true  }
                    }
                };

                // True/False-spørsmål
                var tfQ = new TrueFalseQuestion
                {
                    QuestionText = "C# er et programmeringsspråk.",
                    CorrectAnswer = true
                };

                context.MultipleChoices.Add(fruitQ);
                context.TrueFalseQuestions.Add(tfQ);
                context.SaveChanges();
            }
        }
    }
}