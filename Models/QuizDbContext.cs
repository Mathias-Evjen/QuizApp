using Microsoft.EntityFrameworkCore;

namespace QuizApp.Models
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options) { }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
    }
}