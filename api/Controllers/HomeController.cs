using Microsoft.AspNetCore.Mvc;

namespace QuizApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return RedirectToAction("Quizzes", "Quiz");
        }
    }
}
