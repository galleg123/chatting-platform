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
builder.Services.AddDbContext<DatabaseConnector>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
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

app.UseCors("AllowFrontend"); // Allow all origins for CORS

// Configure the HTTP request pipeline.
app.UseRouting();
app.MapControllers();
app.MapHub<ChatHub>("/chatHub"); // Map the SignalR hub

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
