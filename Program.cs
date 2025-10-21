using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Identity;
using QuizApp.DAL;
using QuizApp.Services;
using QuizApp.Models;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("QuizDbContextConnection") ?? throw new
InvalidOperationException("Connection string 'QuizDbContextConnection' not found.");

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<QuizDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:QuizDbContextConnection"]
    );
});

builder.Services.AddScoped<IRepository<Quiz>, QuizRepository>();
builder.Services.AddScoped<IRepository<Matching>, MatchingRepository>();
builder.Services.AddScoped<IRepository<Sequence>, SequenceRepository>();
builder.Services.AddScoped<IRepository<Ranking>, RankingRepository>();
builder.Services.AddScoped<IRepository<FillInTheBlank>, FillInTheBlankRepository>();
builder.Services.AddScoped<IRepository<FlashCard>, FlashCardRepository>();
builder.Services.AddScoped<IRepository<FlashCardQuiz>, FlashCardQuizRepository>();
builder.Services.AddScoped<IRepository<MultipleChoice>, MultipleChoiceRepository>();
builder.Services.AddScoped<IRepository<TrueFalse>, TrueFalseRepository>();

builder.Services.AddScoped<IAttemptRepository<QuizAttempt>, QuizAttemptRepository>();
builder.Services.AddScoped<IAttemptRepository<FillInTheBlankAttempt>, FillInTheBlankAttemptRepository>();
builder.Services.AddScoped<IAttemptRepository<TrueFalseAttempt>, TrueFalseAttemptRepository>();
builder.Services.AddScoped<IAttemptRepository<MultipleChoiceAttempt>, MultipleChoiceAttemptRepository>();
builder.Services.AddScoped<IAttemptRepository<MatchingAttempt>, MatchingAttemptRepository>();
builder.Services.AddScoped<IAttemptRepository<SequenceAttempt>, SequenceAttemptRepository>();
builder.Services.AddScoped<IAttemptRepository<RankingAttempt>, RankingAttemptRepository>();

builder.Services.AddScoped<QuizService>();
builder.Services.AddScoped<IFlashCardQuizService, FlashCardQuizService>();

builder.Services.AddRazorPages();

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File($"Logs/app_{DateTime.Now:yyyyMMdd_HHmmss}.log");

loggerConfiguration.Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var value) &&
                            e.Level == LogEventLevel.Information &&
                            e.MessageTemplate.Text.Contains("Executed DbCommand"));

var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DBInit.Seed(app);
}

app.UseStaticFiles();

app.MapDefaultControllerRoute();

app.MapRazorPages();

app.UseStaticFiles();
// app.UseHttpsRedirection(); // valgfritt for testing uten HTTPS

app.UseRouting();

app.UseAuthorization();


app.MapDefaultControllerRoute();

//app.MapRazorPages();

app.Run();