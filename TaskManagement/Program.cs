using Application.Service;
using Application.Services;
using Domain.Interface;
using Infrastructure.Data;
using Infrastructure.repository;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// configuration de la connexion a la base de données
var ConnectionString = builder.Configuration.GetConnectionString("Mysql");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
    }
);


// register articleRepository and service
builder.Services.AddScoped<InterfaceArticle, ArticleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<ArticleServiceRepository>();
builder.Services.AddScoped<UserServiceRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Register}/{id?}");

app.Run();
