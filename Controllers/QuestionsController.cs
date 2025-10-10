using Microsoft.AspNetCore.Mvc;
using QuizApp.DAL;
using QuizApp.Models;

namespace QuizApp.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IMultipleChoiceRepository _repo;
        private readonly AppDbContext _context;

        public QuestionsController(IMultipleChoiceRepository repo, AppDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var questions = await _repo.GetAllAsync();
            return View(questions);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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

        public async Task<IActionResult> Edit(int id)
        {
            var question = await _repo.GetDetailedAsync(id);
            if (question == null)
                return NotFound();

            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MultipleChoice updatedQuestion)
        {
            if (id != updatedQuestion.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(updatedQuestion);

            var existingQuestion = await _repo.GetDetailedAsync(id);
            if (existingQuestion == null)
                return NotFound();

            existingQuestion.QuestionText = updatedQuestion.QuestionText;

            foreach (var existingOption in existingQuestion.Options.ToList())
            {
                var updatedOption = updatedQuestion.Options.FirstOrDefault(o => o.Id == existingOption.Id);
                if (updatedOption != null)
                {
                    existingOption.Text = updatedOption.Text;
                    existingOption.IsCorrect = updatedOption.IsCorrect;
                }
                else
                {
                    _context.Options.Remove(existingOption);
                }
            }

            if (Request.Form.ContainsKey("optionTexts"))
            {
                var newOptionTexts = Request.Form["optionTexts"];
                var correctIndexes = Request.Form["correctOptionIndexes"];

                for (int i = 0; i < newOptionTexts.Count; i++)
                {
                    var text = newOptionTexts[i];
                    bool isCorrect = correctIndexes.Contains(i.ToString());

                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        existingQuestion.Options.Add(new Option
                        {
                            Text = text,
                            IsCorrect = isCorrect
                        });
                    }
                }
            }

            await _repo.UpdateAsync(existingQuestion);
            await _repo.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var question = await _repo.GetByIdAsync(id);
            if (question == null)
                return NotFound();

            return View(question);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
