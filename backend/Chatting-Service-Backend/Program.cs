using Chatting_Service_Backend.Data;
using Microsoft.EntityFrameworkCore;
using Chatting_Service_Backend.Hubs;
var builder = WebApplication.CreateBuilder(args);




// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true; // Optional: Makes JSON output more readable
    });
var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
Console.WriteLine($"Connection String: {connectionString}");
builder.Services.AddDbContext<DatabaseConnector>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddSignalR(); // Add SignalR service

// Add CORS policy to allow requests from the frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins("http://localhost:5173")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials()); // Allow credentials for SignalR
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseConnector>();
    dbContext.Database.Migrate(); // Apply any pending migrations
}


app.UseCors("AllowFrontend"); // Allow all origins for CORS
app.Urls.Add("http://*:5062"); // Listen on all network interfaces on port 5062

// Configure the HTTP request pipeline.
app.UseRouting();
app.MapControllers();
app.MapHub<ChatHub>("/chatHub"); // Map the SignalR hub

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
