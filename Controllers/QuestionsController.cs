using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly QuizDbContext _context;
        public QuestionsController(QuizDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var questions = await _context.Questions.Include(q => q.Options).ToListAsync();
            return View(questions);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Question question, List<string> optionTexts, List<int> correctOptionIndexes)
        {
            for (int i = 0; i < optionTexts.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(optionTexts[i])) continue;
                question.Options.Add(new Option
                {
                    Text = optionTexts[i],
                    IsCorrect = correctOptionIndexes.Contains(i)
                });
            }

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}