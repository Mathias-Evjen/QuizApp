using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.DAL;
using QuizApp.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Controllers
{
    public class QuizController : Controller
    {
        private readonly AppDbContext _context;

        public QuizController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Quiz
        public async Task<IActionResult> Index()
        {
            var questions = await _context.MultipleChoices
                .Include(q => q.Options)
                .ToListAsync();
            return View(questions);
        }

        // POST: /Quiz/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Dictionary<int, string[]> answers)
        {
            var questions = await _context.MultipleChoices
                .Include(q => q.Options)
                .ToListAsync();

            var result = new QuizResultViewModel
            {
                TotalQuestions = questions.Count
            };

            foreach (var q in questions)
            {
                var correct = q.Options.Where(o => o.IsCorrect).Select(o => o.Text).ToList();
                var selected = answers.ContainsKey(q.Id)
                    ? answers[q.Id].ToList()
                    : new();

                var isCorrect = correct.OrderBy(c => c).SequenceEqual(selected.OrderBy(s => s));

                result.Results.Add(new ResultQuestion
                {
                    QuestionText = q.QuestionText,
                    CorrectAnswers = correct,
                    UserAnswers = selected,
                    IsCorrect = isCorrect
                });

                if (isCorrect)
                    result.CorrectAnswers++;
            }

            return View("Result", result);
        }
    }
}
