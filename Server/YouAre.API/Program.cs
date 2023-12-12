using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YouAre.Persistent;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();

builder.Logging.ClearProviders();

// Auth:
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();

// Swagger:
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Controllers:
builder.Services.AddControllers();

// Code-first Entity FW DbContext with Auth:
builder.Services.AddDbContext<DataContext>(options => options.UseSqlite("DataSource=YouAreLite.db"));


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
