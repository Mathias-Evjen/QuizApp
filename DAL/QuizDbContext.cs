using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.DAL
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options)
        {
        }

        public DbSet<QuestionText> Questions { get; set; }              
        public DbSet<MultipleChoice> MultipleChoices { get; set; }     
        public DbSet<TrueFalseQuestion> TrueFalseQuestions { get; set; }
        public DbSet<Option> Options { get; set; }                      
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuestionText>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<MultipleChoice>("MultipleChoice")
                .HasValue<TrueFalseQuestion>("TrueFalse");
        }
    }
}
