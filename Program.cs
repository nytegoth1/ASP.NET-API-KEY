using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Optional
});

// Configure database connection and services
builder.Services.AddDbContext<ApiKeyContext>(options =>
    options.UseMySql("Server=127.0.0.1;Database=ApiKeyDb;User Id=root;Password='';", 
                     new MySqlServerVersion(new Version(8, 0, 23))));

builder.Services.AddScoped<ApiKeyService>();
builder.Services.AddControllers();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()  // Allow any origin
                   .AllowAnyMethod()  // Allow any method (GET, POST, etc.)
                   .AllowAnyHeader(); // Allow any header
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll"); // Use the CORS policy here
app.UseAuthorization();

app.MapControllers();

app.Run();
