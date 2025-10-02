var builder = WebApplication.CreateBuilder(args);

// Legg til MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
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