using Microsoft.AspNetCore.Mvc;

namespace InternTimeStamp.Controllers
{
    public class InternStudentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
