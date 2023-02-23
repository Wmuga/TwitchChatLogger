using Backend.ChatListener;
using Backend.Context;
using Backend.Service;
namespace Backend
{
    public class Program
    {
        private static void WebApp(string[] args)
        {
            var allowOrigs = "_allowOrgis";

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddDbContext<ChatContext>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: allowOrigs, policy =>
                {
                    policy.WithOrigins("*");
                });
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IDbService, DBService>();
            builder.Services.AddSingleton<ITwitchIrcClient, TwitchIrcClient>();
            builder.Services.AddSingleton<IListener,Listener>();
			builder.Services.AddSingleton<ILoggerService,LoggerService>();

            var app = builder.Build();

            Shared.app = app;

            app.MapGet("/", () => "Nice test");

            app.UseCors(allowOrigs);
            app.MapControllers();

            // ip:port/swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.Run();
        }

        public static void Main(string[] args)
        {
            WebApp(args);
        }
    }

    public static class Shared
    {
        public static WebApplication app;
    }
}