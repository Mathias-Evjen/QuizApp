using Microsoft.EntityFrameworkCore;
using QuizApp.DAL;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("QuizDbContextConnection") ?? throw new
InvalidOperationException("Connection string 'QuizDbContextConnection' not found.");

// Legg til MVC
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<QuizDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:QuizDbContextConnection"]
    );
});

builder.Services.AddScoped<IMatchingRepository, MatchingRepository>();





var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
// app.UseHttpsRedirection(); // valgfritt for testing uten HTTPS

app.UseRouting();

app.UseAuthorization();

// Standard route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Matching}/{action=Question}/{id?}"
);

app.Run();