using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL     // eller QuizApp.Models hvis du ikke har flyttet den
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<MultipleChoice> MultipleChoices { get; set; }
        public DbSet<TrueFalseQuestion> TrueFalseQuestions { get; set; }
        public DbSet<Option> Options { get; set; }

        // 👇 LEGG TIL DENNE METODEN
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfigurer Table-Per-Hierarchy (TPH) arv
            modelBuilder.Entity<Question>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<MultipleChoice>("MultipleChoice")
                .HasValue<TrueFalseQuestion>("TrueFalse");
        }
    }
}

