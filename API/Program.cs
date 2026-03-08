using Microsoft.EntityFrameworkCore;
using Persistence;
using API;
using API.Middleware;
using API.SignalR;
using Application;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Persistence.Contexts;
using Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddPersistence();
builder.Services.AddInfrastructure();
builder.Services.AddCorsPolicy();
builder.Services.AddApplicationServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMiddleWare();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("Development");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGroup(Environment.GetEnvironmentVariable("API")!).MapIdentityApi<User>(); //  api/login
app.MapHub<CommentHub>(Environment.GetEnvironmentVariable("COMMENT_PATH")!); // SignalR

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    await context.Database.MigrateAsync();
    await DbInitializer.SeedData(context, userManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred seeding the DB.");
}

app.Run();
