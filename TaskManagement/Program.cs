
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
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
// configuration de la connexion a la base de données
var ConnectionString = builder.Configuration.GetConnectionString("Mysql");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
    }
);


// register articleRepository and service

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<UserServiceRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";

                //gere la redirection pour souvenir les pages precedent
                //options.Events = new CookieAuthenticationEvents
                //{
                //    OnRedirectToLogin = context =>
                //    {
                //        context.Response.Redirect(context.RedirectUri + "&returnUrl=" + context.Request.Path);
                //        return Task.CompletedTask;
                //    }
                //};
            });
builder.Services.AddScoped<IUserTaskRepository, UserTaskRepository>();
builder.Services.AddScoped<SUserTaskRepository>();

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
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
