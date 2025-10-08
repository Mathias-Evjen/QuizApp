using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.DAL;
using QuizApp.Models;

namespace QuizApp.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly AppDbContext _context;

        public QuestionsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var questions = await _context.MultipleChoices
                .Include(mc => mc.Options)
                .ToListAsync();
            return View(questions);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(MultipleChoice question,
                                                List<string> optionTexts,
                                                List<int> correctOptionIndexes)
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

            _context.MultipleChoices.Add(question);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
