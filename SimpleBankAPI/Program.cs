using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimpleBankAPI.Clients;
using SimpleBankAPI.Interfaces;
using SimpleBankAPI.Repositories;
using SimpleBankAPI.Services;
using SimpleBankAPI.Data;
using SimpleBankAPI.Factories;

var builder = WebApplication.CreateBuilder(args);
var currencyKey = builder.Configuration.GetValue<string>("CURRENCY_API_KEY");

var services = builder.Services;

services.AddControllers();
services.AddDbContext<AccountContext>(opt =>
    opt.UseInMemoryDatabase("Accounts"));
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddSingleton<ICurrencyRate, CurrencyClient>();
services.AddHttpClient<ICurrencyRate, CurrencyClient>(client => 
    client.BaseAddress = new Uri($"https://api.freecurrencyapi.com/v1/latest?apikey={currencyKey}"));
services.AddSingleton<IFactory<IValidator?>, ValidatorFactory>();
services.AddTransient<IAccountsRepository, AccountsRepository>();
services.AddTransient<IAccountsService, AccountsService>();
services.AddSwaggerGen(opt =>
{
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, 
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "SimpleBankAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
services.AddAutoMapper(typeof(Program).Assembly);
services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Authentication:Issuer"],
                ValidAudience = builder.Configuration["Authentication:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
            };
        }
    );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();