using Microsoft.AspNetCore.Mvc;

namespace QuizApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Kan f.eks. vise en velkomstside eller sende deg til quiz
            return RedirectToAction("Index", "Quiz");
        }
    }
}
