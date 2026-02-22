using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Study_board.Business.Mappings;
using Study_board.Business.Repositories.Implementations;
using Study_board.Business.Repositories.Interfaces;
using Study_board.Business.Services.Implementations;
using Study_board.Business.Services.Interfaces;
using Study_board.Data.Persistance;
using Study_board.Models.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(ChecklistMappingProfile).Assembly);
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IChecklistService, ChecklistService>();
builder.Services.AddTransient<IProjectService, ProjectService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();
builder.Services.Configure<StudyPointsSettings>(
    builder.Configuration.GetSection("StudyPointsSettings"));




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();