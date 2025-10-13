using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL;

public class QuizDbContext : DbContext
{
    public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Matching> MatchingQuestions { get; set; }
    public DbSet<Sequence> SequenceQuestions { get; set; }
    public DbSet<Ranking> RankingQuestions { get; set; }
    public DbSet<FillInTheBlank> FillInTheBlankQuestions { get; set; }
    public DbSet<FlashCardQuiz> FlashCardQuizzes { get; set; }
    public DbSet<FlashCard> FlashCards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

    // Configures cascaded delete 
    // Ensures that all FlashCards belonging to 
    // deleted FlashCardQuiz are also deleted
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FlashCard>()
            .HasOne(fc => fc.Quiz)
            .WithMany(q => q.FlashCards)
            .HasForeignKey(fc => fc.QuizId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}