using BacklogGames.DataAccess.Layer.Data;
using BacklogGames.DataAccess.Layer.Repositories.BaseRepository;
using BacklogGames.DataAccess.Layer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using BacklogGames.Bussinnes.Layer.Services.GameService;
using Microsoft.Extensions.DependencyInjection;
using BacklogGames.Bussinnes.Layer;
using BacklogGames.DataAccess.Layer.Repositories.GameRepository;
using BacklogGames.DataAccess.Layer.Repositories.UserListRepository;
using BacklogGames.Bussinnes.Layer.Services.UserListService;
using BacklogGames.Bussinnes.Layer.Services.IgdbService;

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
builder.Services.AddScoped<IUserListService,UserListService>();// Registrar servicios de negocio
builder.Services.AddHttpClient<IIgdbService, IgdbService>();

// Configurar AutoMapper y otros servicios de negocio
builder.Services.AddBusinessServices();

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

app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
