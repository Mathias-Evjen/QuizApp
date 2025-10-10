using Microsoft.EntityFrameworkCore;
using QuizApp.DAL;
using QuizApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// ✅ Legg til SQLite-databasekobling
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Registrer Repository-avhengigheter
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IMultipleChoiceRepository, MultipleChoiceRepository>();
builder.Services.AddScoped<ITrueFalseRepository, TrueFalseRepository>();

var app = builder.Build();

// ✅ Kjør DbInit manuelt (uten å registrere den som service)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    DbInit.Seed(context); // ✅ Dette fungerer fordi DbInit er static
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
