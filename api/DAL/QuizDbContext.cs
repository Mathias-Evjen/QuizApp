using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL;

public class QuizDbContext : DbContext
{
    public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Matching> MatchingQuestions { get; set; }
    public DbSet<Sequence> SequenceQuestions { get; set; }
    public DbSet<Ranking> RankingQuestions { get; set; }
    public DbSet<FillInTheBlank> FillInTheBlankQuestions { get; set; }
    public DbSet<FlashCardQuiz> FlashCardQuizzes { get; set; }
    public DbSet<FlashCard> FlashCards { get; set; }         
    public DbSet<MultipleChoice> MultipleChoiceQuestions { get; set; }     
    public DbSet<TrueFalse> TrueFalseQuestions { get; set; }
    public DbSet<Option> Options { get; set; } 

    public DbSet<QuizAttempt> QuizAttempts { get; set; }
    public DbSet<FillInTheBlankAttempt> FillInTheBlankAttempts { get; set; }
    public DbSet<TrueFalseAttempt> TrueFalseAttempts { get; set; }
    public DbSet<MultipleChoiceAttempt> MultipleChoiceAttempts { get; set; }
    public DbSet<MatchingAttempt> MatchingAttempts { get; set; }
    public DbSet<SequenceAttempt> SequenceAttempts { get; set; }
    public DbSet<RankingAttempt> RankingAttempts { get; set; }

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

        modelBuilder.Entity<MultipleChoice>()
            .HasOne(mc => mc.Quiz)
            .WithMany(q => q.MultipleChoiceQuestions)
            .HasForeignKey(mc => mc.QuizId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Option>()
            .HasOne(o => o.MultipleChoice)
            .WithMany(mc => mc.Options)
            .HasForeignKey(o => o.MultipleChoiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
