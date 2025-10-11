using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;

namespace QuizApp.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IMultipleChoiceRepository _repo;

        public QuestionsController(IMultipleChoiceRepository repo)
        {
            _repo = repo;
        }

        // GET: /Questions
        public async Task<IActionResult> Index()
        {
            var questions = await _repo.GetAllAsync();
            return View(questions); // bruker Views/Questions/Index.cshtml
        }

        // GET: /Questions/Create
        public IActionResult Create() => View();

        // POST: /Questions/Create
        public async Task<IActionResult> Create(MultipleChoice question)
        {
            if (ModelState.IsValid)
            {
                await _repo.AddAsync(question);
                await _repo.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        // GET: /Questions/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var question = await _repo.GetDetailedAsync(id);
            if (question == null) return NotFound();
            return View(question);
        }

        // POST: /Questions/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(MultipleChoice question)
        {
            if (ModelState.IsValid)
            {
                await _repo.UpdateAsync(question);
                await _repo.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        // GET: /Questions/Delete
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _repo.GetDetailedAsync(id);
            if (question == null)
                return NotFound();

            return View(question);
        }

        // POST: /Questions/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id, IFormCollection form)
        {
            await _repo.DeleteAsync(id);
            await _repo.SaveAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}