using Microsoft.EntityFrameworkCore;
using UAMPass.Models;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------
// 1) MVC
// -----------------------------------------------------
builder.Services.AddControllersWithViews();

// -----------------------------------------------------
// 2) BASE DE DATOS (PostgreSQL)
// -----------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));


// -----------------------------------------------------
// 3) SESSION (NECESARIO PARA LOGIN)
// -----------------------------------------------------
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2); // Tiempo activa
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // <-- NECESARIO
});


// -----------------------------------------------------
// 4) BUILD APP
// -----------------------------------------------------
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// -----------------------------------------------------
// 5) ARCHIVOS ESTÁTICOS (wwwroot)
// -----------------------------------------------------
app.UseStaticFiles();

// -----------------------------------------------------
// 6) ROUTING
// -----------------------------------------------------
app.UseRouting();

// -----------------------------------------------------
// 7) SESSION (DEBE IR AQUÍ ANTES DE Authorization)
// -----------------------------------------------------
app.UseSession();

// -----------------------------------------------------
// 8) AUTHORIZATION
// -----------------------------------------------------
app.UseAuthorization();


// -----------------------------------------------------
// 9) RUTAS
// -----------------------------------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
