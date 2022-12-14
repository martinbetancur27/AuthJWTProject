using AuthJWTWebAPI.Middleware;
using Core.DTO.Auth;
using Core.DTO.Response;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Identity.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("AuthReviewProjectConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAuthentication(config =>
    {
        config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

builder.Services.AddScoped<IUserRepository, UserRepositoryEFSqlServer>();
builder.Services.AddScoped<IRoleRepository, RoleRepositoryEFSqlServer>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepositoryEFSqlServer>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, IdentityTokenClaimService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRolesInUserService, RolesInUserService>();
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddScoped<IEncryptService, EncryptOfSystem>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseStatusCodePages(async statusCodeContext =>
{
    
    statusCodeContext.HttpContext.Response.ContentType = "application/json";

    await statusCodeContext.HttpContext.Response.WriteAsync(
        new ResponseGeneralDTO()
        {
            StatusCode = statusCodeContext.HttpContext.Response.StatusCode,
            Message = ""}.ToString());
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
