
using Application.Classes.Mail;
using Application.Interface;
using Application.Services;
using Application.Services.Mail;
using Domain.Helper;
using Domain.Interface;
using Infrastructure.Data;
using Infrastructure.repository;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using System.Configuration;
using System.Globalization;

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

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


var supportedCultures = new[]
    {
        new CultureInfo("en-US")  // Définir la culture en anglais (États-Unis)
    };


// register articleRepository and service
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<UserServiceRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/User/MustLogin";
                options.AccessDeniedPath = "/User/AccesDenied";
            });
builder.Services.AddScoped<IUserTaskRepository, UserTaskRepository>();
builder.Services.AddScoped<SUserTaskRepository>();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddScoped<ILeavesRepository, LeavesRepository>();
builder.Services.AddScoped<ILeavesService, LeavesService>();

builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddScoped<ITasksService, TasksService>();
builder.Services.AddTransient<IDataEncryptorKeyProvider, DataEncryptorKeyProvider>();
builder.Services.AddSingleton<DataEncryptor>();

builder.Services.AddTransient<IFileViewToStringforEmailService, FileViewToStringforEmailService>();
builder.Services.AddTransient<ISendMailWithornoAttacheService, SendMailWithornoAttacheService>();
builder.Services.AddTransient<ISendMailService, SendMailService>();
builder.Services.AddSingleton<ISmtpMailSender, SmtpMailSender>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));


// Configuration d'AutoMapper
//builder.Services.AddAutoMapper(typeof(Program).Assembly);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseStatusCodePagesWithReExecute("/Home/PageNotFound");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

// Initialisez la base de données pour enregistrer les comptes administrateurs par defaut
using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<UserServiceRepository>();
    dbInitializer.InitialiseUser();
}

app.Run();
