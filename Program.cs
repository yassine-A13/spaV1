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

// Configurer l'authentification et la session
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "Cookies";
})
.AddCookie("Cookies", options =>
{
    options.LoginPath = "/Home/Index";
    options.LogoutPath = "/Home/Logout";
    options.AccessDeniedPath = "/Home/AccessDenied";
});

// Ajouter le service de session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

// Ajouter le middleware de session
app.UseSession();

// Ajouter l'authentification avant l'autorisation
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
