using API.Middlewares;
using Application.Mapper;
using Domain.Settings;
using Libs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using API.Central.Controllers;
using System.Text;
using System.Text.Json.Serialization;
public class Program
{

    static async Task Main(string[] args)
    {
        var builder = CreateBuilder(args);

        var certPassword = builder.Configuration["Kestrel:Endpoints:Https:Certificate:Password"];
        if (!string.IsNullOrEmpty(certPassword))
           builder.Configuration["Kestrel:Endpoints:Https:Certificate:Password"] = certPassword;

        var app = builder.Build();
        Configure(app, app.Environment);
        app.Run();
    }

    private static void AddAuthentication(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Central API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

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

            c.DescribeAllParametersInCamelCase();
            c.IgnoreObsoleteActions();
            c.IgnoreObsoleteProperties();
        });

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Crypto.Decrypt(builder.Configuration["Jwt:Key"])))
            };
        });

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));
        });
    }
    private static WebApplicationBuilder CreateBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Console.WriteLine(Environment.OSVersion.Platform.ToString());
        Console.WriteLine($"Current Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");

        AddAuthentication(builder);
        ConfigureServices(builder);
        ConfigureCORS(builder);
        PrintConfiguration(builder);

        return builder;
    }

    private static void ConfigureCORS(WebApplicationBuilder builder)
    {
        var allowedOrigins = builder.Configuration.Get<AppSettings>().Cors?.AllowedOrigins;

        builder.Services.AddCors(options =>
        {
            if (builder.Environment.IsDevelopment())
            {
                options.AddPolicy("AllowAnyOrigins", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            }
            else
            {
                options.AddPolicy("AllowMultipleOrigins", builder =>
                {
                    if (allowedOrigins != null && allowedOrigins.Count > 0)
                    {
                        builder.WithOrigins(allowedOrigins.ToArray())
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    }
                    else
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    }
                });
            }
        });
    }

    private static void PrintConfiguration(WebApplicationBuilder builder)
    {
        var settings = builder.Configuration.GetSection("").Get<AppSettings>();

        Console.WriteLine("CORS AllowedOrigins:");
        if (settings.Cors?.AllowedOrigins != null && settings.Cors.AllowedOrigins.Count > 0)
        {
            foreach (var origin in settings.Cors.AllowedOrigins)
            {
                Console.WriteLine($"  - {origin}");
            }
        }
        else
        {
            Console.WriteLine("  - Nenhuma origem CORS configurada.");
        }

        var https = settings.Kestrel?.Endpoints?.Https;

        Console.WriteLine("Kestrel Configurações:");

        if (!string.IsNullOrEmpty(https?.Url))
        {
            Console.WriteLine("  - HTTPS Configuração:");
            Console.WriteLine($"    - URL: {https.Url}");

            if (!string.IsNullOrEmpty(https.Certificate?.Path))
            {
                Console.WriteLine("    - Certificado:");
                Console.WriteLine($"      - Caminho: {https.Certificate.Path}");
            }
        }
        else
        {
            Console.WriteLine("  - Nenhuma configuração HTTPS encontrada.");
        }
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        var appSettings = builder.Configuration.Get<AppSettings>();

        if (appSettings?.Connections != null)
        {
            foreach (var connection in appSettings.Connections)
            {
                if (!string.IsNullOrEmpty(connection?.Password))
                {
                    connection.Password = Crypto.Decrypt(connection.Password);
                }
            }
        }

        if (!string.IsNullOrEmpty(appSettings?.Kestrel?.Endpoints?.Https?.Certificate?.Password))
        {
            appSettings.Kestrel.Endpoints.Https.Certificate.Password = Crypto.Decrypt(appSettings.Kestrel.Endpoints.Https.Certificate.Password);
        }

        builder.Services.Configure<AppSettings>(options =>
        {
            options.Connections = appSettings.Connections;
            options.Kestrel = appSettings.Kestrel;
        });


        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
            });

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<GlobalExceptionFilter>();
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHttpClient();

        Application.Base.Configuration.RegisterServices(builder.Services);
    }


    private static void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseCors("AllowAnyOrigins");
        else
            app.UseCors("AllowMultipleOrigins");

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }

        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/")
            {
                context.Response.Redirect("/swagger/index.html");
                return;
            }
            await next();
        });

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.UseMiddleware<HttpUserMiddleware>();
    }

}