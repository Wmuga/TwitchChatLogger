using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
	public class SettingsController : Controller
	{
		public Models.FrontSettings Index()
		{
			return Shared.FrontSettings;
		}
	}
}
