using BacklogGames.Exceptions;
using BacklogGames.DataAccess.Layer.Data;
using BacklogGames.DataAccess.Layer.Repositories.BaseRepository;
using BacklogGames.DataAccess.Layer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using BacklogGames.Bussinnes.Layer.Services.GameService;
using BacklogGames.Bussinnes.Layer;
using BacklogGames.DataAccess.Layer.Repositories.GameRepository;
using BacklogGames.DataAccess.Layer.Repositories.UserListRepository;
using BacklogGames.Bussinnes.Layer.Services.UserListService;
using BacklogGames.Bussinnes.Layer.Services.IgdbService;
using BacklogGames.Bussinnes.Layer.Services.AuthService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configurar DbContext con PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar el repositorio genérico y Unit of Work
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Registrar repositorios específicos
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IUserListRepository, UserListRepository>();

//Registrar Services
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IUserListService, UserListService>();
builder.Services.AddHttpClient<IIgdbService, IgdbService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configurar AutoMapper y otros servicios de negocio
builder.Services.AddBusinessServices();

// Configurar JWT Authentication
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!))
        };
    });

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
