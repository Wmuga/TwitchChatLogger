using Frontend.Services;
using Frontend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Frontend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // TODO: input from file
            Shared.AppSettings = new AppSettings
            {
                BackendURIBase = "http://localhost:5260",
                Channels = new[] { "#iarspider", "#wmuga" },
            };
            Shared.FrontSettings = new FrontSettings
            {
                Channels = new List<string>(Shared.AppSettings.Channels)
            };

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ILoggerService,LoggerService>();
			var app = builder.Build();
            Shared.app = app;
            app.UseStaticFiles();
            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/");

			app.Run();
        }
    }

    public static class Shared
    {
        public static FrontSettings FrontSettings;
        public static AppSettings AppSettings;
        public static bool listened = false;
        public static WebApplication app;

		public static async Task<int> AddListen(ILoggerService logger)
		{
            if (listened) return 0;
            listened = true;

			HttpClient client = new()
			{
				BaseAddress = new Uri(AppSettings.BackendURIBase)
			};
			for (int i = 0; i < AppSettings.Channels.Length; i++)
			{
				var channel = AppSettings.Channels[i];
                if (channel.StartsWith('#')) channel = channel[1..];
				var resp = await client.PostAsync($"/Listen?channel={channel}", null);
                logger.Log($"{channel} : {resp.StatusCode}");
			}

            return 0;
		}
	}
}