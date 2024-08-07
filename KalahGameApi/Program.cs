using Microsoft.Extensions.Options;
using KalahGameApi.Data;
using KalahGameApi.Models;
using MongoDB.Driver;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<Settings>(
    builder.Configuration.GetSection(nameof(Settings)));

builder.Services.AddSingleton<LeaderboardContext>();
builder.Services.AddSingleton<AnnouncementContext>();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddSignalR().AddNewtonsoftJsonProtocol(); // Add SignalR

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "KalahGameApi",
        Version = "v1",
        Description = "API for Kalah Game",
        Contact = new OpenApiContact
        {
            Name = "Your Name",
            Email = "your.email@example.com",
            Url = new Uri("https://yourwebsite.com")
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "KalahGameApi v1");
        c.RoutePrefix = string.Empty; // Sets Swagger UI at app's root
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();
app.MapHub<MatchHub>("/match"); // Map the SignalR hub

app.Run();
