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

builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IMatchingRepository, MatchingRepository>();
builder.Services.AddScoped<ISequenceRepository, SequenceRepository>();
builder.Services.AddScoped<IRankingRepository, RankingRepository>();
builder.Services.AddScoped<IFillInTheBlankRepository, FillInTheBlankRepository>();
builder.Services.AddScoped<IFlashCardRepository, FlashCardRepository>();
builder.Services.AddScoped<IFlashCardQuizRepository, FlashCardQuizRepository>();
builder.Services.AddScoped<QuizService>();
builder.Services.AddScoped<IFlashCardQuizService, FlashCardQuizService>();
builder.Services.AddScoped<IMultipleChoiceRepository, MultipleChoiceRepository>();
builder.Services.AddScoped<ITrueFalseRepository, TrueFalseRepository>();

builder.Services.AddScoped<IQuizAttemptRepository, QuizAttemptRepository>();
builder.Services.AddScoped<IFillInTheBlankAttemptRepository, FillInTheBlankAttemptRepository>();
builder.Services.AddScoped<ITrueFalseAttemptRepository, TrueFalseAttemptRepository>();
builder.Services.AddScoped<IMultipleChoiceAttemptRepository, MultipleChoiceAttemptRepository>();
builder.Services.AddScoped<IMatchingAttemptRepository, MatchingAttemptRepository>();
builder.Services.AddScoped<ISequenceAttemptRepository, SequenceAttemptRepository>();
builder.Services.AddScoped<IRankingAttemptRepository, RankingAttemptRepository>();

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