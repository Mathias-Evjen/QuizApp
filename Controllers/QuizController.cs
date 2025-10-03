using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.Controllers
{
    public class QuizController : Controller
    {
        private readonly QuizDbContext _context;
        public QuizController(QuizDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var questions = await _context.Questions.Include(q => q.Options).ToListAsync();
            return View(questions);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(Dictionary<int, string[]> answers)
        {
            var questions = await _context.Questions.Include(q => q.Options).ToListAsync();
            var attempt = new QuizAttempt { Total = questions.Count };

            foreach (var q in questions)
            {
                var correct = q.Options.Where(o => o.IsCorrect).Select(o => o.Text).ToList();
                var selected = answers.ContainsKey(q.Id) ? answers[q.Id].ToList() : new List<string>();

                var aq = new AnsweredQuestion
                {
                    QuestionId = q.Id,
                    QuestionText = q.Text,
                    CorrectAnswers = correct,
                    SelectedAnswers = selected,
                    IsCorrect = correct.OrderBy(c => c).SequenceEqual(selected.OrderBy(s => s))
                };

                if (aq.IsCorrect) attempt.Answers.Count(a => a.IsCorrect);
                attempt.Answers.Add(aq);
            }

            return View("Result", attempt);
        }
    }
}