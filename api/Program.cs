using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using QuizApp.DAL;
using QuizApp.Services;
using QuizApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<QuizDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:QuizDbContextConnection"]
    );
});

builder.Services.AddControllers();
builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
            builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });

builder.Services.AddScoped<IRepository<Quiz>, QuizRepository>();
builder.Services.AddScoped<IRepository<Matching>, MatchingRepository>();
builder.Services.AddScoped<IRepository<Sequence>, SequenceRepository>();
builder.Services.AddScoped<IRepository<Ranking>, RankingRepository>();
builder.Services.AddScoped<IRepository<FillInTheBlank>, FillInTheBlankRepository>();
builder.Services.AddScoped<IFlashCardRepository, FlashCardRepository>();
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

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File($"APILogs/app_{DateTime.Now:yyyyMMdd_HHmmss}.log")
    .Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var value) &&
                            e.Level == LogEventLevel.Information &&
                            e.MessageTemplate.Text.Contains("Executed DbCommand"));

var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    DBInit.Seed(app);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");
app.MapControllers();

app.Run();