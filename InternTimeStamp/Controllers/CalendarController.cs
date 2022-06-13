using Microsoft.AspNetCore.Mvc;

namespace InternTimeStamp.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
