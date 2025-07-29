using System.Text;
using AiTextSummarizerApi.Data;
using AiTextSummarizerApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"),
    new MySqlServerVersion(new Version(9,3,0))
));



var jwtKey = builder.Configuration["Jwt:Key"] 
    ?? throw new InvalidOperationException("JWT key is missing in configuration");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    Console.WriteLine("JWT Key: " + jwtKey);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero // Disable the default 5 minute clock skew
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // your Angular app URL
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // optional, only if you're sending cookies
    });
});

// builder.Services.AddAuthorization();

builder.Services.AddScoped<TokenService>();

builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontendDev");
// 👇 These are required for JWT to work
app.UseAuthentication(); // validates token
app.UseAuthorization();  // checks policies/roles

//adds all controller having [ApiCOntroller]
app.MapControllers();

app.Run();


