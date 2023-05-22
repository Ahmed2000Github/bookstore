using Core.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes();
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Authoraizition header",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddMemoryCache();
builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

//Add Jwt Configurations
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]))
        };

    });
//Add Database configurations
var connectionString = builder.Configuration.GetConnectionString("WebApiDatabase");
builder.Services.AddDbContext<DataContext>(
    o => o.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddIdentity<AppUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<DataContext>();
// change password options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();

//dotnet ef migrations add initialeCreate -s ..\BookStoreAPI\BookStoreAPI.csproj
//dotnet ef database update -s ..\BookStoreAPI\BookStoreAPI.csproj