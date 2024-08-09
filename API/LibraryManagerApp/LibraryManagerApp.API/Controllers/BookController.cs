using Microsoft.AspNetCore.Mvc;

namespace LibraryManagerApp.API.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
