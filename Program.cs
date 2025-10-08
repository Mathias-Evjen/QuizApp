using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using QuizApp.DAL;                

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Legg til SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


//  Seed database når appen starter
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInit.Seed(db);                 //  kjør seeding
}


// Middleware
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
    pattern: "{controller=Quiz}/{action=Index}/{id?}"); //  evt. start direkte på Quiz

app.Run();
