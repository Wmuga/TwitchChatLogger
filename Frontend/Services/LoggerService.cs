namespace Frontend.Services
{
	public interface ILoggerService
	{
		public void Log(string s, bool endl = true);
	}
	public class LoggerService : ILoggerService
	{
		public void Log(string s, bool endl = true)
		{
			if (Shared.app.Environment.IsDevelopment())
			{
				if (endl) Console.WriteLine(s);
				else Console.Write(s);
			}
		}
	}
}
