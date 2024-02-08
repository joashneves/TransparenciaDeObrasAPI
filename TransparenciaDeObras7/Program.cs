using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infraestrutura;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using System.Text;
using System.Threading;
using System.Threading.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using Microsoft.AspNetCore;

namespace TransparenciaDeObras7
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Limiter 
            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter(policyName: "fixed", configureOptions =>
                {
                    configureOptions.PermitLimit = 50;
                    configureOptions.Window = TimeSpan.FromSeconds(10);
                    configureOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    configureOptions.QueueLimit = 10;
                })
                .OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        await context.HttpContext.Response.WriteAsync($"{retryAfter.TotalMinutes}Minutos",cancellationToken: token);
                    }else
                    {
                        await context.HttpContext.Response.WriteAsync($"Muitas requisições", cancellationToken: token);
                    }
                };
            });
            // Add services to the container.
            builder.Services.AddDbContext<UserContext>();

            builder.Services.AddDbContext<ObraContext>();

            builder.Services.AddDbContext<MedicaoContext>();

            builder.Services.AddDbContext<FotoContext>();

            builder.Services.AddDbContext<AditivoContext>();

            builder.Services.AddDbContext<AnexoContext>();

            builder.Services.AddDbContext<FiscalGestorContext>();

            builder.Services.AddDbContext<HistoricoContext>();

         
            // CORS
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy(name: "MyPolicy",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173",
                            "https://localhost:7067/User",
                            "http://172.31.254.8:5173/").AllowAnyHeader().AllowAnyMethod();
                    });
            });

            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy(name: "OtherPolicy",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .WithMethods("GET");
                    });
            });

            // TOKEN
            var key = Encoding.ASCII.GetBytes(Key.Secret);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            builder.Services.AddSwaggerGen(c =>
            {

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (!app.Environment.IsProduction())
            {
                app.Use((context, next) =>
                {
                    context.Request.Scheme = "https";
                    return next(context);
                });
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("MyPolicy");
            app.UseCors("OtherPolicy");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            // Rate Limiter
            app.UseRateLimiter();
            app.MapDefaultControllerRoute().RequireRateLimiting("fixed");


            app.MapControllers();

            app.Run();
        }

    }




}
