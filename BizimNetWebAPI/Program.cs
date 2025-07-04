﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.DependencyResolvers.Autofac;
using Core.Configuration;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using DataAccess.DependencyResolvers;
using Entities.Profiles.AutoMapperProfiles;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new AutofacBusinessModule());
    containerBuilder.RegisterModule(new AutoFacCoreModule());
    containerBuilder.RegisterModule(new AutoFacDataAccessModule());
});



builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("MongoDB"));
var database = mongoClient.GetDatabase("BizimNetDB");
builder.Services.AddSingleton(database);

builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// JWT Authentication
//var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidIssuer = tokenOptions.Issuer,
//            ValidAudience = tokenOptions.Audience,
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
//            LifetimeValidator = (notBefore, expires, token, param) => expires != null ? expires > DateTime.UtcNow : false,
//            NameClaimType = ClaimTypes.Name
//        };
//    });

// AutoMapper konfigürasyonu
builder.Services.AddAutoMapper(typeof(EntitiesAutoMapperProfile));

// Swagger konfigürasyonu
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BizimNet Web API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Core DI modülleri
//builder.Services.AddDependencyResolvers(new ICoreModule[] {
//    new CoreModule(),
//    new DataAccessModule(),
//    new BusinessModule()
//});

var app = builder.Build();

// Geliştirici hata sayfası
app.UseDeveloperExceptionPage();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BizimNet Web API v1");
});

// Özel exception middleware’in varsa burada çalıştır
//app.ConfigureCustomExceptionMiddleware();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowAngular");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
