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

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
            builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });

builder.Services.AddScoped(typeof(IQuestionRepository<>), typeof(QuestionRepository<>));
builder.Services.AddScoped<IQuestionRepository<MultipleChoice>, MultipleChoiceRepository>();
builder.Services.AddScoped<IQuizRepository<Quiz>, QuizRepository>();
builder.Services.AddScoped<IQuizRepository<FlashCardQuiz>, FlashCardQuizRepository>();
builder.Services.AddScoped(typeof(IAttemptRepository<>), typeof(AttemptRepository<>));
builder.Services.AddScoped<IAttemptRepository<QuizAttempt>, QuizAttemptRepository>();


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