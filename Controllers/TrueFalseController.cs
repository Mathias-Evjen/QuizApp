using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;
using System.Threading.Tasks;

namespace QuizApp.Controllers
{
    public class TrueFalseController : Controller
    {
        private readonly ITrueFalseRepository _repo;

        public TrueFalseController(ITrueFalseRepository repo)
        {
            _repo = repo;
        }

        // GET: /TrueFalse
        public async Task<IActionResult> Index()
        {
            var questions = await _repo.GetAllAsync();
            return View(questions);
        }

        // GET: /TrueFalse/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /TrueFalse/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrueFalseQuestion question)
        {
            if (!ModelState.IsValid)
                return View(question);

            await _repo.AddAsync(question);
            await _repo.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /TrueFalse/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var question = await _repo.GetByIdAsync(id);
            if (question == null)
                return NotFound();

            return View(question);
        }

        // POST: /TrueFalse/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrueFalseQuestion updatedQuestion)
        {
            if (id != updatedQuestion.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(updatedQuestion);

            var existingQuestion = await _repo.GetByIdAsync(id);
            if (existingQuestion == null)
                return NotFound();

            // Oppdater felter
            existingQuestion.QuestionText = updatedQuestion.QuestionText;
            existingQuestion.CorrectAnswer = updatedQuestion.CorrectAnswer;

            await _repo.UpdateAsync(existingQuestion);
            await _repo.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /TrueFalse/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _repo.GetByIdAsync(id);
            if (question == null)
                return NotFound();

            return View(question);
        }

        // POST: /TrueFalse/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, TrueFalseQuestion model)
        {
            await _repo.DeleteAsync(id);
            await _repo.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
