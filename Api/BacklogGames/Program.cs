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
        policy.WithOrigins(
            "https://cheerful-custard-b65983.netlify.app",
            "http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

//Configuracion para Render
if (builder.Environment.IsProduction())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

//Connection String con variable de entorno
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


var app = builder.Build();


// Aplicar migraciones automáticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

//EP para testar que este vivo la api
app.MapGet("/health", () => Results.Ok("alive"));


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
