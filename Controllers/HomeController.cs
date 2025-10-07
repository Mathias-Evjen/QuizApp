using Microsoft.AspNetCore.Mvc;

namespace QuizApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Redirecter direkte til Quiz/Index
            return View(); //RedirectToAction("Index", "Quiz");
        }
    }
}