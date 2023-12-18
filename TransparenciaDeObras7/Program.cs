
using Infraestrutura;
using Microsoft.AspNetCore.RateLimiting;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Threading.RateLimiting;

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
                    configureOptions.PermitLimit = 4;
                    configureOptions.Window = TimeSpan.FromSeconds(10);
                    configureOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    configureOptions.QueueLimit = 5;
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

            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy(name: "MyPolicy",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173",
                            "https://localhost:7067/User").AllowAnyHeader().AllowAnyMethod() ;
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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("MyPolicy");
            app.UseCors("OtherPolicy");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Rate Limiter
            app.UseRateLimiter();
            app.MapDefaultControllerRoute().RequireRateLimiting("fixed");


            app.MapControllers();

            app.Run();
        }
    }
}
