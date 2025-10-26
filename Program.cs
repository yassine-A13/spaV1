using Microsoft.EntityFrameworkCore;
using spaV1.Models;
using spaV1.Interfaces;
using spaV1.Services;

var builder = WebApplication.CreateBuilder(args);

// Ajouter MVC
builder.Services.AddControllersWithViews();

// Ajouter la connexion à SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Enregistrer les services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IServiceSpa, ServiceSpaService>();
builder.Services.AddScoped<IRendezVousService, RendezVousService>();
builder.Services.AddScoped<IPaiementService, PaiementService>();

var app = builder.Build();

// Middleware de base
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
