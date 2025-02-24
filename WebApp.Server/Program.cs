using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataAccessLevel;
using BusinessLogic.IManager;
using BusinessLogic.Services;
using BusinessLogic.Manager;

var builder = WebApplication.CreateBuilder(args);

// Configurazione del DbContext per il database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Aggiungi i controller al servizio
builder.Services.AddControllers();

// Configurazione CORS per permettere al frontend di fare richieste al backend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://localhost:49755", "http://localhost:4200") // Aggiungi più origini se necessario
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Abilita Swagger per la documentazione dell'API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrazione dei servizi 
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserFactory, UserFactory>();


// Configurazione dell'autenticazione JWT con log per debug
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "La chiave JWT non è configurata correttamente.")
            ))
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($" Autenticazione fallita: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine($" Token validato per: {context.Principal.Identity.Name}");
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Middleware per gestione errori globali
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

// Middleware per CORS
app.UseCors("AllowSpecificOrigins");

// Middleware per autenticazione e autorizzazione
app.UseAuthentication();
app.UseAuthorization();

// Mappatura dei controller
app.MapControllers();

// Avvio dell'applicazione
app.Run();
